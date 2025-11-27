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
using spotify.Models;

namespace spotify.Windows
{
    /// <summary>
    /// Логика взаимодействия для UserControlPlaylist.xaml
    /// </summary>
    public partial class UserControlPlaylist : UserControl
    {
        public UserControlPlaylist()
        {
            InitializeComponent();
            Load();
        }

        public void Load()
        {
            SpotifyContext context = new();
            PlaylistListBox.ItemsSource = context.Playlists.Include(x => x.Tracks).Include(x => x.Tags).Include(x => x.Users).Include(x => x.Users).ToList();
        }
    }
}
