using System.Linq;
using System.Windows;
using MusicPlus.Models;
using MusicPlus.ViewModels;

namespace MusicPlus.Views
{
    public partial class AddEditTrackView : Window
    {
        private readonly AddEditTrackViewModel _viewModel;

        public AddEditTrackView(Track? track = null, int? albumId = null)
        {
            InitializeComponent();
            _viewModel = new AddEditTrackViewModel(track, albumId);
            DataContext = _viewModel;
            
            _viewModel.CloseRequested += ViewModel_CloseRequested;

            Loaded += AddEditTrackView_Loaded;
        }

        private void AddEditTrackView_Loaded(object sender, RoutedEventArgs e)
        {

            var artistsListBox = FindName("ArtistsListBox") as System.Windows.Controls.ListBox;
            if (artistsListBox != null && _viewModel.SelectedArtists.Any())
            {
                artistsListBox.SelectedItems.Clear();
                foreach (var artist in _viewModel.Artists)
                {
                    if (_viewModel.SelectedArtists.Any(sa => sa.ArtistID == artist.ArtistID))
                    {
                        artistsListBox.SelectedItems.Add(artist);
                    }
                }
            }
        }

        private void ArtistsListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (sender is System.Windows.Controls.ListBox listBox && _viewModel != null)
            {
                _viewModel.SelectedArtists.Clear();
                foreach (Artist artist in listBox.SelectedItems)
                {
                    _viewModel.SelectedArtists.Add(artist);
                }
            }
        }

        private void AlbumComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            _viewModel?.OnAlbumSelected();
        }

        private void ViewModel_CloseRequested(object? sender, bool saved)
        {
            DialogResult = saved;
            Close();
        }
    }
}
