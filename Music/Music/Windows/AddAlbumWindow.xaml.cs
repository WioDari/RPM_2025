using AutoCompleteTextBox.Editors;
using Microsoft.EntityFrameworkCore;
using Music.Context;
using Music.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Music.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddAlbumWindow.xaml
    /// </summary>
    public partial class AddAlbumWindow : Window
    {
        public AddAlbumWindow()
        {
            InitializeComponent();
            Load();
        }

        private async void Load()
        {
            await using MusicContext context = new MusicContext();
            ArtistBox.ItemsSource = await context.Artists.Include(x => x.Tracks).ToListAsync();
            GenreBox.ItemsSource = await context.Genres.ToListAsync();
            TrackBox.ItemsSource = await context.Tracks.ToListAsync();
            DataContext = await context.Albums.Include(x => x.Genres).Include(x => x.Artist).FirstAsync();
        }

        private void UriBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Image.Source = Img.GetImage((sender as TextBox)!.Text);
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string errors = "";

        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ArtistBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var popup = (Popup)ArtistBox.Template.FindName("PART_Popup", ArtistBox);
            if (popup != null)
            {
                popup.IsOpen = true;
            }
        }

        private void ArtistBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var popup = (Popup)ArtistBox.Template.FindName("PART_Popup", ArtistBox);
            if (popup != null)
            {
                popup.IsOpen = false;
            }
        }

        private void AddGenreButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedGenre = GenreBox.SelectedItem as Genre;
        }
    }

}
