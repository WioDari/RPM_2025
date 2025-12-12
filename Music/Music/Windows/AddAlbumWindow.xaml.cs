using AutoCompleteTextBox.Editors;
using Microsoft.EntityFrameworkCore;
using Music.Context;
using Music.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
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
    /// Логика взаимодействия для AddAlbumWindow.xaml
    /// </summary>
    public partial class AddAlbumWindow : Window
    {
        private ObservableCollection<Models.Track> _availibleTracks = new();
        private ObservableCollection<Genre> _genres = new();
        private ObservableCollection<Models.Track> _tracks = new();

        public AddAlbumWindow()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private async void OnLoaded(object? sender, RoutedEventArgs e)
        {
            //await using MusicContext context = new MusicContext();

            ReleaseYearBox.Text = DateTime.Now.Year.ToString();

            await Task.Run(DB.LoadContext);

            ArtistBox.ItemsSource = await DB.Context.Artists.Include(x => x.Tracks).ToListAsync();
            GenreBox.ItemsSource = await DB.Context.Genres.ToListAsync();
            TrackBox.ItemsSource = _availibleTracks;

            GenresListBox.ItemsSource = _genres;
            TracksListBox.ItemsSource = _tracks;

            
        }

        private void UriBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Image.Source = Img.GetImage((sender as TextBox)!.Text);
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            //await using MusicContext context = new MusicContext();
            try
            {
                //Album album = (Album)DataContext;

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
                if (_genres.Count == 0)
                {
                    sb.AppendLine("Добавьте хотя бы один жанр");
                }
                if (_tracks.Count == 0)
                {
                    sb.AppendLine("Добавьте хотя бы один трек");
                }

                Album album = new()
                {
                    Id = await DB.Context.Albums.AnyAsync() ? (await DB.Context.Albums.MaxAsync(a => a.Id) + 1) : 1,
                    Title = TitleBox.Text,
                    ReleaseYear = int.Parse(ReleaseYearBox.Text),
                    ArtistId = (ArtistBox.SelectedItem as Artist).Id,
                    CoverPath = UriBox.Text,
                    Tracks = _tracks,
                    Genres = _genres,
                };

                foreach (var t in album.Tracks)
                    album.TotalDuration += t.Duration;

                if (sb.ToString() != "Неверно указаны данные:\n")
                {
                    MessageBox.Show(sb.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    IsEnabled = false;

                    /*await Task.Run(() =>
                    {
                        DB.Context.Entry(album).State = EntityState.Modified;
                        //context.Tracks.Entry(_tracks).State = EntityState.Modified;
                    });*/


                    await DB.Context.AddAsync(album);
                    await DB.Context.SaveChangesAsync();

                    

                    IsEnabled = true;

                    MessageBox.Show($"Альбом {album.Title} был успешно добавлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

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
            /*var album = DataContext as Album;
            if (selectedGenre != null && album != null && !album.Genres.Contains(selectedGenre))
            {
                album.Genres.Add(selectedGenre);
            }*/
            if (selectedGenre != null && !_genres.Contains(selectedGenre))
            {
                _genres.Add(selectedGenre);
            }

        }

        private void AddTrackButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedTrack = TrackBox.SelectedItem as Models.Track;
            /*var album = DataContext as Album;
            if (selectedTrack != null && album != null && !album.Tracks.Contains(selectedTrack))
            {
                album.Tracks.Add(selectedTrack);
            }*/

            if (selectedTrack != null && !_tracks.Contains(selectedTrack))
            {
                _tracks.Add(selectedTrack);
            }
        }

        private void RemoveGenre_Click(object sender, RoutedEventArgs e)
        {
            var button = (sender as Button);
            _genres.Remove(button.DataContext as Genre);
        }

        private void RemoveTrack_Click(object sender, RoutedEventArgs e)
        {
            var button = (sender as Button);
            _tracks.Remove(button.DataContext as Models.Track);
        }

        private void CreateTrackButton_Click(object sender, RoutedEventArgs e)
        {
            var addTrackWindow = new AddTrackWindow { Owner = this };

            if (addTrackWindow.ShowDialog() == true)
            {
                _tracks.Add(addTrackWindow.CurrentTrack);
                _availibleTracks.Add(addTrackWindow.CurrentTrack);
                TrackBox.ItemsSource = _availibleTracks;
            }
        }
    }

}
