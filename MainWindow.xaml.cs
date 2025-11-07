using MusicWpf.Context;
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

namespace MusicWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Data();
        }
        public void Data()
        {
            MusicContext context = new MusicContext();

            trecksBox.Text = $"Количество треков: {context.Tracks.Count()}";
            albumBox.Text = $"Количество альбомов: {context.Albums.Count()}";
            playlistBox.Text = $"Количество плейлистов: {context.Playlists.Count()}";
        }
    }
}