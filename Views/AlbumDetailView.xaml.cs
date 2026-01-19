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
    public partial class AlbumDetailView : Window
    {
        private Album _album;
        private User _currentUser;
        private ApplicationDbContext _context;

        public AlbumDetailView(Album album, User currentUser)
        {
            InitializeComponent();
            _album = album;
            _currentUser = currentUser;
            DataContext = this;
            
            var connectionString = "Server=localhost;Database=musicplus_db;User=root;Password=12345;Port=3306;CharSet=utf8mb4;";
            var serverVersion = ServerVersion.Create(new Version(8, 0, 21), ServerType.MySql);
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseMySql(connectionString, serverVersion)
                .Options;
            
            _context = new ApplicationDbContext(options);
            
            CanEdit = _currentUser.Role == "Admin";
            CanDelete = _currentUser.Role == "Admin";
            
            LoadAlbumData();
        }

        public Album Album => _album;
        
        private bool _canEdit;
        public bool CanEdit
        {
            get => _canEdit;
            private set
            {
                if (_canEdit != value)
                {
                    _canEdit = value;
                    OnPropertyChanged();
                }
            }
        }
        
        private bool _canDelete;
        public bool CanDelete
        {
            get => _canDelete;
            private set
            {
                if (_canDelete != value)
                {
                    _canDelete = value;
                    OnPropertyChanged();
                }
            }
        }

        public string GenresText
        {
            get
            {
                if (_album.AlbumGenres == null || !_album.AlbumGenres.Any())
                    return "Не указано";
                
                return string.Join(", ", _album.AlbumGenres
                    .Where(ag => ag.Genre != null)
                    .Select(ag => ag.Genre!.GenreName)
                    .OrderBy(g => g));
            }
        }

        public ObservableCollection<AlbumTrackViewModel> Tracks { get; } = new ObservableCollection<AlbumTrackViewModel>();

        private void LoadAlbumData()
        {
            try
            {
                var albumId = _album.AlbumID;
                var reloadedAlbum = _context.Albums
                    .Include(a => a.Artist)
                    .Include(a => a.AlbumGenres)
                        .ThenInclude(ag => ag.Genre)
                    .Include(a => a.Tracks)
                        .ThenInclude(t => t.TrackArtists)
                            .ThenInclude(ta => ta.Artist)
                    .FirstOrDefault(a => a.AlbumID == albumId);

                if (reloadedAlbum == null)
                {
                    MessageBox.Show("Альбом не найден", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    Close();
                    return;
                }
                
                _album = reloadedAlbum;

                OnPropertyChanged(nameof(GenresText));

                Tracks.Clear();
                if (_album.Tracks != null)
                {
                    foreach (var track in _album.Tracks.OrderBy(t => t.TrackName))
                    {
                        var trackViewModel = new AlbumTrackViewModel
                        {
                            TrackID = track.TrackID,
                            TrackName = track.TrackName,
                            Duration = track.Duration,
                            Rating = track.Rating ?? 0,
                            ArtistsText = GetTrackArtists(track)
                        };
                        Tracks.Add(trackViewModel);
                    }
                }

                OnPropertyChanged(nameof(Album));
                OnPropertyChanged(nameof(GenresText));
                OnPropertyChanged(nameof(Tracks));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных альбома: {ex.Message}", "Ошибка", 
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

        private void AlbumCoverImage_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.Image image)
            {
                image.Source = LoadAlbumCover(_album);
            }
        }

        private BitmapImage? LoadAlbumCover(Album album)
        {
            try
            {
                if (!string.IsNullOrEmpty(album.CoverPath))
                {
                    if (!Uri.TryCreate(album.CoverPath, UriKind.Absolute, out var uri) || 
                        (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
                    {
                        var localPaths = new[]
                        {
                            Path.Combine("Resources", "covers", Path.GetFileName(album.CoverPath)),
                            Path.Combine("data", "covers", Path.GetFileName(album.CoverPath)),
                            album.CoverPath
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

                if (album.CoverBinary != null && album.CoverBinary.Length > 0)
                {
                    return LoadImageFromBytes(album.CoverBinary);
                }

                var placeholderPaths = new[]
                {
                    "Resources/placeholder_cover.png",
                    "data/placeholder_cover.png",
                    Path.Combine("Resources", "placeholder_cover.png"),
                    Path.Combine("data", "placeholder_cover.png")
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
                System.Diagnostics.Debug.WriteLine($"Ошибка загрузки обложки: {ex.Message}");
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

        private void EditAlbum_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                var fullAlbum = _context.Albums
                    .Include(a => a.Artist)
                    .Include(a => a.AlbumGenres)
                        .ThenInclude(ag => ag.Genre)
                    .FirstOrDefault(a => a.AlbumID == _album.AlbumID);

                if (fullAlbum != null)
                {
                    var editView = new AddEditAlbumView(fullAlbum);
                    if (editView.ShowDialog() == true)
                    {

                        LoadAlbumData();
                        OnPropertyChanged(nameof(GenresText));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка открытия формы редактирования: {ex.Message}", "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddTrack_Click(object sender, RoutedEventArgs e)
        {
            var addTrackView = new AddEditTrackView(albumId: _album.AlbumID);
            if (addTrackView.ShowDialog() == true)
            {

                LoadAlbumData();
            }
        }

        private void DeleteAlbum_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
                $"Вы уверены, что хотите удалить альбом \"{_album.AlbumTitle}\"?\nВсе треки из этого альбома также будут удалены из плейлистов.",
                "Подтверждение удаления",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {

                    _context.Albums.Remove(_album);
                    _context.SaveChanges();

                    MessageBox.Show("Альбом успешно удален", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);

                    DialogResult = true;
                    Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка удаления альбома: {ex.Message}", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void EditTrack_Click(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.Button button && button.DataContext is AlbumTrackViewModel trackVm)
            {
                if (_currentUser.Role != "Admin")
                {
                    MessageBox.Show("Только администратор может редактировать треки.", 
                        "Доступ запрещен", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                try
                {
                    var trackToEdit = _context.Tracks
                        .Include(t => t.TrackArtists)
                        .Include(t => t.Album)
                        .FirstOrDefault(t => t.TrackID == trackVm.TrackID);
                    
                    if (trackToEdit != null)
                    {
                        var editTrackView = new AddEditTrackView(trackToEdit);
                        if (editTrackView.ShowDialog() == true)
                        {

                            LoadAlbumData();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка открытия формы редактирования: {ex.Message}", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void DeleteTrack_Click(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.Button button && button.DataContext is AlbumTrackViewModel trackVm)
            {
                var result = MessageBox.Show(
                    $"Вы уверены, что хотите удалить трек \"{trackVm.TrackName}\"?\nТрек будет удален из всех плейлистов.",
                    "Подтверждение удаления",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        var track = _context.Tracks.Find(trackVm.TrackID);
                        if (track != null)
                        {
                            _context.Tracks.Remove(track);
                            _context.SaveChanges();

                            Tracks.Remove(trackVm);

                            MessageBox.Show("Трек успешно удален", "Успех",
                                MessageBoxButton.OK, MessageBoxImage.Information);

                            LoadAlbumData();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка удаления трека: {ex.Message}", "Ошибка",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }

    public class AlbumTrackViewModel : INotifyPropertyChanged
    {
        private int _trackID;
        private string _trackName = "";
        private int _duration;
        private decimal _rating;
        private string _artistsText = "";

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

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
