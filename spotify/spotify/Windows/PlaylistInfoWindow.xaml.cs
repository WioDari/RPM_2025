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
    /// Логика взаимодействия для PlaylistInfoWindow.xaml
    /// </summary>
    public partial class PlaylistInfoWindow : Window
    {
        Playlist playlist1 = null!;
        public PlaylistInfoWindow()
        {
            InitializeComponent();
        }

        public PlaylistInfoWindow(Playlist playlist)
        {
            InitializeComponent();
            SpotifyContext spotifyContext = new();
            playlist1 = playlist;

            TitleTextBlock.Text = playlist1.PlaylistNameAndFullName;
            DateTextBlock.Text = playlist1.DateCreated.ToString();
            LikesTextBlock.Text = playlist1.Likes.ToString();
            Duration.Text = playlist1.Duration;
            TrackListBox.ItemsSource = playlist1.Tracks.ToList();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ChangeButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
