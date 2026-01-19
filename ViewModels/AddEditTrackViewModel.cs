using MusicPlus.ViewModels;
using MusicPlus.Models;
using MusicPlus.Data;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows;

namespace MusicPlus.ViewModels
{
    public class AddEditTrackViewModel : BaseViewModel
    {
        private readonly ApplicationDbContext _context;
        private Track? _track;
        private bool _isEditMode;

        public AddEditTrackViewModel(Track? track = null, int? albumId = null)
        {
            _track = track;
            _isEditMode = track != null;

            var connectionString = "Server=localhost;Database=musicplus_db;User=root;Password=12345;Port=3306;CharSet=utf8mb4;";
            var serverVersion = ServerVersion.Create(new Version(8, 0, 21), ServerType.MySql);
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseMySql(connectionString, serverVersion)
                .Options;
            
            _context = new ApplicationDbContext(options);

            Artists = new ObservableCollection<Artist>();
            Albums = new ObservableCollection<Album>();
            SelectedArtists = new ObservableCollection<Artist>();

            SaveCommand = new RelayCommand(_ => Save(), _ => CanSave());
            CancelCommand = new RelayCommand(_ => Cancel());
            LoadCoverFromFileCommand = new RelayCommand(_ => LoadCoverFromFile());
            ApplyUrlCommand = new RelayCommand(_ => ApplyCoverUrl());

            LoadData(albumId);
        }

        public string Title => _isEditMode ? "Редактирование трека" : "Добавление трека";
        public string TrackName { get; set; } = "";
        public int Duration { get; set; }
        
        private string _durationString = "";
        public string DurationString
        {
            get => _durationString;
            set
            {
                _durationString = value;

                Duration = ParseDurationString(value);
                OnPropertyChanged();
                OnPropertyChanged(nameof(Duration));
                CommandManager.InvalidateRequerySuggested();
            }
        }
        
        private int ParseDurationString(string duration)
        {
            if (string.IsNullOrWhiteSpace(duration))
                return 0;
                
            try
            {
                duration = duration.Trim();
                var parts = duration.Split(':');
                
                if (parts.Length == 2)
                {
                    if (int.TryParse(parts[0], out int first) && int.TryParse(parts[1], out int second))
                    {
                        if (first > 59)
                        {

                            return first * 3600 + second * 60;
                        }
                        else
                        {

                            return first * 60 + second;
                        }
                    }
                }
                else if (parts.Length == 3)
                {
                    if (int.TryParse(parts[0], out int hours) && 
                        int.TryParse(parts[1], out int minutes) && 
                        int.TryParse(parts[2], out int seconds))
                    {
                        if (minutes >= 0 && minutes < 60 && seconds >= 0 && seconds < 60)
                        {
                            return hours * 3600 + minutes * 60 + seconds;
                        }
                    }
                }
            }
            catch { }
            
            return 0;
        }
        
        private string FormatDuration(int seconds)
        {
            int hours = seconds / 3600;
            int minutes = (seconds % 3600) / 60;
            int secs = seconds % 60;

            if (hours > 0)
            {
                return $"{hours}:{minutes:D2}:{secs:D2}";
            }
            return $"{minutes}:{secs:D2}";
        }
        
        public DateTime? ReleaseDate { get; set; }
        public int? Bitrate { get; set; }
        public decimal? Rating { get; set; }
        
        public Album? SelectedAlbum { get; set; }
        public ObservableCollection<Album> Albums { get; }
        
        public ObservableCollection<Artist> Artists { get; }
        public ObservableCollection<Artist> SelectedArtists { get; }
        
        public string? AlbumCoverPath { get; set; }
        public byte[]? AlbumCoverBinary { get; set; }
        public BitmapImage? CoverImage { get; set; }
        public string? CoverUrlInput { get; set; }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand LoadCoverFromFileCommand { get; }
        public ICommand ApplyUrlCommand { get; }

        public event EventHandler<bool>? CloseRequested;
        public event EventHandler<Album>? AlbumSelectedForAutoFill;

