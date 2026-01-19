using MusicPlus.ViewModels;
using MusicPlus.Models;
using MusicPlus.Data;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace MusicPlus.ViewModels
{
    public class TracksViewModel : BaseViewModel
    {
        private readonly ApplicationDbContext _context;
        private User _currentUser;
        private ObservableCollection<TrackViewModel> _tracks;
        private ObservableCollection<TrackViewModel> _filteredTracks;
        private string _searchText = "";
        private string _selectedGenre = "Все жанры";
        private ObservableCollection<string> _genres;
        private string _sortOrder = "По умолчанию";
        private ObservableCollection<string> _sortOptions;

        public TracksViewModel(User user)
        {
            _currentUser = user;
            
            var connectionString = "Server=localhost;Database=musicplus_db;User=root;Password=12345;Port=3306;CharSet=utf8mb4;";
            var serverVersion = ServerVersion.Create(new Version(8, 0, 21), ServerType.MySql);
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseMySql(connectionString, serverVersion)
                .Options;
            
            _context = new ApplicationDbContext(options);

            _tracks = new ObservableCollection<TrackViewModel>();
            _filteredTracks = new ObservableCollection<TrackViewModel>();
            _genres = new ObservableCollection<string> { "Все жанры" };
            _sortOptions = new ObservableCollection<string>
            {
                "По умолчанию",
                "По названию",
                "По исполнителю",
                "По альбому",
                "По продолжительности (возрастание)",
                "По продолжительности (убывание)",
                "По рейтингу (возрастание)",
                "По рейтингу (убывание)"
            };

            LoadTracksCommand = new RelayCommand(_ => LoadTracks());
            ViewTrackDetailsCommand = new RelayCommand(track => ViewTrackDetails(track as TrackViewModel));

            LoadTracks();
        }

        public ObservableCollection<TrackViewModel> Tracks => _filteredTracks;
        public ObservableCollection<string> Genres => _genres;
        public ObservableCollection<string> SortOptions => _sortOptions;

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                ApplyFilters();
            }
        }

        public string SelectedGenre
        {
            get => _selectedGenre;
            set
            {
                _selectedGenre = value;
                OnPropertyChanged();
                ApplyFilters();
            }
        }

        public string SortOrder
        {
            get => _sortOrder;
            set
            {
                _sortOrder = value;
                OnPropertyChanged();
                ApplyFilters();
            }
        }

        public bool IsGuest => _currentUser.Role == "Guest";
        public bool IsAdmin => _currentUser.Role == "Admin";

        public ICommand LoadTracksCommand { get; }
        public ICommand ViewTrackDetailsCommand { get; }

        public event EventHandler<TrackViewModel>? TrackSelected;

        private void LoadTracks()
        {
            try
            {
                _tracks.Clear();

                var tracks = _context.Tracks
                    .Include(t => t.TrackArtists)
                        .ThenInclude(ta => ta.Artist)
                    .Include(t => t.Album)
                        .ThenInclude(a => a.AlbumGenres)
                            .ThenInclude(ag => ag.Genre)
                    .OrderBy(t => t.TrackName)
                    .ToList();

                foreach (var track in tracks)
                {
                    var trackVm = new TrackViewModel
                    {
                        TrackID = track.TrackID,
                        TrackName = track.TrackName,
                        Duration = track.Duration,
                        Rating = track.Rating ?? 0,
                        ArtistsText = GetTrackArtists(track),
                        AlbumTitle = track.Album?.AlbumTitle ?? "",
                        AlbumID = track.AlbumID,
                        Album = track.Album
                    };
                    _tracks.Add(trackVm);
                }

                var allGenres = _context.Genres
                    .OrderBy(g => g.GenreName)
                    .Select(g => g.GenreName)
                    .ToList();

                _genres.Clear();
                _genres.Add("Все жанры");
                foreach (var genre in allGenres)
                {
                    _genres.Add(genre);
                }

                ApplyFilters();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Ошибка загрузки треков: {ex.Message}", "Ошибка", 
                    System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        private string GetTrackArtists(Track track)
        {
            if (track.TrackArtists == null || !track.TrackArtists.Any())
                return "Не указано";
            
            return string.Join(", ", track.TrackArtists
                .Where(ta => ta.Artist != null)
                .Select(ta => ta.Artist!.ArtistName)
                .OrderBy(a => a));
        }

        private void ApplyFilters()
        {
            _filteredTracks.Clear();

            var filtered = _tracks.AsEnumerable();

            if (!IsGuest && !string.IsNullOrWhiteSpace(_searchText))
            {
                var searchLower = _searchText.ToLower();
                filtered = filtered.Where(t =>
                    t.TrackName.ToLower().Contains(searchLower) ||
                    t.ArtistsText.ToLower().Contains(searchLower) ||
                    (t.AlbumTitle != null && t.AlbumTitle.ToLower().Contains(searchLower)));
            }

            if (!IsGuest && _selectedGenre != "Все жанры")
            {
                filtered = filtered.Where(t =>
                    t.Album != null &&
                    t.Album.AlbumGenres != null &&
                    t.Album.AlbumGenres.Any(ag => ag.Genre != null && ag.Genre.GenreName == _selectedGenre));
            }

            if (!IsGuest)
            {
                switch (_sortOrder)
                {
                    case "По названию":
                        filtered = filtered.OrderBy(t => t.TrackName);
                        break;
                    case "По исполнителю":
                        filtered = filtered.OrderBy(t => t.ArtistsText);
                        break;
                    case "По альбому":
                        filtered = filtered.OrderBy(t => t.AlbumTitle ?? "");
                        break;
                    case "По продолжительности (возрастание)":
                        filtered = filtered.OrderBy(t => t.Duration);
                        break;
                    case "По продолжительности (убывание)":
                        filtered = filtered.OrderByDescending(t => t.Duration);
                        break;
                    case "По рейтингу (возрастание)":
                        filtered = filtered.OrderBy(t => t.Rating);
                        break;
                    case "По рейтингу (убывание)":
                        filtered = filtered.OrderByDescending(t => t.Rating);
                        break;
                    default:
                        filtered = filtered.OrderBy(t => t.TrackName);
                        break;
                }
            }

            foreach (var track in filtered)
            {
                _filteredTracks.Add(track);
            }

            OnPropertyChanged(nameof(Tracks));
        }

        private void ViewTrackDetails(TrackViewModel? track)
        {
            if (track == null) return;
            
            TrackSelected?.Invoke(this, track);
        }

        public ApplicationDbContext GetContext()
        {
            var connectionString = "Server=localhost;Database=musicplus_db;User=root;Password=12345;Port=3306;CharSet=utf8mb4;";
            var serverVersion = ServerVersion.Create(new Version(8, 0, 21), ServerType.MySql);
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseMySql(connectionString, serverVersion)
                .Options;
            
            return new ApplicationDbContext(options);
        }
    }

    public class TrackViewModel
    {
        public int TrackID { get; set; }
        public string TrackName { get; set; } = "";
        public int Duration { get; set; }
        public decimal Rating { get; set; }
        public string ArtistsText { get; set; } = "";
        public string AlbumTitle { get; set; } = "";
        public int? AlbumID { get; set; }
        public Album? Album { get; set; }
    }
}
