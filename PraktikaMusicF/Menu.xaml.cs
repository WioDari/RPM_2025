using PraktikaMusicF.Context;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;

namespace PraktikaMusicF
{
    /// <summary>
    /// Логика взаимодействия для Menu.xaml
    /// </summary>
    public partial class Menu : Window, INotifyPropertyChanged
    {
        private string _nameUsr;

        public string NameUsr
        {
            get => _nameUsr;
            set
            {
                _nameUsr = value;
                OnPropertyChanged(nameof(NameUsr));
            }
        }
        public Menu()
        {
            InitializeComponent();
            DataContext = this;
            LoadPlaylistsList();
            LoadAlbumsList();
        }

        public Menu(string nameUsr) : this()
        {
            InitializeComponent();
            NameUsr = nameUsr;
        }

        public void LoadPlaylistsList()
        {
            using var mbdf = new MusicBdFContext();
            var allPl = mbdf.Playlists.ToList();

            var stack = new StackPanel
            {
                Orientation = Orientation.Vertical
            };

            foreach (var pl in allPl)
            {
                var pluc = new PlaylistUC();
                pluc.LoadPlaylist(pl);
                stack.Children.Add(pluc);
            }

            plFrame.Content = stack;
        }

        public void LoadAlbumsList()
        {
            using var mbdf = new MusicBdFContext();
            var allAlb = mbdf.Albums.ToList();

            alItemsControl.Items.Clear();

            foreach (var al in allAlb)
            {
                var aluc = new AlbumsUC();
                aluc.LoadAlbum(al);
                alItemsControl.Items.Add(aluc);
            }
        }
    

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
