using MusicPlus.ViewModels;
using MusicPlus.Models;
using MusicPlus.Data;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Windows;
using System.Windows.Media.Imaging;
using System.IO;

namespace MusicPlus.ViewModels
{
    public class AddEditArtistViewModel : BaseViewModel
    {
        private readonly ApplicationDbContext _context;
        private Artist? _artist;
        private bool _isEditMode;

        public AddEditArtistViewModel(Artist? artist = null)
        {
            _artist = artist;
            _isEditMode = artist != null;

            var connectionString = "Server=localhost;Database=musicplus_db;User=root;Password=12345;Port=3306;CharSet=utf8mb4;";
            var serverVersion = ServerVersion.Create(new Version(8, 0, 21), ServerType.MySql);
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseMySql(connectionString, serverVersion)
                .Options;
            
            _context = new ApplicationDbContext(options);

            SaveCommand = new RelayCommand(_ => Save(), _ => CanSave());
            CancelCommand = new RelayCommand(_ => Cancel());
            LoadPhotoFromFileCommand = new RelayCommand(_ => LoadPhotoFromFile());
            ApplyUrlCommand = new RelayCommand(_ => ApplyUrl());

            LoadData();
        }

        public string Title => _isEditMode ? "Редактирование артиста" : "Добавление артиста";
        public string ArtistName { get; set; } = "";
        public string? Country { get; set; }
        public string? YearsActive { get; set; }
        public string? Description { get; set; }
        public string? PhotoPath { get; set; }
        public BitmapImage? PhotoImage { get; set; }
        public string? PhotoPathInput { get; set; }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand LoadPhotoFromFileCommand { get; }
        public ICommand ApplyUrlCommand { get; }

        public event EventHandler<bool>? CloseRequested;

        private void LoadData()
        {
            try
            {
                if (_isEditMode && _artist != null)
                {
                    ArtistName = _artist.ArtistName;
                    Country = _artist.Country;
                    YearsActive = _artist.YearsActive;
                    Description = _artist.Description;
                    PhotoPath = _artist.PhotoPath;

                    if (!string.IsNullOrEmpty(PhotoPath))
                    {
                        PhotoPathInput = PhotoPath;
                        LoadPhotoImage();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool CanSave()
        {
            return !string.IsNullOrWhiteSpace(ArtistName);
        }

        private void Save()
        {
            if (string.IsNullOrWhiteSpace(ArtistName))
            {
                MessageBox.Show("Необходимо указать название артиста", "Ошибка валидации", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                if (_isEditMode && _artist != null)
                {

                    var artistToEdit = _context.Artists
                        .FirstOrDefault(a => a.ArtistID == _artist.ArtistID);
                    
                    if (artistToEdit == null)
                    {
                        MessageBox.Show("Артист не найден в базе данных", "Ошибка", 
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    artistToEdit.ArtistName = ArtistName;
                    artistToEdit.Country = Country;
                    artistToEdit.YearsActive = YearsActive;
                    artistToEdit.Description = Description;
                    artistToEdit.PhotoPath = PhotoPath;

                    _context.SaveChanges();
                    MessageBox.Show("Артист успешно обновлен", "Успех", 
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {

                    var newArtist = new Artist
                    {
                        ArtistName = ArtistName,
                        Country = Country ?? "Unknown",
                        YearsActive = YearsActive,
                        Description = Description,
                        PhotoPath = PhotoPath
                    };

                    _context.Artists.Add(newArtist);
                    _context.SaveChanges();
                    MessageBox.Show("Артист успешно добавлен", "Успех", 
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }

                CloseRequested?.Invoke(this, true);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения артиста: {ex.Message}", "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel()
        {
            CloseRequested?.Invoke(this, false);
        }

        private void LoadPhotoFromFile()
        {
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Изображения (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg|Все файлы (*.*)|*.*",
                Title = "Выберите фото артиста"
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

                    PhotoImage = image;
                    PhotoPath = dialog.FileName;
                    PhotoPathInput = dialog.FileName;

                    OnPropertyChanged(nameof(PhotoImage));
                    OnPropertyChanged(nameof(PhotoPathInput));
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка загрузки фото: {ex.Message}", "Ошибка", 
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ApplyUrl()
        {
            if (string.IsNullOrWhiteSpace(PhotoPathInput))
            {
                MessageBox.Show("Введите URL изображения", "Предупреждение", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                PhotoPath = PhotoPathInput;
                LoadPhotoImage();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка применения URL: {ex.Message}", "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadPhotoImage()
        {
            try
            {
                if (!string.IsNullOrEmpty(PhotoPath))
                {
                    var image = new BitmapImage();
                    image.BeginInit();
                    image.UriSource = new Uri(PhotoPath);
                    image.EndInit();
                    PhotoImage = image;
                }
                else
                {
                    PhotoImage = null;
                }

                OnPropertyChanged(nameof(PhotoImage));
            }
            catch
            {
                PhotoImage = null;
                OnPropertyChanged(nameof(PhotoImage));
            }
        }

    }
}
