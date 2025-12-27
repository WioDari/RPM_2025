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

        private void Load()
        {
            SpotifyContext context = new();
            PlaylistListBox.ItemsSource = context.Playlists.Include(x => x.Tracks).ThenInclude(x => x.Artists).Include(x => x.Users).ToList();
        }

        private void PlaylistListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (PlaylistListBox.SelectedItem is Playlist playlist && playlist != null)
            {
                PlaylistInfoWindow playlistInfoWindow = new(playlist);
                playlistInfoWindow.ShowDialog();
            }
        }
    }
}
