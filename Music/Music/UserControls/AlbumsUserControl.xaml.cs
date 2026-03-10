using Microsoft.EntityFrameworkCore;
using Music.Context;
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
using System.Windows.Controls.Primitives;
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
        MenuWindow OwnerWindow = null!;

        private List<Album> _albums = null!;

        private bool _isGuest = false;

        public AlbumsUserControl()
        {
            InitializeComponent();
        }

        public AlbumsUserControl(MenuWindow w, bool isGuest)
        {
            InitializeComponent();
            Loaded += (_, _) => Load();
            OwnerWindow = w;
            _isGuest = isGuest;
        }

        public async void Load()
        {
            if (_isGuest)
            {
                AlbumsListBox.SelectionChanged -= AlbumsListBox_SelectionChanged;
                SortingComboBox.IsEnabled = false;
                SearchBox.IsEnabled = false;
                FiltersButton.IsEnabled = false;
            }

            await using MusicContext context = new();

            _albums = await context.Albums
                .Include(x => x.Tracks).ThenInclude(x => x.Artists)
                .Include(x => x.Artist)
                .Include(x => x.Genres)
                .ToListAsync();
            AlbumsListBox.ItemsSource = _albums;
            GenreBox.ItemsSource = await context.Genres.ToListAsync();

            ConfirmFilters();
        }

        private void AlbumsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Album? album = (AlbumsListBox.SelectedItem as Album)!;
            if (album != null)
            {
                AlbumsListBox.SelectedItem = null;
                new OpenAlbumWindow(album, OwnerWindow.User) { Owner = OwnerWindow }.ShowDialog();
            }
        }

        private void ConfirmFilters()
        {
            if (!IsLoaded)
                return;

            List<Album> filteredAlbums = _albums;

            switch (SortingComboBox.SelectedIndex)
            {
                case 0:
                    filteredAlbums = _albums;
                    break;
                case 1:
                    filteredAlbums = filteredAlbums.OrderBy(x => x.TotalDuration).ToList();
                    break;
                case 2:
                    filteredAlbums = filteredAlbums.OrderByDescending(x => x.TotalDuration).ToList();
                    break;
            }

            if (SearchBox != null && !string.IsNullOrEmpty(SearchBox.Text))
                filteredAlbums = filteredAlbums.Where(x => x.Title.Contains(SearchBox.Text, StringComparison.CurrentCultureIgnoreCase)).ToList();

            if (!string.IsNullOrEmpty(GenreBox.Text) && GenreBox.SelectedItem is Genre selectedGenre)
                filteredAlbums = filteredAlbums.Where(x => x.Genres.Contains(selectedGenre)).ToList();

            AlbumsListBox.ItemsSource = filteredAlbums;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ConfirmFilters();
        }

        private void SortingComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ConfirmFilters();
        }

        private void FiltersButton_Checked(object sender, RoutedEventArgs e)
        {
            FiltersPopup.IsOpen = true;
        }

        private void FiltersButton_Unchecked(object sender, RoutedEventArgs e)
        {
            FiltersPopup.IsOpen = false;
        }

        private void FiltersPopup_Closed(object sender, EventArgs e)
        {
            if (FiltersPopup.IsOpen == false)
                FiltersButton.IsChecked = false;
        }

        private void GenreBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ConfirmFilters();
        }
    }
}
