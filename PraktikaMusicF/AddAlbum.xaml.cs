using PraktikaMusicF.Context;
using PraktikaMusicF.Models;
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
using System.Net.Http;
using System.IO;


namespace PraktikaMusicF
{
    /// <summary>
    /// Логика взаимодействия для AddAlbum.xaml
    /// </summary>
    public partial class AddAlbum : Window
    {
        private ObservableCollection<Genre> _selectedGenres= new();
        private List<Genre> _allGenres = new();
        private List<Artist> _artists = new();
        private Artist? _selectedArtist = new();

        public AddAlbum()
        {
            InitializeComponent();

            GetList();
        }

        public void GetList()
        {
            MusicBdFContext mbdf = new();
            _allGenres = mbdf.Genres.ToList();
            genreList.ItemsSource = _allGenres;

            GetArtists();
        }

        public void GetArtists()
        {
            MusicBdFContext mbdf = new();
            _artists = mbdf.Artists.ToList();
            comboArt.ItemsSource = _artists;
        }

        private async void LoadImage_Click(object sender, RoutedEventArgs e)
        {
            string url = urlLinkImage.Text;
            if (string.IsNullOrEmpty(url))
            {
                MessageBox.Show("Введено пустое поле", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            MessageBox.Show("Картинка загружается","Успешно",MessageBoxButton.OK, MessageBoxImage.Information);
            using (HttpClient client = new HttpClient())
            {
                var bytes = await client.GetByteArrayAsync(url);

                BitmapImage bitmap = new BitmapImage();
                using (var ms = new MemoryStream(bytes))
                {
                    bitmap.BeginInit();
                    bitmap.StreamSource = ms;
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                }

                albImage.Source = bitmap;
            }
        }

        private void comboArt_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboArt.SelectedItem is Artist selected)
            {
                _selectedArtist = selected;
            }
        }

        private void saveAlb_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(nameAlb.Text))
            {
                MessageBox.Show("Вы не написали название альбома", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            if (_selectedArtist == null)
            {
                MessageBox.Show("Вы не выбрали артиста","Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (datePicker.SelectedDate == null)
            {
                MessageBox.Show("Вы не выбрали дату", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrWhiteSpace(urlLinkImage.Text))
            {
                MessageBox.Show("Введите ссылку на изображение", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            using var mbdf = new MusicBdFContext();
            try
            {
                var newAlb = new Album
                {
                    AlbName = nameAlb.Text,
                    ArtistId = _selectedArtist.ArtId,
                    AlbImage = urlLinkImage.Text,
                    AlbRelease = DateOnly.FromDateTime(datePicker.SelectedDate.Value)
                };

                mbdf.Albums.Add(newAlb);
                mbdf.SaveChanges();

                foreach (var genre in _selectedGenres)
                {
                    mbdf.AlbumsGenres.Add(new AlbumsGenre
                    {
                        AlbId = newAlb.AlbId,
                        GenreId = genre.GenreId
                    });
                }

                mbdf.SaveChanges();

                MessageBox.Show("Альбом успешно сохранён!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private void genreList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedGenres = new ObservableCollection<Genre>(
                              genreList.SelectedItems.Cast<Genre>()
                              );
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
