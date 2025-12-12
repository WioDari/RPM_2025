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
using spotify.Context;
using spotify.Models;

namespace spotify.Windows
{
    /// <summary>
    /// Логика взаимодействия для NewTrack.xaml
    /// </summary>
    public partial class NewTrack : Window
    {
        User user1 = new();
        ObservableCollection<Genre> genres = [];
        ObservableCollection<Artist> artists = [];
        public NewTrack()
        {
            InitializeComponent();
        }

        public NewTrack(User user)
        {
            SpotifyContext context = new();
            InitializeComponent();
            user1 = user;
            GenreComboBox.ItemsSource = context.Genres.ToList();
            ArtistComboBox.ItemsSource = context.Artists.ToList();
            ReleaseDatePicker.SelectedDate = DateTime.Now;
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

        private void AddArtistButton_Click(object sender, RoutedEventArgs e)
        {
            if (ArtistComboBox.SelectedItem != null)
            {
                if (artists.Contains(ArtistComboBox.SelectedItem as Artist))
                {
                    MessageBox.Show("Исполнитель уже добавлен");
                    return;
                }
                artists.Add(ArtistComboBox.SelectedItem as Artist);
                ArtistListBox.ItemsSource = artists;
                ArtistComboBox.SelectedItem = null;
            }
            else
            {
                MessageBox.Show("Исполнитель не выбран");
                return;
            }
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

        private async void NewTrackButton_Click(object sender, RoutedEventArgs e)
        {
            var color = new SolidColorBrush(Color.FromRgb(171, 173, 179));

            TrackTextBox.BorderBrush = color;
            FilePathTextBox.BorderBrush = color;
            GenreListBox.BorderBrush = color;
            ArtistListBox.BorderBrush = color;
            BitrateTextBox.BorderBrush = color;
            DurationTextBox.BorderBrush = color;
                
            await using SpotifyContext context = new SpotifyContext();
            if (string.IsNullOrEmpty(TrackTextBox.Text) || string.IsNullOrEmpty(FilePathTextBox.Text) || genres.Count == 0 || artists.Count == 0 || int.TryParse(BitrateTextBox.Text, out _) == false || string.IsNullOrEmpty(DurationTextBox.Text) || DurationTextBox.Text == "00:00:00")
            {
                if (string.IsNullOrEmpty(TrackTextBox.Text))
                {
                    TrackTextBox.BorderBrush = Brushes.Red;
                }

                if (string.IsNullOrEmpty(FilePathTextBox.Text))
                {
                    FilePathTextBox.BorderBrush= Brushes.Red;
                }

                if(genres.Count == 0)
                {
                    GenreListBox.BorderBrush = Brushes.Red;
                }

                if(artists.Count == 0)
                {
                    ArtistListBox.BorderBrush = Brushes.Red;
                }

                if(int.TryParse(BitrateTextBox.Text, out _) == false)
                {
                    BitrateTextBox.BorderBrush = Brushes.Red;
                }

                if(string.IsNullOrEmpty(DurationTextBox.Text) || DurationTextBox.Text == "00:00:00")
                {
                    DurationTextBox.BorderBrush = Brushes.Red;
                }

                MessageBox.Show("Какие то из полей не заполенены");
                return;
            }
        }

        private void DeleteGenre_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            genres.Remove(button.DataContext as Genre);
        }

        private void DeleteArtist_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            artists.Remove(button.DataContext as Artist);
        }
    }
}
