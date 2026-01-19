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

namespace MusicPlus.ViewModels
{
    public class AddEditPlaylistViewModel : BaseViewModel
    {
        private readonly ApplicationDbContext _context;
        private Playlist? _playlist;
        private bool _isEditMode;
        private User _currentUser;

        public AddEditPlaylistViewModel(User currentUser, Playlist? playlist = null)
        {
            _playlist = playlist;
            _isEditMode = playlist != null;
            _currentUser = currentUser;

            var connectionString = "Server=localhost;Database=musicplus_db;User=root;Password=12345;Port=3306;CharSet=utf8mb4;";
            var serverVersion = ServerVersion.Create(new Version(8, 0, 21), ServerType.MySql);
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseMySql(connectionString, serverVersion)
                .Options;
            
            _context = new ApplicationDbContext(options);

            SaveCommand = new RelayCommand(_ => Save(), _ => CanSave());
            CancelCommand = new RelayCommand(_ => Cancel());

            LoadData();
        }

        public string Title => _isEditMode ? "Редактирование плейлиста" : "Добавление плейлиста";
        public string PlaylistName { get; set; } = "";
        public string? Description { get; set; }
        public ObservableCollection<string> AvailableTags { get; } = new ObservableCollection<string>();
        public ObservableCollection<string> SelectedTags { get; } = new ObservableCollection<string>();
        public string CustomTagsInput { get; set; } = "";

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public event EventHandler<bool>? CloseRequested;

        private void LoadData()
        {
            try
            {

                var existingTags = _context.PlaylistTags
                    .Select(pt => pt.TagName)
                    .Distinct()
                    .OrderBy(t => t)
                    .ToList();

                AvailableTags.Clear();
                foreach (var tag in existingTags)
                {
                    AvailableTags.Add(tag);
                }

                if (_isEditMode && _playlist != null)
                {
                    PlaylistName = _playlist.PlaylistName;
                    Description = _playlist.Description;

                    var playlistTags = _context.PlaylistTags
                        .Where(pt => pt.PlaylistID == _playlist.PlaylistID)
                        .Select(pt => pt.TagName)
                        .ToList();

                    SelectedTags.Clear();
                    foreach (var tag in playlistTags)
                    {
                        SelectedTags.Add(tag);
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
            return !string.IsNullOrWhiteSpace(PlaylistName);
        }

        private void Save()
        {

            if (string.IsNullOrWhiteSpace(PlaylistName))
            {
                MessageBox.Show("Необходимо указать название плейлиста", "Ошибка валидации", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                Playlist playlistToSave;
                
                if (_isEditMode && _playlist != null)
                {

                    playlistToSave = _context.Playlists.FirstOrDefault(p => p.PlaylistID == _playlist.PlaylistID);
                    if (playlistToSave == null)
                    {
                        MessageBox.Show("Плейлист не найден в базе данных", "Ошибка", 
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    playlistToSave.PlaylistName = PlaylistName;
                    playlistToSave.Description = Description;

                    var oldTags = _context.PlaylistTags
                        .Where(pt => pt.PlaylistID == playlistToSave.PlaylistID)
                        .ToList();
                    _context.PlaylistTags.RemoveRange(oldTags);
                }
                else
                {

                    playlistToSave = new Playlist
                    {
                        PlaylistName = PlaylistName,
                        Description = Description,
                        UserID = _currentUser.UserID,
                        DateCreated = DateTime.Now,
                        Likes = 0
                    };

                    _context.Playlists.Add(playlistToSave);
                }

                _context.SaveChanges();

                var allTags = new HashSet<string>(SelectedTags);

                if (!string.IsNullOrWhiteSpace(CustomTagsInput))
                {
                    var customTags = CustomTagsInput.Split(',')
                        .Select(t => t.Trim())
                        .Where(t => !string.IsNullOrWhiteSpace(t));
                    foreach (var tag in customTags)
                    {
                        allTags.Add(tag);
                    }
                }

                foreach (var tagName in allTags)
                {
                    _context.PlaylistTags.Add(new PlaylistTag
                    {
                        PlaylistID = playlistToSave.PlaylistID,
                        TagName = tagName
                    });
                }

                _context.SaveChanges();

                MessageBox.Show(_isEditMode ? "Плейлист успешно обновлен" : "Плейлист успешно добавлен", "Успех", 
                    MessageBoxButton.OK, MessageBoxImage.Information);

                CloseRequested?.Invoke(this, true);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения плейлиста: {ex.Message}", "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel()
        {
            CloseRequested?.Invoke(this, false);
        }
    }
}
