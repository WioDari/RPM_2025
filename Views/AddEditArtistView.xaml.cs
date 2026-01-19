using System.Windows;
using MusicPlus.Models;
using MusicPlus.ViewModels;

namespace MusicPlus.Views
{
    public partial class AddEditArtistView : Window
    {
        private readonly AddEditArtistViewModel _viewModel;

        public AddEditArtistView(Artist? artist = null)
        {
            InitializeComponent();
            _viewModel = new AddEditArtistViewModel(artist);
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
    }
}
