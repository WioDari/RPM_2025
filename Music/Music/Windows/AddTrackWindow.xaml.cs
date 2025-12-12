using Microsoft.EntityFrameworkCore;
using Music.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using Music.Context;

namespace Music.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddTrackWindow.xaml
    /// </summary>
    public partial class AddTrackWindow : Window
    {
        public Track CurrentTrack = new Track();

        private ObservableCollection<Artist> _artists = new();
        private ObservableCollection<Genre> _genres = new();

        public AddTrackWindow()
        {
            InitializeComponent();
            IsEnabled = false;
            Loaded += OnLoaded;
        }

        private async void OnLoaded(object? sender, RoutedEventArgs e)
        {
            FilePathBox.Text = "";
            DurationStringBox.Text = "00:00:00";
            ReleaseDateTimePicker.SelectedDate = DateTime.Now;

            //await using Context.MusicContext context = new();
            ArtistBox.ItemsSource = await DB.Context.Artists.ToListAsync();
            GenresBox.ItemsSource = await DB.Context.Genres.ToListAsync();

            ArtistsListBox.ItemsSource = _artists;
            GenresListBox.ItemsSource = _genres;

            IsEnabled = true;
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            //CurrentTrack = (DataContext as Track)!;

            StringBuilder sb = new("Неверно введены данные:\n");

            if (string.IsNullOrEmpty(TrackNameBox.Text))
            {
                sb.AppendLine("Введите название трека");
            }
            if (!TimeSpan.TryParse(DurationStringBox.Text, out TimeSpan d) || d <= TimeSpan.Zero)
            {
                sb.AppendLine("Введите длительность трека");
            }
            if (!int.TryParse(BitrateBox.Text, out int b) || b <= 0)
            {
                sb.AppendLine("Введите битрейт трека");
            }
            if (_artists.Count == 0)
            {
                sb.AppendLine("Добавьте хотя бы одного исполнителя");
            }
            if (_genres.Count == 0)
            {
                sb.AppendLine("Добавьте хотя бы один жанр");
            }
            
            if (sb.ToString() != "Неверно введены данные:\n")
            {
                MessageBox.Show(sb.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                //await using Context.MusicContext context = new();
                CurrentTrack = new Track
                {
                    Id = 0,
                    TrackName = TrackNameBox.Text,
                    AlbumId = 0,
                    DurationString = DurationStringBox.Text,
                    ReleaseDateTime = ReleaseDateTimePicker.SelectedDate,
                    Bitrate = int.Parse(BitrateBox.Text),
                    FilePath = FilePathBox.Text,
                    Raiting = 0m,
                    PlayCount = 0,
                    Artists = _artists,
                    Genres = _genres
                };

                DialogResult = true;
            }

        }

        private void AddGenreButton_Click(object sender, RoutedEventArgs e)
        {
            var genre = (GenresBox.SelectedItem as Genre)!;
            if (genre == null || _genres.Contains(genre) == true)
            {
                return;
            }
            _genres.Add(genre) ;
        }

        private void AddArtistButton_Click(object sender, RoutedEventArgs e)
        {
            var artist = ArtistBox.SelectedItem as Artist;
            if (artist == null || _artists.Contains(artist) == true)
            {
                return;
            }
            _artists.Add(artist);
        }

        private void RemoveGenre_Click(object sender, RoutedEventArgs e)
        {
            var button = (sender as Button);
            _genres.Remove(button.DataContext as Genre);
        }

        private void RemoveArtist_Click(object sender, RoutedEventArgs e)
        {
            var button = (sender as Button);
            _artists.Remove(button.DataContext as Artist);
        }
    }
}
