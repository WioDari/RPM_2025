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
    public class AddEditAlbumViewModel : BaseViewModel
    {
        private readonly ApplicationDbContext _context;
        private Album? _album;
        private bool _isEditMode;

        public AddEditAlbumViewModel(Album? album = null)
        {
            _album = album;
            _isEditMode = album != null;

            var connectionString = "Server=localhost;Database=musicplus_db;User=root;Password=12345;Port=3306;CharSet=utf8mb4;";
            var serverVersion = ServerVersion.Create(new Version(8, 0, 21), ServerType.MySql);
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseMySql(connectionString, serverVersion)
                .Options;
            
            _context = new ApplicationDbContext(options);

            Artists = new ObservableCollection<Artist>();
            AllGenres = new ObservableCollection<Genre>();
            SelectedGenres = new ObservableCollection<Genre>();

            SaveCommand = new RelayCommand(_ => Save(), _ => CanSave());
            CancelCommand = new RelayCommand(_ => Cancel());
            LoadCoverFromFileCommand = new RelayCommand(_ => LoadCoverFromFile());
            ApplyUrlCommand = new RelayCommand(_ => ApplyCoverUrl());

            LoadData();
        }

        public string Title => _isEditMode ? "Редактирование альбома" : "Добавление альбома";
        public string AlbumTitle { get; set; } = "";
        public int? ReleaseYear { get; set; }
        public Artist? SelectedArtist { get; set; }
        public ObservableCollection<Artist> Artists { get; }
        public ObservableCollection<Genre> AllGenres { get; }
        public ObservableCollection<Genre> SelectedGenres { get; }
        
        public System.Collections.Generic.List<int> SelectedGenreIds => SelectedGenres.Select(g => g.GenreID).ToList();
        public string? CoverPath { get; set; }
        public byte[]? CoverBinary { get; set; }
        public BitmapImage? CoverImage { get; set; }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand LoadCoverFromFileCommand { get; }
        public ICommand ApplyUrlCommand { get; }

        public event EventHandler<bool>? CloseRequested;

        private void LoadData()
        {
            try
            {
                Artists.Clear();
                var artists = _context.Artists.OrderBy(a => a.ArtistName).ToList();
                foreach (var artist in artists)
                {
                    Artists.Add(artist);
                }

                AllGenres.Clear();
                var genres = _context.Genres.OrderBy(g => g.GenreName).ToList();
                foreach (var genre in genres)
                {
                    AllGenres.Add(genre);
                }

                if (_isEditMode && _album != null)
                {
                    AlbumTitle = _album.AlbumTitle;
                    ReleaseYear = _album.ReleaseYear;
                    
                    if (_album.ArtistID.HasValue)
                    {
                        SelectedArtist = Artists.FirstOrDefault(a => a.ArtistID == _album.ArtistID.Value);
                    }

                    var albumGenres = _context.AlbumGenres
                        .Include(ag => ag.Genre)
                        .Where(ag => ag.AlbumID == _album.AlbumID)
                        .Select(ag => ag.Genre)
                        .Where(g => g != null)
                        .ToList();

                    SelectedGenres.Clear();
                    foreach (var genre in albumGenres)
                    {
                        if (genre != null && AllGenres.Contains(genre))
                            SelectedGenres.Add(genre);
                    }
                    OnPropertyChanged(nameof(SelectedGenres));

                    if (!string.IsNullOrEmpty(_album.CoverPath))
                    {
                        CoverPath = _album.CoverPath;
                        LoadCoverImage();
                    }
                    else if (_album.CoverBinary != null && _album.CoverBinary.Length > 0)
                    {
                        CoverBinary = _album.CoverBinary;
                        LoadCoverFromBinary();
                    }
                }

                OnPropertyChanged(nameof(AlbumTitle));
                OnPropertyChanged(nameof(ReleaseYear));
                OnPropertyChanged(nameof(SelectedArtist));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool CanSave()
        {
            return !string.IsNullOrWhiteSpace(AlbumTitle) &&
                   SelectedArtist != null &&
                   ReleaseYear.HasValue &&
                   ReleaseYear.Value > 1900 &&
                   ReleaseYear.Value <= DateTime.Now.Year + 1;
        }

        private void Save()
        {
            if (string.IsNullOrWhiteSpace(AlbumTitle))
            {
                MessageBox.Show("Необходимо указать название альбома", "Ошибка валидации", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (SelectedArtist == null)
            {
                MessageBox.Show("Необходимо выбрать исполнителя", "Ошибка валидации", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!ReleaseYear.HasValue || ReleaseYear.Value < 1900 || ReleaseYear.Value > DateTime.Now.Year + 1)
            {
                MessageBox.Show("Год выпуска должен быть между 1900 и текущим годом + 1", "Ошибка валидации", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                if (_isEditMode && _album != null)
                {
                    var albumToUpdate = _context.Albums.FirstOrDefault(a => a.AlbumID == _album.AlbumID);
                    if (albumToUpdate == null)
                    {
                        MessageBox.Show("Альбом не найден в базе данных", "Ошибка", 
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    albumToUpdate.AlbumTitle = AlbumTitle;
                    albumToUpdate.ReleaseYear = ReleaseYear;
                    albumToUpdate.ArtistID = SelectedArtist?.ArtistID;
                    albumToUpdate.CoverPath = CoverPath;
                    albumToUpdate.CoverBinary = null;

                    var existingGenres = _context.AlbumGenres
                        .Where(ag => ag.AlbumID == albumToUpdate.AlbumID)
                        .ToList();
                    _context.AlbumGenres.RemoveRange(existingGenres);

                    foreach (var genre in SelectedGenres)
                    {
                        _context.AlbumGenres.Add(new AlbumGenre
                        {
                            AlbumID = albumToUpdate.AlbumID,
                            GenreID = genre.GenreID
                        });
                    }

                    _context.SaveChanges();
                    MessageBox.Show("Альбом успешно обновлен", "Успех", 
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {

                    var newAlbum = new Album
                    {
                        AlbumTitle = AlbumTitle,
                        ReleaseYear = ReleaseYear,
                        ArtistID = SelectedArtist?.ArtistID,
                        CoverPath = CoverPath,
                        CoverBinary = null,
                        TotalDuration = 0
                    };

                    _context.Albums.Add(newAlbum);
                    _context.SaveChanges();

                    foreach (var genre in SelectedGenres)
                    {
                        _context.AlbumGenres.Add(new AlbumGenre
                        {
                            AlbumID = newAlbum.AlbumID,
                            GenreID = genre.GenreID
                        });
                    }

                    _context.SaveChanges();
                    MessageBox.Show("Альбом успешно добавлен", "Успех", 
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }

                CloseRequested?.Invoke(this, true);
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                if (ex.InnerException != null)
                {
                    errorMessage += $"\n\nДетали: {ex.InnerException.Message}";
                }
                MessageBox.Show($"Ошибка сохранения альбома: {errorMessage}", "Ошибка", 
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
                Title = "Выберите обложку альбома"
            };

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    var fileInfo = new FileInfo(dialog.FileName);
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

                    if (fileInfo.Length > 10 * 1024 * 1024)
                    {
                        MessageBox.Show("Размер файла не должен превышать 10MB", 
                            "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    string coversDirectory = Path.Combine("Resources", "covers");
                    if (!Directory.Exists(coversDirectory))
                    {
                        Directory.CreateDirectory(coversDirectory);
                    }

                    string originalFileName = Path.GetFileName(dialog.FileName);
                    string fileName = !string.IsNullOrEmpty(AlbumTitle) 
                        ? $"{AlbumTitle}{Path.GetExtension(originalFileName)}" 
                        : $"{Path.GetFileNameWithoutExtension(originalFileName)}_{DateTime.Now.Ticks}{Path.GetExtension(originalFileName)}";

                    foreach (char c in Path.GetInvalidFileNameChars())
                    {
                        fileName = fileName.Replace(c, '_');
                    }

                    string destinationPath = Path.Combine(coversDirectory, fileName);

                    int counter = 1;
                    while (File.Exists(destinationPath))
                    {
                        string nameWithoutExt = Path.GetFileNameWithoutExtension(fileName);
                        string ext = Path.GetExtension(fileName);
                        fileName = $"{nameWithoutExt}_{counter}{ext}";
                        destinationPath = Path.Combine(coversDirectory, fileName);
                        counter++;
                    }

                    File.Copy(dialog.FileName, destinationPath, true);

                    CoverPath = fileName;
                    CoverBinary = null;
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

        public string? CoverUrlInput { get; set; }

        public void ApplyCoverUrl()
        {
            if (!string.IsNullOrWhiteSpace(CoverUrlInput))
            {
                CoverPath = CoverUrlInput.Trim();
                CoverBinary = null;
                LoadCoverImage();
            }
        }

        private void LoadCoverImage()
        {
            if (string.IsNullOrEmpty(CoverPath)) return;

            try
            {
                if (Uri.TryCreate(CoverPath, UriKind.Absolute, out var uri) && 
                    (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps))
                {

                    CoverImage = null;
                }
                else
                {

                    if (File.Exists(CoverPath))
                    {
                        var image = new BitmapImage();
                        image.BeginInit();
                        image.UriSource = new Uri(CoverPath);
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
            if (CoverBinary == null || CoverBinary.Length == 0) return;

            try
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.StreamSource = new MemoryStream(CoverBinary);
                image.EndInit();
                CoverImage = image;
                OnPropertyChanged(nameof(CoverImage));
            }
            catch { }
        }
    }
}
