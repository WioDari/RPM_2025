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
using Spotify_wpf.Context;

namespace Spotify_wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            SelcetDate();
        }

        public void SelcetDate()
        {
            MusicContext con = new MusicContext();
            TrackCount.Text = $"Количество треков:{con.Tracks.Count()}";
            AlbumCount.Text = $"Количество альбомов:{con.Albums.Count()}";
            PlaylistCount.Text = $"Количество плейлистов:{con.Playlists.Count()}";
        }
    }
}