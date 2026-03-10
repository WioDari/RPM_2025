using Microsoft.EntityFrameworkCore;
using Music.Context;
using Music.Models;
using Music.UserControls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Music.Windows
{
    /// <summary>
    /// Логика взаимодействия для EditAlbumWindow.xaml
    /// </summary>
    public partial class EditAlbumWindow : Window
    {
        private ObservableCollection<Models.Track> _availibleTracks = null!;

        private Album _album = null!;

        private MusicContext _context = null!;

        public EditAlbumWindow()
        {
            InitializeComponent();
        }

        public EditAlbumWindow(int albumId)
        {
            InitializeComponent();
            Loaded += (_, _) => OnLoaded(albumId);
        }

        private async void OnLoaded(int albumId)
        {
            _context = new MusicContext();

            _album = await _context.Albums.Include(x => x.Tracks).Include(x => x.Genres).Include(x => x.Artist).FirstAsync(x => x.Id == albumId);
            DataContext = _album;

            _availibleTracks = new ObservableCollection<Models.Track>(_album.Tracks);

            ArtistBox.ItemsSource = await _context.Artists.Include(x => x.Tracks).ToListAsync();
            GenreBox.ItemsSource = await _context.Genres.ToListAsync();
            TrackBox.ItemsSource = _availibleTracks;

            GenresListBox.ItemsSource = _album.Genres;
            TracksListBox.ItemsSource = _album.Tracks;
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StringBuilder sb = new("Неверно указаны данные:\n");

                if (string.IsNullOrEmpty(TitleBox.Text))
                {
                    sb.AppendLine("Укажите название альбома");
                }
                if ((ArtistBox.SelectedItem as Artist) == null)
                {
                    sb.AppendLine("Укажите исполнителя");
                }
                if (!int.TryParse(ReleaseYearBox.Text, out _) || int.Parse(ReleaseYearBox.Text) < 1 || int.Parse(ReleaseYearBox.Text) > DateTime.Now.Year)
                {
                    sb.AppendLine("Укажите корректный год выпуска");
                }
                if (_album.Genres.Count == 0)
                {
                    sb.AppendLine("Добавьте хотя бы один жанр");
                }
                if (_album.Tracks.Count == 0)
                {
                    sb.AppendLine("Добавьте хотя бы один трек");
                }

                _album.Title = TitleBox.Text;
                _album.ReleaseYear = int.Parse(ReleaseYearBox.Text);
                _album.ArtistId = (ArtistBox.SelectedItem as Artist)!.Id;
                _album.CoverPath = UriBox.Text;

                _album.TotalDuration = TimeSpan.Zero;
                foreach (var t in _album.Tracks)
                    _album.TotalDuration += t.Duration;

                if (sb.ToString() != "Неверно указаны данные:\n")
                {
                    MessageBox.Show(sb.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    IsEnabled = false;

                    await _context.SaveChangesAsync();

                    IsEnabled = true;

                    MessageBox.Show($"Альбом {_album.Title} был успешно изменён!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    Close();


                }
            }
            catch (Exception ex)
            {
                IsEnabled = true;
                Console.WriteLine(ex);
                MessageBox.Show("Некорректный ввод данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private async void UriBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Image.Source = Img.GetImage((sender as TextBox)!.Text);
        }

        private void ArtistBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var popup = (Popup)ArtistBox.Template.FindName("PART_Popup", ArtistBox);
            if (popup != null)
            {
                popup.IsOpen = true;
            }
        }

        private void ArtistBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var popup = (Popup)ArtistBox.Template.FindName("PART_Popup", ArtistBox);
            if (popup != null)
            {
                popup.IsOpen = false;
            }
        }

        private void AddGenreButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedGenre = GenreBox.SelectedItem as Genre;

            if (selectedGenre != null && !_album.Genres.Contains(selectedGenre))
            {
                _album.Genres.Add(selectedGenre);
            }

        }

        private void AddTrackButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedTrack = TrackBox.SelectedItem as Models.Track;

            if (selectedTrack != null && !_album.Tracks.Contains(selectedTrack))
            {
                _album.Tracks.Add(selectedTrack);
            }
        }

        private void RemoveGenre_Click(object sender, RoutedEventArgs e)
        {
            var button = (sender as Button);
            _album.Genres.Remove(button.DataContext as Genre);
        }

        private void RemoveTrack_Click(object sender, RoutedEventArgs e)
        {
            var button = (sender as Button);
            _album.Tracks.Remove(button.DataContext as Models.Track);
        }

        private void CreateTrackButton_Click(object sender, RoutedEventArgs e)
        {
            var addTrackWindow = new AddTrackWindow(_context) { Owner = this };

            if (addTrackWindow.ShowDialog() == true)
            {
                addTrackWindow.CurrentTrack.AlbumId = _album.Id;
                _album.Tracks.Add(addTrackWindow.CurrentTrack);
                _availibleTracks.Add(addTrackWindow.CurrentTrack);
                TrackBox.ItemsSource = _availibleTracks;
            }
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Вы уверены, что хотите удалить этот альбом со всеми его треками?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result != MessageBoxResult.Yes)
            {
                return;
            }
            _context.Albums.Remove(_album);
            await _context.SaveChangesAsync();
            Close();
            MessageBox.Show($"Альбом {_album.Title} был успешно удалён.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

        }
    }
}
