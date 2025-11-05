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
using spotify.Context;

namespace spotify
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SpotifyContext spotifyContext = new();
        public MainWindow()
        {
            InitializeComponent();

            AmountTrack.Text = spotifyContext.Tracks.Count().ToString();
            AmountAlbum.Text = spotifyContext.Albums.Count().ToString();
            AmountArtist.Text = spotifyContext.Artists.Count().ToString();
        }
    }
}