using PraktikaMusicF.Context;
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

namespace PraktikaMusicF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int _trackCount;

        public int trackCount
        {
            get => _trackCount;
            set
            {
                _trackCount = value;
                TrackCount.Text = "Количество треков: " + value.ToString();
            }
        }

        private int _albCount;

        public int albCount
        { 
            get => _albCount;
            set
            {
                _albCount = value;
                AlbCount.Text = "Количество альбомов: " + value.ToString();
            }
        }

        private int _plalCount;
        
        public int plalCount
        {
            get => _plalCount;
            set
            {
                _plalCount = value;
                PlalCount.Text = "Количество плейлистов: " + value.ToString();
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            
            CountElements();
        }

        public void CountElements()
        {
            MusicBdFContext mbdf = new MusicBdFContext();
            trackCount = mbdf.Tracks.Count();
            albCount = mbdf.Albums.Count();
            plalCount = mbdf.Playlists.Count();
        }

    }
}