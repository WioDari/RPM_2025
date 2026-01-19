using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.IO;
using Microsoft.EntityFrameworkCore;
using MusicPlus.Models;
using MusicPlus.ViewModels;

namespace MusicPlus.Views
{
    public partial class AlbumsView : UserControl
    {
        private readonly AlbumsViewModel _viewModel;
        private User _currentUser;

        public AlbumsView(User user)
        {
            InitializeComponent();
            _currentUser = user;
            _viewModel = new AlbumsViewModel(user);
            DataContext = _viewModel;
            
            _viewModel.AlbumSelected += ViewModel_AlbumSelected;
        }

        private void AddAlbum_Click(object sender, RoutedEventArgs e)
        {
            var addAlbumView = new AddEditAlbumView();
            if (addAlbumView.ShowDialog() == true)
            {

                _viewModel.LoadAlbumsCommand.Execute(null);
            }
        }

        private void AlbumTile_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border border && border.DataContext is Album album)
            {
                if (_viewModel.IsGuest)
                {
                    MessageBox.Show("Для просмотра деталей альбома необходимо войти в аккаунт.", 
                        "Гостевой режим", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    _viewModel.ViewAlbumDetailsCommand.Execute(album);
                }
            }
        }


        private void AlbumCoverImage_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is Image image && image.DataContext is Album album)
            {
                image.Source = LoadAlbumCover(album);
            }
        }

        private BitmapImage? LoadAlbumCover(Album album)
        {
            try
            {

                if (!string.IsNullOrEmpty(album.CoverPath))
                {

                    if (Uri.TryCreate(album.CoverPath, UriKind.Absolute, out var uri) && 
                        (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps))
                    {

                    }
                    else
                    {

                        var localPath = Path.Combine("Resources", "covers", Path.GetFileName(album.CoverPath));
                        if (File.Exists(localPath))
                        {
                            return LoadImageFromFile(localPath);
                        }

                        var altPath = Path.Combine("data", "covers", Path.GetFileName(album.CoverPath));
                        if (File.Exists(altPath))
                        {
                            return LoadImageFromFile(altPath);
                        }
                    }
                }

                if (album.CoverBinary != null && album.CoverBinary.Length > 0)
                {
                    return LoadImageFromBytes(album.CoverBinary);
                }

                var placeholderPaths = new[]
                {
                    "Resources/placeholder_cover.png",
                    "data/placeholder_cover.png",
                    Path.Combine("Resources", "placeholder_cover.png"),
                    Path.Combine("data", "placeholder_cover.png")
                };

                foreach (var placeholderPath in placeholderPaths)
                {
                    if (File.Exists(placeholderPath))
                    {
                        return LoadImageFromFile(placeholderPath);
                    }
                }
            }
            catch (Exception ex)
            {

                System.Diagnostics.Debug.WriteLine($"Ошибка загрузки обложки: {ex.Message}");
            }

            return null;
        }

        private BitmapImage LoadImageFromFile(string path)
        {
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(Path.GetFullPath(path), UriKind.Absolute);
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();
            bitmap.Freeze();
            return bitmap;
        }

        private BitmapImage LoadImageFromBytes(byte[] bytes)
        {
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.StreamSource = new MemoryStream(bytes);
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();
            bitmap.Freeze();
            return bitmap;
        }

        private void ViewModel_AlbumSelected(object? sender, Album album)
        {

            try
            {
                using (var context = _viewModel.GetContext())
                {
                    var fullAlbum = context.Albums
                        .Include(a => a.Artist)
                        .Include(a => a.AlbumGenres)
                            .ThenInclude(ag => ag.Genre)
                        .Include(a => a.Tracks)
                            .ThenInclude(t => t.TrackArtists)
                                .ThenInclude(ta => ta.Artist)
                        .FirstOrDefault(a => a.AlbumID == album.AlbumID);

                    if (fullAlbum != null)
                    {
                        var detailView = new AlbumDetailView(fullAlbum, _currentUser);
                        if (detailView.ShowDialog() == true)
                        {

                            _viewModel.LoadAlbumsCommand.Execute(null);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка открытия альбома: {ex.Message}", "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
