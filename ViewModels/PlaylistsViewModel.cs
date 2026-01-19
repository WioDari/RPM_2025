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
    public class PlaylistsViewModel : BaseViewModel
    {
        private readonly ApplicationDbContext _context;
        private User _currentUser;
        private ObservableCollection<PlaylistViewModel> _playlists;
        private ObservableCollection<PlaylistViewModel> _filteredPlaylists;
        private string _searchText = "";
        private string _selectedSortOrder = "По умолчанию";
        private ObservableCollection<string> _sortOptions;

        public PlaylistsViewModel(User user)
        {
            _currentUser = user;
            
            var connectionString = "Server=localhost;Database=musicplus_db;User=root;Password=12345;Port=3306;CharSet=utf8mb4;";
            var serverVersion = ServerVersion.Create(new Version(8, 0, 21), ServerType.MySql);
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseMySql(connectionString, serverVersion)
                .Options;
            
            _context = new ApplicationDbContext(options);

            _playlists = new ObservableCollection<PlaylistViewModel>();
            _filteredPlaylists = new ObservableCollection<PlaylistViewModel>();
            _sortOptions = new ObservableCollection<string>
            {
                "По умолчанию",
                "По названию",
                "По автору",
                "По дате создания",
                "По количеству лайков (возрастание)",
                "По количеству лайков (убывание)",
                "По количеству треков (возрастание)",
                "По количеству треков (убывание)"
            };

            LoadPlaylistsCommand = new RelayCommand(_ => LoadPlaylists());
            ViewPlaylistDetailsCommand = new RelayCommand(playlist => ViewPlaylistDetails(playlist as PlaylistViewModel));
            AddPlaylistCommand = new RelayCommand(_ => AddPlaylist());
            EditPlaylistCommand = new RelayCommand(playlist => EditPlaylist(playlist as PlaylistViewModel));
            DeletePlaylistCommand = new RelayCommand(playlist => DeletePlaylist(playlist as PlaylistViewModel));

            LoadPlaylists();
        }

        public ObservableCollection<PlaylistViewModel> Playlists => _filteredPlaylists;
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

        public string SelectedSortOrder
        {
            get => _selectedSortOrder;
            set
            {
                _selectedSortOrder = value;
                OnPropertyChanged();
                ApplyFilters();
            }
        }

        public bool CanAddEdit => _currentUser.Role == "Admin" || _currentUser.Role == "Manager";
        public bool CanDeleteAll => _currentUser.Role == "Admin";

        public ICommand LoadPlaylistsCommand { get; }
        public ICommand ViewPlaylistDetailsCommand { get; }
        public ICommand AddPlaylistCommand { get; }
        public ICommand EditPlaylistCommand { get; }
        public ICommand DeletePlaylistCommand { get; }

        public event EventHandler<PlaylistViewModel>? PlaylistSelected;
        public event EventHandler? AddPlaylistRequested;
        public event EventHandler<PlaylistViewModel>? EditPlaylistRequested;
        public event EventHandler? RefreshRequested;

        private void LoadPlaylists()
        {
            try
            {
                _playlists.Clear();

                var playlists = _context.Playlists
                    .Include(p => p.User)
                    .Include(p => p.PlaylistTracks)
                        .ThenInclude(pt => pt.Track)
                    .Include(p => p.PlaylistSubscriptions)
                    .ToList();

                foreach (var playlist in playlists)
                {

                    bool canEdit = CanEditPlaylist(playlist);
                    bool canDelete = CanDeletePlaylist(playlist);

                    var playlistVm = new PlaylistViewModel
                    {
                        PlaylistID = playlist.PlaylistID,
                        PlaylistName = playlist.PlaylistName,
                        CreatorName = playlist.User?.FullName ?? "Неизвестно",
                        CreatorLogin = playlist.User?.UserLogin ?? "",
                        DateCreated = playlist.DateCreated ?? DateTime.Now,
                        Likes = playlist.Likes,
                        TrackCount = playlist.PlaylistTracks?.Count ?? 0,
                        SubscriberCount = playlist.PlaylistSubscriptions?.Count ?? 0,
                        TotalDuration = playlist.PlaylistTracks?.Sum(pt => pt.Track?.Duration ?? 0) ?? 0,
                        IsPremium = playlist.User?.Subscription == "Premium",
                        CanEdit = canEdit,
                        CanDelete = canDelete,
                        Description = playlist.Description ?? ""
                    };

                    _playlists.Add(playlistVm);
                }

                ApplyFilters();
                OnPropertyChanged(nameof(Playlists));
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Ошибка загрузки плейлистов: {ex.Message}", "Ошибка", 
                    System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        private bool CanEditPlaylist(Playlist playlist)
        {
            if (_currentUser.Role == "Admin") return true;
            if (_currentUser.Role == "Manager" && playlist.UserID == _currentUser.UserID) return true;
            return false;
        }

        private bool CanDeletePlaylist(Playlist playlist)
        {
            if (_currentUser.Role == "Admin") return true;
            if (_currentUser.Role == "Manager" && playlist.UserID == _currentUser.UserID) return true;
            return false;
        }

        private void ViewPlaylistDetails(PlaylistViewModel? playlist)
        {
            if (playlist == null) return;
            
            PlaylistSelected?.Invoke(this, playlist);
        }

        private void AddPlaylist()
        {
            AddPlaylistRequested?.Invoke(this, EventArgs.Empty);
        }

        private void EditPlaylist(PlaylistViewModel? playlist)
        {
            if (playlist == null || !playlist.CanEdit) return;
            
            EditPlaylistRequested?.Invoke(this, playlist);
        }

        private void DeletePlaylist(PlaylistViewModel? playlist)
        {
            if (playlist == null || !playlist.CanDelete) return;

            var result = System.Windows.MessageBox.Show(
                $"Вы уверены, что хотите удалить плейлист \"{playlist.PlaylistName}\"?",
                "Подтверждение удаления",
                System.Windows.MessageBoxButton.YesNo,
                System.Windows.MessageBoxImage.Question);

            if (result == System.Windows.MessageBoxResult.Yes)
            {
                try
                {
                    var playlistToDelete = _context.Playlists.Find(playlist.PlaylistID);
                    if (playlistToDelete != null)
                    {
                        _context.Playlists.Remove(playlistToDelete);
                        _context.SaveChanges();
                        LoadPlaylists();
                        RefreshRequested?.Invoke(this, EventArgs.Empty);
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show($"Ошибка удаления плейлиста: {ex.Message}", "Ошибка", 
                        System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                }
            }
        }

        private void ApplyFilters()
        {
            _filteredPlaylists.Clear();

            var filtered = _playlists.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(_searchText))
            {
                var searchLower = _searchText.ToLower();
                filtered = filtered.Where(p =>
                    p.PlaylistName.ToLower().Contains(searchLower) ||
                    p.CreatorName.ToLower().Contains(searchLower) ||
                    (p.Description != null && p.Description.ToLower().Contains(searchLower)));
            }

            switch (_selectedSortOrder)
            {
                case "По названию":
                    filtered = filtered.OrderBy(p => p.PlaylistName);
                    break;
                case "По автору":
                    filtered = filtered.OrderBy(p => p.CreatorName);
                    break;
                case "По дате создания":
                    filtered = filtered.OrderByDescending(p => p.DateCreated ?? DateTime.MinValue);
                    break;
                case "По количеству лайков (возрастание)":
                    filtered = filtered.OrderBy(p => p.Likes);
                    break;
                case "По количеству лайков (убывание)":
                    filtered = filtered.OrderByDescending(p => p.Likes);
                    break;
                case "По количеству треков (возрастание)":
                    filtered = filtered.OrderBy(p => p.TrackCount);
                    break;
                case "По количеству треков (убывание)":
                    filtered = filtered.OrderByDescending(p => p.TrackCount);
                    break;
                default:
                    filtered = filtered.OrderBy(p => p.PlaylistName);
                    break;
            }

            foreach (var playlist in filtered)
            {
                _filteredPlaylists.Add(playlist);
            }

            OnPropertyChanged(nameof(Playlists));
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

    public class PlaylistViewModel : BaseViewModel
    {
        private int _playlistID;
        private string _playlistName = "";
        private string _creatorName = "";
        private string _creatorLogin = "";
        private DateTime? _dateCreated;
        private int _likes;
        private int _trackCount;
        private int _subscriberCount;
        private int _totalDuration;
        private bool _isPremium;
        private bool _canEdit;
        private bool _canDelete;
        private string _description = "";

        public int PlaylistID
        {
            get => _playlistID;
            set { _playlistID = value; OnPropertyChanged(); }
        }

        public string PlaylistName
        {
            get => _playlistName;
            set { _playlistName = value; OnPropertyChanged(); }
        }

        public string CreatorName
        {
            get => _creatorName;
            set { _creatorName = value; OnPropertyChanged(); }
        }

        public string CreatorLogin
        {
            get => _creatorLogin;
            set { _creatorLogin = value; OnPropertyChanged(); }
        }

        public DateTime? DateCreated
        {
            get => _dateCreated;
            set { _dateCreated = value; OnPropertyChanged(); }
        }

        public int Likes
        {
            get => _likes;
            set { _likes = value; OnPropertyChanged(); }
        }

        public int TrackCount
        {
            get => _trackCount;
            set { _trackCount = value; OnPropertyChanged(); }
        }

        public int SubscriberCount
        {
            get => _subscriberCount;
            set { _subscriberCount = value; OnPropertyChanged(); }
        }

        public int TotalDuration
        {
            get => _totalDuration;
            set { _totalDuration = value; OnPropertyChanged(); }
        }

        public bool IsPremium
        {
            get => _isPremium;
            set { _isPremium = value; OnPropertyChanged(); }
        }

        public bool CanEdit
        {
            get => _canEdit;
            set { _canEdit = value; OnPropertyChanged(); }
        }

        public bool CanDelete
        {
            get => _canDelete;
            set { _canDelete = value; OnPropertyChanged(); }
        }

        public string Description
        {
            get => _description;
            set { _description = value; OnPropertyChanged(); }
        }
    }
}
