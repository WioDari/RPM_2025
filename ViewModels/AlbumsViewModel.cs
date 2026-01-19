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
    public class AlbumsViewModel : BaseViewModel
    {
        private readonly ApplicationDbContext _context;
        private User _currentUser;
        private ObservableCollection<Album> _albums;
        private ObservableCollection<Album> _filteredAlbums;
        private string _searchText = "";
        private string _selectedGenre = "Все жанры";
        private ObservableCollection<string> _genres;
        private string _sortOrder = "По умолчанию";
        private ObservableCollection<string> _sortOptions;

        public AlbumsViewModel(User user)
        {
            _currentUser = user;
            
            var connectionString = "Server=localhost;Database=musicplus_db;User=root;Password=12345;Port=3306;CharSet=utf8mb4;";
            var serverVersion = ServerVersion.Create(new Version(8, 0, 21), ServerType.MySql);
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseMySql(connectionString, serverVersion)
                .Options;
            
            _context = new ApplicationDbContext(options);

            _albums = new ObservableCollection<Album>();
            _filteredAlbums = new ObservableCollection<Album>();
            _genres = new ObservableCollection<string> { "Все жанры" };
            _sortOptions = new ObservableCollection<string>
            {
                "По умолчанию",
                "По названию",
                "По артисту",
                "По году",
                "По продолжительности (возрастание)",
                "По продолжительности (убывание)"
            };

            LoadAlbumsCommand = new RelayCommand(_ => LoadAlbums());
            SearchCommand = new RelayCommand(_ => ApplyFilters());
            FilterByGenreCommand = new RelayCommand(_ => ApplyFilters());
            SortCommand = new RelayCommand(_ => ApplyFilters());
            ViewAlbumDetailsCommand = new RelayCommand(album => ViewAlbumDetails(album as Album));

            LoadAlbums();
        }

        public ObservableCollection<Album> Albums => _filteredAlbums;
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
        public bool CanAddEdit => _currentUser.Role == "Admin";

        public ICommand LoadAlbumsCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand FilterByGenreCommand { get; }
        public ICommand SortCommand { get; }
        public ICommand ViewAlbumDetailsCommand { get; }

        public event EventHandler<Album>? AlbumSelected;

        private void LoadAlbums()
        {
            try
            {
                _albums.Clear();
                _filteredAlbums.Clear();

                var albums = _context.Albums
                    .Include(a => a.Artist)
                    .Include(a => a.AlbumGenres)
                        .ThenInclude(ag => ag.Genre)
                    .ToList();

                foreach (var album in albums)
                {
                    _albums.Add(album);
                }

                _genres.Clear();
                _genres.Add("Все жанры");
                var allGenres = _context.Genres.Select(g => g.GenreName).Distinct().OrderBy(g => g).ToList();
                foreach (var genre in allGenres)
                {
                    _genres.Add(genre);
                }
                OnPropertyChanged(nameof(Genres));

                ApplyFilters();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Ошибка загрузки альбомов: {ex.Message}", "Ошибка", 
                    System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        private void ApplyFilters()
        {
            _filteredAlbums.Clear();

            var filtered = _albums.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(_searchText))
            {
                var searchLower = _searchText.ToLower();
                filtered = filtered.Where(a => 
                    a.AlbumTitle.ToLower().Contains(searchLower) ||
                    (a.Artist?.ArtistName ?? "").ToLower().Contains(searchLower));
            }

            if (_selectedGenre != null && _selectedGenre != "Все жанры")
            {
                filtered = filtered.Where(a => 
                    a.AlbumGenres.Any(ag => ag.Genre?.GenreName == _selectedGenre));
            }

            switch (_sortOrder)
            {
                case "По продолжительности (возрастание)":
                    filtered = filtered.OrderBy(a => a.TotalDuration);
                    break;
                case "По продолжительности (убывание)":
                    filtered = filtered.OrderByDescending(a => a.TotalDuration);
                    break;
                case "По названию":
                    filtered = filtered.OrderBy(a => a.AlbumTitle);
                    break;
                case "По артисту":
                    filtered = filtered.OrderBy(a => a.Artist?.ArtistName ?? "");
                    break;
                case "По году":
                    filtered = filtered.OrderByDescending(a => a.ReleaseYear ?? 0);
                    break;
            }

            foreach (var album in filtered)
            {
                _filteredAlbums.Add(album);
            }

            OnPropertyChanged(nameof(Albums));
        }

        private void ViewAlbumDetails(Album? album)
        {
            if (album == null || IsGuest) return;
            
            AlbumSelected?.Invoke(this, album);
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
}
