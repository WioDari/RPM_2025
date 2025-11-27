using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using spotify.Context;
using spotify.Models;

namespace spotify.Windows
{
    /// <summary>
    /// Логика взаимодействия для NewAlbum.xaml
    /// </summary>
    public partial class NewAlbum : Window
    {
        User user1 = new();
        public NewAlbum()
        {
            InitializeComponent();
        }

        public NewAlbum(User user)
        {
            SpotifyContext context = new();
            InitializeComponent();
            user1 = user;

            ArtistComboBox.ItemsSource = context.Artists.ToList();
            GenreComboBox.ItemsSource = context.Genres.ToList();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            MainMenuWindow mainMenuWindow = new MainMenuWindow(user1);
            mainMenuWindow.Show();
            this.Close();
        }

        private void ComboBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var box = sender as ComboBox;
            box.IsDropDownOpen = true;
        }

        private void ComboBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var box = sender as ComboBox;
            box.IsDropDownOpen = false;
        }

        private async Task NewAlbumButton_Click(object sender, RoutedEventArgs e)
        {
            await using SpotifyContext context = new SpotifyContext();

            var album = new Album()
            {
                Id = context.Albums.Any() ? (context.Albums.Max(x => x.Id) + 1) : 1,
                AlbumTitle = AlbumTextBox.Text,
                ArtistId = context.Artists.First(x => x.ArtistName == ArtistComboBox.Text).Id,
                ReleaseYear = int.Parse(YearTextBox.Text),
                CoverPath = CoverPathTextBox.Text,
                TotalDuration = TimeOnly.FromTimeSpan(TimeSpan.Zero)
            };
        }

        private void AddGenreButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
