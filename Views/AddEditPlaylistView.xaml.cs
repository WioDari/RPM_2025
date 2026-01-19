using System.Windows;
using MusicPlus.Models;
using MusicPlus.ViewModels;

namespace MusicPlus.Views
{
    public partial class AddEditPlaylistView : Window
    {
        private readonly AddEditPlaylistViewModel _viewModel;

        public AddEditPlaylistView(User currentUser, Playlist? playlist = null)
        {
            InitializeComponent();
            _viewModel = new AddEditPlaylistViewModel(currentUser, playlist);
            DataContext = _viewModel;
            
            _viewModel.CloseRequested += ViewModel_CloseRequested;
        }

        private void ViewModel_CloseRequested(object? sender, bool saved)
        {
            if (saved)
            {
                DialogResult = true;
            }
            else
            {
                DialogResult = false;
            }
            Close();
        }

        private void TagsListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (sender is System.Windows.Controls.ListBox listBox && _viewModel != null)
            {
                _viewModel.SelectedTags.Clear();
                foreach (string tag in listBox.SelectedItems)
                {
                    _viewModel.SelectedTags.Add(tag);
                }
            }
        }

        private void CustomTagsTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }
    }
}
