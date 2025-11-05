using Microsoft.EntityFrameworkCore;
using Music.Context;
using System.Runtime.CompilerServices;
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

namespace Music
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Load();
        }
        private void Load()
        {
            MusicContext context = new();
            TracksCount.Text = context.Tracks.Count().ToString();
            AlbumsCount.Text = context.Albums.Count().ToString();
            PlaylistCount.Text = context.Playlists.Count().ToString();
        }
    }
}