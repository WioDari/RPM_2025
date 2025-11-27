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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.EntityFrameworkCore;
using spotify.Context;

namespace spotify.Windows
{
    /// <summary>
    /// Логика взаимодействия для UserControlTrack.xaml
    /// </summary>
    public partial class UserControlAlbum : UserControl
    {
        public UserControlAlbum()
        {
            InitializeComponent();
            Load();
        }

        public void Load()
        {
            SpotifyContext context = new();
            AlbumListBox.ItemsSource = context.Albums.Include(x => x.Artist).Include(x => x.Tracks);
        }
    }
}
