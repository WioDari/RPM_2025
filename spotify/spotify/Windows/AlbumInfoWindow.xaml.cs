using Microsoft.EntityFrameworkCore;
using spotify.Context;
using spotify.Models;
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

namespace spotify.Windows
{
    /// <summary>
    /// Логика взаимодействия для AlbumInfoWindow.xaml
    /// </summary>
    public partial class AlbumInfoWindow : Window
    {
        Album album1 = new();
        public AlbumInfoWindow()
        {
            InitializeComponent();
        }

        public AlbumInfoWindow(Album album)
        {
            SpotifyContext spotifyContext = new();
            album1 = spotifyContext.Albums.Include(x => x.Artist).Include(x => x.Genres).Include(x => x.Tracks).ThenInclude(x => x.Artists).FirstOrDefault(x => x.Id == album.Id)!;
            InitializeComponent();
            AlbumCover.Source = new BitmapImage(new Uri(album1.CoverPath, UriKind.RelativeOrAbsolute));
            AlbumTitleTExtBlock.Text = album1.AlbumTitle;
            ArtistNameTextBlock.Text = album1.Artist.ArtistName;
            GenreTextBlock.Text = album1.GenresToString;
            TrackListBox.ItemsSource = album1.Tracks.ToList();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ChangeButton_Click(object sender, RoutedEventArgs e)
        {
            NewAlbum newAlbum = new(album1);
            newAlbum.Show();
            this.Close();
        }
    }
}
