using Microsoft.EntityFrameworkCore;
using Music.Models;
using Music.Windows;
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
    /// Логика взаимодействия для AlbumsUserControl.xaml
    /// </summary>
    public partial class AlbumsUserControl : UserControl
    {
        Window OwnerWindow;
        public AlbumsUserControl(Window w)
        {
            InitializeComponent();
            Loaded += Load;
            OwnerWindow = w;
        }

        public async void Load(object? sender, RoutedEventArgs e)
        {
            await using Context.MusicContext context = new();

            AlbumsListBox.ItemsSource = await context.Albums
                .Include(x => x.Tracks).ThenInclude(x => x.Artists)
                .Include(x => x.Artist)
                .Include(x => x.Genres)
                .ToListAsync();
        }

        private void AlbumsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Album? album = (AlbumsListBox.SelectedItem as Album)!;
            if (album != null)
            {
                AlbumsListBox.SelectedItem = null;
                new OpenAlbumWindow(album) { Owner = OwnerWindow}.ShowDialog();
            }
        }
    }
}
