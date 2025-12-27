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
        Album album1;
        ObservableCollection<Genre> genres = [];
        ObservableCollection<Track> tracks = [];
        public SpotifyContext context = new();
        public NewAlbum()
        {
            InitializeComponent();
        }

        public NewAlbum(User user)
        {
            InitializeComponent();
            user1 = user;

            ArtistComboBox.ItemsSource = context.Artists.ToList();
            GenreComboBox.ItemsSource = context.Genres.ToList();
            TrackComboBox.ItemsSource = context.Tracks.Where(x => x.AlbumId == null).ToList();
        }

        public NewAlbum(Album album)
        {
            InitializeComponent();
            ArtistComboBox.ItemsSource = context.Artists.ToList();
            GenreComboBox.ItemsSource = context.Genres.ToList();
            TrackComboBox.ItemsSource = context.Tracks.Where(x => x.AlbumId == null).ToList();
            Title = "Изменение альбома";
            TitleTextBlock.Text = "Изменение альбома";

            album1 = album;
            AlbumTextBox.Text = album1.AlbumTitle;
            YearTextBox.Text = album1.ReleaseYear.ToString();
            GenreListBox.ItemsSource = album1.Genres;
            CoverPathTextBox.Text = album1.CoverPath;
            ArtistComboBox.Text = album1.Artist.ArtistName;
            TrackListBox.ItemsSource = album1.Tracks;
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
            var color = new SolidColorBrush(Color.FromRgb(171, 173, 179));
            var duration = new TimeSpan { };

            if (album1.Id != null)
            {
                AlbumTextBox.BorderBrush = color;
                YearTextBox.BorderBrush = color;
                CoverPathTextBox.BorderBrush = color;
                ArtistComboBox.BorderBrush = color;
                GenreListBox.BorderBrush = color;
                TrackListBox.BorderBrush = color;

                foreach (var track in album1.Tracks)
                {
                    if (track != null) tracks.Add(track);
                }
                TrackListBox.ItemsSource = tracks;

                foreach (var genre in album1.Genres)
                {
                    if (genre != null) genres.Add(genre);
                }
                GenreListBox.ItemsSource = genres;

                if (string.IsNullOrEmpty(AlbumTextBox.Text) || int.TryParse(YearTextBox.Text, out _) == false || string.IsNullOrEmpty(CoverPathTextBox.Text) || string.IsNullOrEmpty(ArtistComboBox.Text) || genres.Count == 0 || tracks.Count == 0)
                {
                    if (string.IsNullOrEmpty(AlbumTextBox.Text))
                    {
                        AlbumTextBox.BorderBrush = Brushes.Red;
                    }

                    if (int.TryParse(YearTextBox.Text, out _) == false)
                    {
                        YearTextBox.BorderBrush = Brushes.Red;
                    }

                    if (string.IsNullOrEmpty(CoverPathTextBox.Text))
                    {
                        CoverPathTextBox.BorderBrush = Brushes.Red;
                    }

                    if (string.IsNullOrEmpty(ArtistComboBox.Text))
                    {
                        ArtistComboBox.BorderBrush = Brushes.Red;
                    }

                    if (genres.Count == 0)
                    {
                        GenreListBox.BorderBrush = Brushes.Red;
                    }

                    if (tracks.Count == 0)
                    {
                        TrackListBox.BorderBrush = Brushes.Red;
                    }

                    MessageBox.Show("Какие то из полей не заполенены");
                    return;
                }

                foreach (var track in tracks)
                {
                    if(track != null)
                    {
                        duration += track.Duration;
                    }
                }

                album1.AlbumTitle = AlbumTextBox.Text;
                album1.ArtistId = context.Artists.First(x => x.ArtistName == ArtistComboBox.Text).Id;
                album1.ReleaseYear = int.Parse(YearTextBox.Text);
                album1.CoverPath = CoverPathTextBox.Text;
                album1.TotalDuration = duration;
                album1.Genres = genres;
                album1.Tracks = tracks;

                context.Albums.Update(album1);
                await context.SaveChangesAsync();

                MessageBox.Show("Альбом успешно изменен");
                return;
            }

            AlbumTextBox.BorderBrush = color;
            YearTextBox.BorderBrush = color;
            CoverPathTextBox.BorderBrush = color;
            ArtistComboBox.BorderBrush = color;
            GenreListBox.BorderBrush = color;
            TrackListBox.BorderBrush = color;


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
                TotalDuration = duration
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
