using System;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using MusicPlus.Models;
using MusicPlus.ViewModels;
using System.Collections.ObjectModel;

namespace MusicPlus.Views
{
    public partial class AddEditAlbumView : Window
    {
        private readonly AddEditAlbumViewModel _viewModel;

        public AddEditAlbumView(Album? album = null)
        {
            InitializeComponent();
            _viewModel = new AddEditAlbumViewModel(album);
            DataContext = _viewModel;
            
            _viewModel.CloseRequested += ViewModel_CloseRequested;

            Loaded += AddEditAlbumView_Loaded;

            _viewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void ViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

            if (e.PropertyName == "SelectedGenres" || e.PropertyName == "AllGenres")
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    SetSelectedGenresInListBox();
                }), DispatcherPriority.Loaded);
            }
        }

        private void SetSelectedGenresInListBox()
        {
            var genresListBox = FindName("GenresListBox") as System.Windows.Controls.ListBox;
            if (genresListBox != null && _viewModel != null && _viewModel.AllGenres.Count > 0)
            {

                genresListBox.SelectedItems.Clear();

                foreach (var genre in _viewModel.AllGenres)
                {
                    if (_viewModel.SelectedGenres.Any(sg => sg.GenreID == genre.GenreID))
                    {
                        genresListBox.SelectedItems.Add(genre);
                    }
                }
            }
        }

        private void AddEditAlbumView_Loaded(object sender, RoutedEventArgs e)
        {


            Dispatcher.BeginInvoke(new Action(() =>
            {
                SetSelectedGenresInListBox();
            }), DispatcherPriority.Loaded);
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

        private void GenresListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (sender is System.Windows.Controls.ListBox listBox && _viewModel != null)
            {
                _viewModel.SelectedGenres.Clear();
                foreach (Genre genre in listBox.SelectedItems)
                {
                    _viewModel.SelectedGenres.Add(genre);
                }
            }
        }
    }
}
