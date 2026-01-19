using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using MusicPlus.Models;
using MusicPlus.ViewModels;

namespace MusicPlus.Views
{
    public partial class TracksView : UserControl
    {
        private readonly TracksViewModel _viewModel;
        private User _currentUser;

        public TracksView(User user)
        {
            InitializeComponent();
            _currentUser = user;
            _viewModel = new TracksViewModel(user);
            DataContext = _viewModel;
            
            _viewModel.TrackSelected += ViewModel_TrackSelected;
        }

        private void TrackItem_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is Border border && border.DataContext is TrackViewModel track)
            {
                if (_viewModel.IsGuest)
                {
                    MessageBox.Show("Для просмотра деталей трека необходимо войти в аккаунт.", 
                        "Гостевой режим", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    _viewModel.ViewTrackDetailsCommand.Execute(track);
                }
            }
        }

        private void ViewModel_TrackSelected(object? sender, TrackViewModel track)
        {

            if (track.AlbumID.HasValue)
            {
                using (var context = _viewModel.GetContext())
                {
                    var album = context.Albums
                        .Include(a => a.Artist)
                        .FirstOrDefault(a => a.AlbumID == track.AlbumID.Value);
                    
                    if (album != null)
                    {
                        var albumDetailView = new AlbumDetailView(album, _currentUser);
                        albumDetailView.ShowDialog();
                    }
                }
            }
        }

        private void AddTrack_Click(object sender, RoutedEventArgs e)
        {
            if (_currentUser.Role != "Admin")
            {
                MessageBox.Show("Только администратор может добавлять треки.", 
                    "Доступ запрещен", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var addTrackView = new AddEditTrackView();
            if (addTrackView.ShowDialog() == true)
            {
                _viewModel.LoadTracksCommand.Execute(null);
            }
        }

        private void EditTrack_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is TrackViewModel track)
            {
                if (_currentUser.Role != "Admin")
                {
                    MessageBox.Show("Только администратор может редактировать треки.", 
                        "Доступ запрещен", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                using (var context = _viewModel.GetContext())
                {
                    var trackToEdit = context.Tracks
                        .Include(t => t.TrackArtists)
                        .Include(t => t.Album)
                        .FirstOrDefault(t => t.TrackID == track.TrackID);
                    
                    if (trackToEdit != null)
                    {
                        var editTrackView = new AddEditTrackView(trackToEdit);
                        if (editTrackView.ShowDialog() == true)
                        {
                            _viewModel.LoadTracksCommand.Execute(null);
                        }
                    }
                }
            }
        }
    }
}
