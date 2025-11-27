using Microsoft.EntityFrameworkCore;
using Music.Models;
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
        public AlbumsUserControl()
        {
            InitializeComponent();
            Load();
        }

        private async void Load()
        {
            await using Context.MusicContext context = new();

            AlbumsListBox.ItemsSource = context.Albums
                .Include(x => x.Tracks)
                .Include(x => x.Artist)
                .Include(x => x.Genres)
                .ToList();
        }
    }
}
