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

        private MusicContext _context = null!;

        public AddTrackWindow()
        {
            InitializeComponent();
        }

        public AddTrackWindow(MusicContext context)
        {
            InitializeComponent();
            _context = context;
            Loaded += OnLoaded;
        }

        private async void OnLoaded(object? sender, RoutedEventArgs e)
        {
            FilePathBox.Text = "";
            DurationStringBox.Text = "00:00:00";
            ReleaseDateTimePicker.SelectedDate = DateTime.Now;

            ArtistBox.ItemsSource = await _context.Artists.ToListAsync();
            GenresBox.ItemsSource = await _context.Genres.ToListAsync();

            ArtistsListBox.ItemsSource = CurrentTrack.Artists;
            GenresListBox.ItemsSource = CurrentTrack.Genres;
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
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
                sb.AppendLine("Введите корректный битрейт трека");
            }
            if (CurrentTrack.Artists.Count == 0)
            {
                sb.AppendLine("Добавьте хотя бы одного исполнителя");
            }
            if (CurrentTrack.Genres.Count == 0)
            {
                sb.AppendLine("Добавьте хотя бы один жанр");
            }

            if (sb.ToString() != "Неверно введены данные:\n")
            {
                MessageBox.Show(sb.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            CurrentTrack = new Track
            {
                TrackName = TrackNameBox.Text,
                DurationString = DurationStringBox.Text,
                ReleaseDateTime = ReleaseDateTimePicker.SelectedDate,
                Bitrate = int.Parse(BitrateBox.Text),
                FilePath = FilePathBox.Text,
                Raiting = 0m,
                PlayCount = 0,
                Artists = CurrentTrack.Artists,
                Genres = CurrentTrack.Genres
            };

            DialogResult = true;

        }

        private void AddGenreButton_Click(object sender, RoutedEventArgs e)
        {
            var genre = (GenresBox.SelectedItem as Genre)!;
            if (genre == null || CurrentTrack.Genres.Contains(genre) == true)
            {
                return;
            }
            CurrentTrack.Genres.Add(genre);
        }

        private void AddArtistButton_Click(object sender, RoutedEventArgs e)
        {
            var artist = ArtistBox.SelectedItem as Artist;
            if (artist == null || CurrentTrack.Artists.Contains(artist) == true)
            {
                return;
            }
            CurrentTrack.Artists.Add(artist);
        }

        private void RemoveGenre_Click(object sender, RoutedEventArgs e)
        {
            var button = (sender as Button);
            CurrentTrack.Genres.Remove(button.DataContext as Genre);
        }

        private void RemoveArtist_Click(object sender, RoutedEventArgs e)
        {
            var button = (sender as Button);
            CurrentTrack.Artists.Remove(button.DataContext as Artist);
        }
    }
}
