using SpotApp_wpf.Context;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SpotApp_wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ShowCount();
        }

        private void ShowCount()
        {
            var context = new SpotifyContext();
            TracksCount.Text = $"Кол-во треков: {context.Tracks.Count()}";
            AlbumsCount.Text = $"Кол-во Альбомов: {context.Albums.Count()}";
            PlaylistsCount.Text = $"Кол-во плейлистов: {context.Playlists.Count()}";
        }
    }
}