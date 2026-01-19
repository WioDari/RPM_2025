using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
using System.IO;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using MusicPlus.Models;
using MusicPlus.ViewModels;
using MusicPlus.Data;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace MusicPlus.Views
{
    public partial class PlaylistDetailView : Window, INotifyPropertyChanged
    {
        private PlaylistViewModel? _playlistViewModel;
        private User _currentUser;
        private ApplicationDbContext _context;

        public PlaylistDetailView(int playlistId, User currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser;
            DataContext = this;
            
            var connectionString = "Server=localhost;Database=musicplus_db;User=root;Password=12345;Port=3306;CharSet=utf8mb4;";
            var serverVersion = ServerVersion.Create(new Version(8, 0, 21), ServerType.MySql);
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseMySql(connectionString, serverVersion)
                .Options;
            
            _context = new ApplicationDbContext(options);
            
            LoadPlaylistData(playlistId);
        }

        public string PlaylistName => _playlistViewModel?.PlaylistName ?? "";
        public string CreatorName => _playlistViewModel?.CreatorName ?? "";
        public DateTime? DateCreated => _playlistViewModel?.DateCreated;
        public int Likes => _playlistViewModel?.Likes ?? 0;
        public string Description => _playlistViewModel?.Description ?? "";
        public string TagsText { get; private set; } = "Теги отсутствуют";
        public int TrackCount => Tracks.Count;
        public int TotalDuration => Tracks.Sum(t => t.Duration);
        public bool IsPremium => _playlistViewModel?.IsPremium ?? false;
        public bool CanEdit => _currentUser.Role == "Admin" || 
                               (_currentUser.Role == "Manager" && _playlistViewModel?.CreatorLogin == _currentUser.UserLogin);

        public ObservableCollection<PlaylistTrackViewModel> Tracks { get; } = new ObservableCollection<PlaylistTrackViewModel>();
        private PlaylistTrackViewModel? _selectedTrack;
        
        public PlaylistTrackViewModel? SelectedTrack
        {
            get => _selectedTrack;
            set
            {
                _selectedTrack = value;
                OnPropertyChanged();
            }
        }

        private void LoadPlaylistData(int playlistId)
        {
            try
            {
                var playlist = _context.Playlists
                    .Include(p => p.User)
                    .Include(p => p.PlaylistTracks)
                        .ThenInclude(pt => pt.Track)
                            .ThenInclude(t => t.TrackArtists)
                                .ThenInclude(ta => ta.Artist)
                    .Include(p => p.PlaylistTracks)
                        .ThenInclude(pt => pt.Track)
                            .ThenInclude(t => t.Album)
                    .Include(p => p.PlaylistTags)
                    .FirstOrDefault(p => p.PlaylistID == playlistId);

                if (playlist == null)
                {
                    MessageBox.Show("Плейлист не найден", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    this.Close();
                    return;
                }

                _playlistViewModel = new PlaylistViewModel
                {
                    PlaylistID = playlist.PlaylistID,
                    PlaylistName = playlist.PlaylistName,
                    CreatorName = playlist.User?.FullName ?? "Неизвестно",
                    CreatorLogin = playlist.User?.UserLogin ?? "",
                    DateCreated = playlist.DateCreated ?? DateTime.Now,
                    Likes = playlist.Likes,
                    Description = playlist.Description ?? "",
                    IsPremium = playlist.User?.Subscription == "Premium"
                };

                var playlistTags = _context.PlaylistTags
                    .Where(pt => pt.PlaylistID == playlist.PlaylistID)
                    .Select(pt => pt.TagName)
                    .ToList();
                TagsText = playlistTags.Any() ? string.Join(", ", playlistTags) : "Теги отсутствуют";

                Tracks.Clear();
                if (playlist.PlaylistTracks != null)
                {
                    foreach (var pt in playlist.PlaylistTracks.OrderBy(pt => pt.DateAdded ?? DateTime.MinValue))
                    {
                        var track = pt.Track;
                        if (track != null)
                        {
                            var trackVm = new PlaylistTrackViewModel
                            {
                                TrackID = track.TrackID,
                                TrackName = track.TrackName,
                                Duration = track.Duration,
                                Rating = track.Rating ?? 0,
                                ArtistsText = GetTrackArtists(track),
                                AlbumTitle = track.Album?.AlbumTitle ?? "",
                                AlbumCoverPath = track.AlbumCoverPath,
                                AlbumCoverBinary = track.AlbumCoverBinary,
                                Album = track.Album
                            };
                            Tracks.Add(trackVm);
                        }
                    }
                }

                OnPropertyChanged(nameof(PlaylistName));
                OnPropertyChanged(nameof(CreatorName));
                OnPropertyChanged(nameof(DateCreated));
                OnPropertyChanged(nameof(Likes));
                OnPropertyChanged(nameof(Description));
                OnPropertyChanged(nameof(TagsText));
                OnPropertyChanged(nameof(TrackCount));
                OnPropertyChanged(nameof(TotalDuration));
                OnPropertyChanged(nameof(IsPremium));
                OnPropertyChanged(nameof(CanEdit));
                OnPropertyChanged(nameof(Tracks));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных плейлиста: {ex.Message}", "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
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

        private void TrackCoverImage_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.Image image && image.DataContext is PlaylistTrackViewModel track)
            {
                image.Source = LoadTrackCover(track);
            }
        }

        private BitmapImage? LoadTrackCover(PlaylistTrackViewModel track)
        {
            try
            {

                if (!string.IsNullOrEmpty(track.AlbumCoverPath))
                {
                    if (!Uri.TryCreate(track.AlbumCoverPath, UriKind.Absolute, out var uri) || 
                        (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
                    {
                        var localPaths = new[]
                        {
                            Path.Combine("Resources", "covers", Path.GetFileName(track.AlbumCoverPath)),
                            Path.Combine("data", "covers", Path.GetFileName(track.AlbumCoverPath)),
                            track.AlbumCoverPath
                        };

                        foreach (var localPath in localPaths)
                        {
                            if (File.Exists(localPath))
                            {
                                return LoadImageFromFile(localPath);
                            }
                        }
                    }
                }

                if (track.AlbumCoverBinary != null && track.AlbumCoverBinary.Length > 0)
                {
                    return LoadImageFromBytes(track.AlbumCoverBinary);
                }

                if (track.Album != null)
                {
                    if (!string.IsNullOrEmpty(track.Album.CoverPath))
                    {
                        var albumPaths = new[]
                        {
                            Path.Combine("Resources", "covers", Path.GetFileName(track.Album.CoverPath)),
                            Path.Combine("data", "covers", Path.GetFileName(track.Album.CoverPath)),
                            track.Album.CoverPath
                        };

                        foreach (var path in albumPaths)
                        {
                            if (File.Exists(path))
                            {
                                return LoadImageFromFile(path);
                            }
                        }
                    }

                    if (track.Album.CoverBinary != null && track.Album.CoverBinary.Length > 0)
                    {
                        return LoadImageFromBytes(track.Album.CoverBinary);
                    }
                }

                var placeholderPaths = new[]
                {
                    "Resources/placeholder_cover.png",
                    "data/placeholder_cover.png"
                };

                foreach (var placeholderPath in placeholderPaths)
                {
                    if (File.Exists(placeholderPath))
                    {
                        return LoadImageFromFile(placeholderPath);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка загрузки обложки трека: {ex.Message}");
            }

            return null;
        }

        private BitmapImage LoadImageFromFile(string path)
        {
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(Path.GetFullPath(path), UriKind.Absolute);
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();
            bitmap.Freeze();
            return bitmap;
        }

        private BitmapImage LoadImageFromBytes(byte[] bytes)
        {
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.StreamSource = new MemoryStream(bytes);
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();
            bitmap.Freeze();
            return bitmap;
        }

        private void EditPlaylist_Click(object sender, RoutedEventArgs e)
        {
            if (_playlistViewModel == null) return;

            var playlistToEdit = _context.Playlists.FirstOrDefault(p => p.PlaylistID == _playlistViewModel.PlaylistID);
            if (playlistToEdit != null)
            {
                var editPlaylistView = new AddEditPlaylistView(_currentUser, playlistToEdit);
                if (editPlaylistView.ShowDialog() == true)
                {

                    LoadPlaylistData(_playlistViewModel.PlaylistID);

                    OnPropertyChanged(nameof(PlaylistName));
                }
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AddTrack_Click(object sender, RoutedEventArgs e)
        {
            if (_playlistViewModel == null) return;

            var addTracksView = new AddTracksToPlaylistView(_playlistViewModel.PlaylistID);
            if (addTracksView.ShowDialog() == true)
            {

                LoadPlaylistData(_playlistViewModel.PlaylistID);
            }
        }

        private void DeleteSelectedTrack_Click(object sender, RoutedEventArgs e)
        {
            if (_playlistViewModel == null || SelectedTrack == null) return;

            var result = MessageBox.Show(
                $"Вы уверены, что хотите удалить трек \"{SelectedTrack.TrackName}\" из плейлиста?",
                "Подтверждение удаления",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    var playlistTrack = _context.PlaylistTracks
                        .FirstOrDefault(pt => pt.PlaylistID == _playlistViewModel.PlaylistID && 
                                             pt.TrackID == SelectedTrack.TrackID);

                    if (playlistTrack != null)
                    {
                        _context.PlaylistTracks.Remove(playlistTrack);
                        _context.SaveChanges();

                        Tracks.Remove(SelectedTrack);
                        SelectedTrack = null;

                        OnPropertyChanged(nameof(TrackCount));
                        OnPropertyChanged(nameof(TotalDuration));

                        MessageBox.Show("Трек успешно удален из плейлиста", "Успех",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка удаления трека: {ex.Message}", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            _context?.Dispose();
            base.OnClosing(e);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class PlaylistTrackViewModel : INotifyPropertyChanged
    {
        private int _trackID;
        private string _trackName = "";
        private int _duration;
        private decimal _rating;
        private string _artistsText = "";
        private string _albumTitle = "";
        private string? _albumCoverPath;
        private byte[]? _albumCoverBinary;
        private Album? _album;

        public int TrackID
        {
            get => _trackID;
            set { _trackID = value; OnPropertyChanged(); }
        }

        public string TrackName
        {
            get => _trackName;
            set { _trackName = value; OnPropertyChanged(); }
        }

        public int Duration
        {
            get => _duration;
            set { _duration = value; OnPropertyChanged(); }
        }

        public decimal Rating
        {
            get => _rating;
            set { _rating = value; OnPropertyChanged(); }
        }

        public string ArtistsText
        {
            get => _artistsText;
            set { _artistsText = value; OnPropertyChanged(); }
        }

        public string AlbumTitle
        {
            get => _albumTitle;
            set { _albumTitle = value; OnPropertyChanged(); }
        }

        public string? AlbumCoverPath
        {
            get => _albumCoverPath;
            set { _albumCoverPath = value; OnPropertyChanged(); }
        }

        public byte[]? AlbumCoverBinary
        {
            get => _albumCoverBinary;
            set { _albumCoverBinary = value; OnPropertyChanged(); }
        }

        public Album? Album
        {
            get => _album;
            set { _album = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
