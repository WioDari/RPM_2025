using Microsoft.EntityFrameworkCore;
using Music.Models;
using Music.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private Window OwnerWindow = null!;
        private bool _isGuest = false;
        private ObservableCollection<Playlist> _playlists = new();

        public PlaylistsUserControl()
        {
            InitializeComponent();
            Loaded += (_, _) => Load();
            PlaylistsListBox.ItemsSource = _playlists;
        }

        public PlaylistsUserControl(Window w, bool isGuest)
        {
            InitializeComponent();
            OwnerWindow = w;
            _isGuest = isGuest;
            PlaylistsListBox.ItemsSource = _playlists;
            Loaded += (_, _) => Load();

        }

        public void Load()
        {
            if (_isGuest)
            {
                PlaylistsListBox.SelectionChanged -= PlaylistsListBox_SelectionChanged;
            }

            using Context.MusicContext context = new();

            var playlistsFromDb = context.Playlists
                .Include(x => x.Tracks).ThenInclude(x => x.Artists)
                .Include(x => x.Tags)
                .Include(x => x.CreatorUser)
                .Include(x => x.Users)
                .ToList();

            _playlists.Clear();
            foreach (var playlist in playlistsFromDb)
                _playlists.Add(playlist);

            
        }

        private void PlaylistsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Playlist? playlist = (PlaylistsListBox.SelectedItem as Playlist)!;
            if (playlist != null)
            {
                PlaylistsListBox.SelectedItem = null;
                new OpenPlaylistWindow(playlist, (OwnerWindow as MenuWindow)!.User) { Owner = OwnerWindow }.ShowDialog();
            }
        }
    }
}
