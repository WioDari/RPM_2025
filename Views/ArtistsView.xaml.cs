using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using MusicPlus.Models;
using MusicPlus.ViewModels;
using MusicPlus.Data;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace MusicPlus.Views
{
    public partial class ArtistsView : UserControl
    {
        private User _currentUser;
        private ObservableCollection<Artist> _artists;

        public ArtistsView(User user)
        {
            InitializeComponent();
            _currentUser = user;
            _artists = new ObservableCollection<Artist>();
            DataContext = this;
            
            LoadArtists();
        }

        public bool CanAddEdit => _currentUser.Role == "Admin";

        public ObservableCollection<Artist> Artists => _artists;

        private void LoadArtists()
        {
            try
            {
                _artists.Clear();

                var connectionString = "Server=localhost;Database=musicplus_db;User=root;Password=12345;Port=3306;CharSet=utf8mb4;";
                var serverVersion = ServerVersion.Create(new Version(8, 0, 21), ServerType.MySql);
                var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseMySql(connectionString, serverVersion)
                    .Options;

                using (var context = new ApplicationDbContext(options))
                {
                    var artists = context.Artists
                        .OrderBy(a => a.ArtistName)
                        .ToList();

                    foreach (var artist in artists)
                    {
                        _artists.Add(artist);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Ошибка загрузки артистов: {ex.Message}", "Ошибка", 
                    System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        private void AddArtist_Click(object sender, RoutedEventArgs e)
        {
            var addArtistView = new AddEditArtistView();
            if (addArtistView.ShowDialog() == true)
            {
                LoadArtists();
            }
        }

        private void ArtistItem_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is Border border && border.DataContext is Artist artist)
            {
                if (_currentUser.Role == "Admin")
                {
                    var editArtistView = new AddEditArtistView(artist);
                    if (editArtistView.ShowDialog() == true)
                    {
                        LoadArtists();
                    }
                }
            }
        }

        private void EditArtist_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Artist artist)
            {
                if (_currentUser.Role != "Admin")
                {
                    System.Windows.MessageBox.Show("Только администратор может редактировать артистов.", 
                        "Доступ запрещен", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                    return;
                }

                var editArtistView = new AddEditArtistView(artist);
                if (editArtistView.ShowDialog() == true)
                {
                    LoadArtists();
                }
            }
        }
    }
}
