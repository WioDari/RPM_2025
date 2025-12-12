using Microsoft.EntityFrameworkCore;
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

namespace Music.UserControls
{
    /// <summary>
    /// Логика взаимодействия для PlaylistsUserControl.xaml
    /// </summary>
    public partial class PlaylistsUserControl : UserControl
    {
        public PlaylistsUserControl()
        {
            InitializeComponent();
            Loaded += Load;
        }

        private void Load(object? sender, RoutedEventArgs e)
        {
            using Context.MusicContext context = new();
            PlaylistsListBox.ItemsSource = context.Playlists
                .Include(x => x.Tracks)
                .Include(x => x.Tags)
                .Include(x => x.CreatorUser)
                .Include(x => x.Users)
                .ToList();
        }
    }
}