        private void LoadData(int? albumId = null)
        {
            try
            {

                Artists.Clear();
                var artists = _context.Artists.OrderBy(a => a.ArtistName).ToList();
                foreach (var artist in artists)
                {
                    Artists.Add(artist);
                }

                Albums.Clear();
                var albums = _context.Albums
                    .Include(a => a.Artist)
                    .OrderBy(a => a.AlbumTitle)
                    .ToList();
                foreach (var album in albums)
                {
                    Albums.Add(album);
                }

                if (_isEditMode && _track != null)
                {
                    TrackName = _track.TrackName;
                    Duration = _track.Duration;
                    _durationString = FormatDuration(Duration);
                    ReleaseDate = _track.ReleaseDate;
                    Bitrate = _track.Bitrate;
                    Rating = _track.Rating ?? 0;

                    if (_track.AlbumID.HasValue)
                    {
                        SelectedAlbum = Albums.FirstOrDefault(a => a.AlbumID == _track.AlbumID.Value);
                        if (SelectedAlbum != null)
                        {
                            AutoFillFromAlbum(SelectedAlbum);
                        }
                    }

                    var trackArtists = _context.TrackArtists
                        .Include(ta => ta.Artist)
                        .Where(ta => ta.TrackID == _track.TrackID)
                        .Select(ta => ta.Artist)
                        .Where(a => a != null)
                        .ToList();

                    SelectedArtists.Clear();
                    foreach (var artist in trackArtists)
                    {
                        if (artist != null && Artists.Contains(artist))
                            SelectedArtists.Add(artist);
                    }

                    if (!string.IsNullOrEmpty(_track.AlbumCoverPath))
                    {
                        AlbumCoverPath = _track.AlbumCoverPath;
                        LoadCoverImage();
                    }
                    else if (_track.AlbumCoverBinary != null && _track.AlbumCoverBinary.Length > 0)
                    {
                        AlbumCoverBinary = _track.AlbumCoverBinary;
                        LoadCoverFromBinary();
                    }
                }
                else if (albumId.HasValue)
                {

                    SelectedAlbum = Albums.FirstOrDefault(a => a.AlbumID == albumId.Value);
                    if (SelectedAlbum != null)
                    {
                        AutoFillFromAlbum(SelectedAlbum);
                    }
                }

                OnPropertyChanged(nameof(TrackName));
                OnPropertyChanged(nameof(Duration));
                OnPropertyChanged(nameof(ReleaseDate));
                OnPropertyChanged(nameof(Bitrate));
                OnPropertyChanged(nameof(Rating));
                OnPropertyChanged(nameof(SelectedAlbum));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void OnAlbumSelected()
        {
            if (SelectedAlbum != null)
            {
                AutoFillFromAlbum(SelectedAlbum);
                AlbumSelectedForAutoFill?.Invoke(this, SelectedAlbum);
            }
        }

        private void AutoFillFromAlbum(Album album)
        {

            if (album.ReleaseYear.HasValue && !ReleaseDate.HasValue)
            {
                ReleaseDate = new DateTime(album.ReleaseYear.Value, 1, 1);
                OnPropertyChanged(nameof(ReleaseDate));
            }

            if (string.IsNullOrEmpty(AlbumCoverPath) && AlbumCoverBinary == null && album != null)
            {
                AlbumCoverPath = album.CoverPath;
                AlbumCoverBinary = album.CoverBinary;
                if (AlbumCoverBinary != null && AlbumCoverBinary.Length > 0)
                {
                    LoadCoverFromBinary();
                }
                else if (!string.IsNullOrEmpty(AlbumCoverPath))
                {
                    LoadCoverImage();
                }
                OnPropertyChanged(nameof(CoverImage));
            }
        }

        private bool CanSave()
        {
            return !string.IsNullOrWhiteSpace(TrackName) &&
                   Duration > 0 &&
                   SelectedArtists.Any();
        }

        private void Save()
        {

            if (string.IsNullOrWhiteSpace(TrackName))
            {
                MessageBox.Show("Необходимо указать название трека", "Ошибка валидации", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (Duration <= 0)
            {
                MessageBox.Show("Продолжительность должна быть больше 0", "Ошибка валидации", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!SelectedArtists.Any())
            {
                MessageBox.Show("Необходимо выбрать хотя бы одного исполнителя", "Ошибка валидации", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (Rating.HasValue && (Rating < 0 || Rating > 5))
            {
                MessageBox.Show("Рейтинг должен быть от 0.0 до 5.0", "Ошибка валидации", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                if (_isEditMode && _track != null)
                {

                    var trackToEdit = _context.Tracks
                        .Include(t => t.TrackArtists)
                        .FirstOrDefault(t => t.TrackID == _track.TrackID);
                    
                    if (trackToEdit == null)
                    {
                        MessageBox.Show("Трек не найден в базе данных", "Ошибка", 
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    trackToEdit.TrackName = TrackName;
                    trackToEdit.Duration = Duration;
                    trackToEdit.ReleaseDate = ReleaseDate;
                    trackToEdit.Bitrate = Bitrate;
                    trackToEdit.Rating = Rating;
                    trackToEdit.AlbumID = SelectedAlbum?.AlbumID;
                    trackToEdit.AlbumCoverPath = AlbumCoverPath;
                    trackToEdit.AlbumCoverBinary = AlbumCoverBinary;

                    var existingArtists = _context.TrackArtists
                        .Where(ta => ta.TrackID == trackToEdit.TrackID)
                        .ToList();
                    _context.TrackArtists.RemoveRange(existingArtists);

                    foreach (var artist in SelectedArtists)
                    {
                        _context.TrackArtists.Add(new TrackArtist
                        {
                            TrackID = trackToEdit.TrackID,
                            ArtistID = artist.ArtistID
                        });
                    }

                    _context.SaveChanges();
                    MessageBox.Show("Трек успешно обновлен", "Успех", 
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {

                    var newTrack = new Track
                    {
                        TrackName = TrackName,
                        Duration = Duration,
                        ReleaseDate = ReleaseDate,
                        Bitrate = Bitrate,
                        Rating = Rating,
                        AlbumID = SelectedAlbum?.AlbumID,
                        AlbumCoverPath = AlbumCoverPath,
                        AlbumCoverBinary = AlbumCoverBinary,
                        PlayCount = 0
                    };

                    _context.Tracks.Add(newTrack);
                    _context.SaveChanges();

                    foreach (var artist in SelectedArtists)
                    {
                        _context.TrackArtists.Add(new TrackArtist
                        {
                            TrackID = newTrack.TrackID,
                            ArtistID = artist.ArtistID
                        });
                    }

                    _context.SaveChanges();
                    MessageBox.Show("Трек успешно добавлен", "Успех", 
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }

                CloseRequested?.Invoke(this, true);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения трека: {ex.Message}", "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel()
        {
            CloseRequested?.Invoke(this, false);
        }

        private void LoadCoverFromFile()
        {
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "PNG файлы (*.png)|*.png|Все файлы (*.*)|*.*",
                Title = "Выберите обложку"
            };

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    var image = new BitmapImage();
                    image.BeginInit();
                    image.UriSource = new Uri(dialog.FileName);
                    image.EndInit();

                    if (image.PixelWidth > 1000 || image.PixelHeight > 1000)
                    {
                        MessageBox.Show("Размер изображения не должен превышать 1000x1000 пикселей", 
                            "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    if (Path.GetExtension(dialog.FileName).ToLower() != ".png")
                    {
                        MessageBox.Show("Допускаются только PNG файлы", 
                            "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    AlbumCoverBinary = File.ReadAllBytes(dialog.FileName);
                    AlbumCoverPath = null;
                    CoverImage = image;
                    OnPropertyChanged(nameof(CoverImage));
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка загрузки файла: {ex.Message}", "Ошибка", 
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public void ApplyCoverUrl()
        {
            if (!string.IsNullOrWhiteSpace(CoverUrlInput))
            {
                AlbumCoverPath = CoverUrlInput.Trim();
                AlbumCoverBinary = null;
                LoadCoverImage();
            }
        }

        private void LoadCoverImage()
        {
            if (string.IsNullOrEmpty(AlbumCoverPath)) return;

            try
            {
                if (Uri.TryCreate(AlbumCoverPath, UriKind.Absolute, out var uri) && 
                    (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps))
                {

                    CoverImage = null;
                }
                else
                {

                    if (File.Exists(AlbumCoverPath))
                    {
                        var image = new BitmapImage();
                        image.BeginInit();
                        image.UriSource = new Uri(AlbumCoverPath);
                        image.EndInit();
                        CoverImage = image;
                    }
                }
            }
            catch { }

            OnPropertyChanged(nameof(CoverImage));
        }

        private void LoadCoverFromBinary()
        {
            if (AlbumCoverBinary == null || AlbumCoverBinary.Length == 0) return;

            try
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.StreamSource = new MemoryStream(AlbumCoverBinary);
                image.EndInit();
                CoverImage = image;
                OnPropertyChanged(nameof(CoverImage));
            }
            catch { }
        }
    }
}
