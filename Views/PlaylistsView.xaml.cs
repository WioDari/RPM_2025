using System.Windows;
using System.Windows.Controls;
using System.Linq;
using MusicPlus.Models;
using MusicPlus.ViewModels;

namespace MusicPlus.Views
{
    public partial class PlaylistsView : UserControl
    {
        private readonly PlaylistsViewModel _viewModel;
        private User _currentUser;

        public PlaylistsView(User user)
        {
            InitializeComponent();
            _currentUser = user;
            _viewModel = new PlaylistsViewModel(user);
            DataContext = _viewModel;
            
            _viewModel.PlaylistSelected += ViewModel_PlaylistSelected;
            _viewModel.AddPlaylistRequested += ViewModel_AddPlaylistRequested;
            _viewModel.EditPlaylistRequested += ViewModel_EditPlaylistRequested;
        }

        private void AddPlaylist_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.AddPlaylistCommand.Execute(null);
        }

        private void PlaylistName_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is TextBlock textBlock && textBlock.DataContext is PlaylistViewModel playlist)
            {
                _viewModel.ViewPlaylistDetailsCommand.Execute(playlist);
            }
        }

        private void ViewModel_PlaylistSelected(object? sender, PlaylistViewModel playlist)
        {
            var detailView = new PlaylistDetailView(playlist.PlaylistID, _currentUser);
            detailView.ShowDialog();
        }

        private void ViewModel_AddPlaylistRequested(object? sender, System.EventArgs e)
        {
            var addPlaylistView = new AddEditPlaylistView(_currentUser);
            if (addPlaylistView.ShowDialog() == true)
            {

                _viewModel.LoadPlaylistsCommand.Execute(null);
            }
        }

        private void ViewModel_EditPlaylistRequested(object? sender, PlaylistViewModel playlist)
        {

            using (var context = _viewModel.GetContext())
            {
                var playlistToEdit = context.Playlists.FirstOrDefault(p => p.PlaylistID == playlist.PlaylistID);
                if (playlistToEdit != null)
                {
                    var editPlaylistView = new AddEditPlaylistView(_currentUser, playlistToEdit);
                    if (editPlaylistView.ShowDialog() == true)
                    {

                        _viewModel.LoadPlaylistsCommand.Execute(null);
                    }
                }
            }
        }
    }
}
