using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Microsoft.EntityFrameworkCore;
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
        ObservableCollection<Genre> genres = [];
        ObservableCollection<Track> tracks = [];
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
            TrackComboBox.ItemsSource = context.Tracks.Where(x => x.AlbumId == null).ToList();
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

        private async void NewAlbumButton_Click(object sender, RoutedEventArgs e)
        {
            await using SpotifyContext context = new SpotifyContext();

            if ( string.IsNullOrEmpty(AlbumTextBox.Text) || int.TryParse(YearTextBox.Text, out _) == false || string.IsNullOrEmpty(CoverPathTextBox.Text) || string.IsNullOrEmpty(ArtistComboBox.Text) || genres.Count == 0 || tracks.Count == 0)
            {
                if(string.IsNullOrEmpty(AlbumTextBox.Text))
                {
                    AlbumTextBox.BorderBrush = Brushes.Red;
                }

                if(int.TryParse(YearTextBox.Text, out _) == false)
                {
                    YearTextBox.BorderBrush = Brushes.Red;
                }

                if(string.IsNullOrEmpty(CoverPathTextBox.Text))
                {
                    CoverPathTextBox.BorderBrush = Brushes.Red;
                }
                    
                if(string.IsNullOrEmpty(ArtistComboBox.Text))
                {
                    ArtistComboBox.BorderBrush = Brushes.Red;
                }

                if(genres.Count == 0)
                {
                    GenreListBox.BorderBrush = Brushes.Red;
                }

                if(tracks.Count == 0)
                {
                    TrackListBox.BorderBrush = Brushes.Red;
                }

                MessageBox.Show("Какие то из полей не заполенены");
                return;
            }

            var duration = new TimeSpan();

            foreach (var track in tracks)
            {
                duration += track.Duration;
            }

            var album = new Album()
            {
                Id = context.Albums.Any() ? (context.Albums.Max(x => x.Id) + 1) : 1,
                AlbumTitle = AlbumTextBox.Text,
                ArtistId = context.Artists.First(x => x.ArtistName == ArtistComboBox.Text).Id,
                ReleaseYear = int.Parse(YearTextBox.Text),
                CoverPath = CoverPathTextBox.Text,
                TotalDuration = duration,
                Genres = new List<Genre>()
            };
            context.Albums.Add(album);
            context.SaveChanges();

            album.Genres = genres;
            context.Albums.Update(album);
            album.Tracks = tracks;
            context.Albums.Update(album);
            await context.SaveChangesAsync();

            MessageBox.Show("Альбом успешно добавлен");
            return;
        }

        private void AddGenreButton_Click(object sender, RoutedEventArgs e)
        {
            if (GenreComboBox.SelectedItem != null)
            {
                if (genres.Contains(GenreComboBox.SelectedItem as Genre))
                {
                    MessageBox.Show("Жанр уже добавлен");
                    return;
                }
                genres.Add(GenreComboBox.SelectedItem as Genre);
                GenreListBox.ItemsSource = genres;
                GenreComboBox.SelectedItem = null;
            }
            else
            {
                MessageBox.Show("Жанр не выбран");
                return;
            }
        }

        private void DeleteGenre_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            genres.Remove(button.DataContext as Genre);
        }

        private void CoverPathTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                AlbumCover.Source = new BitmapImage(new Uri(CoverPathTextBox.Text, UriKind.RelativeOrAbsolute));
            }
            catch
            {
                AlbumCover.Source = new BitmapImage(new Uri(@"/Resources/placeholder_cover.png", UriKind.RelativeOrAbsolute));
            }
        }

        private void AlbumCover_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            AlbumCover.Source = new BitmapImage(new Uri(@"/Resources/placeholder_cover.png", UriKind.RelativeOrAbsolute));

        }

        private void AddTrackButton_Click(object sender, RoutedEventArgs e)
        {
            if (TrackComboBox.SelectedItem != null)
            {
                if(tracks.Contains(TrackComboBox.SelectedItem as Track))
                {
                    MessageBox.Show("Трек уже добавлен");
                    return;
                }
                tracks.Add(TrackComboBox.SelectedItem as Track);
                TrackListBox.ItemsSource = tracks;
                TrackComboBox.SelectedItem = null;
            }
            else
            {
                MessageBox.Show("Трек не выбран");
                return;
            }
        }

        private void DeleteTrack_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            tracks.Remove(button.DataContext as Track);
        }
    }
}
