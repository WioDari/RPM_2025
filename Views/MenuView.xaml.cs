using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.IO;
using MusicPlus.Models;
using MusicPlus.ViewModels;
using MusicPlus.Views;

namespace MusicPlus.Views
{
    public partial class MenuView : Window
    {
        private readonly MenuViewModel _viewModel;
        private User _currentUser;

        public MenuView(User user)
        {
            InitializeComponent();
            _currentUser = user;
            _viewModel = new MenuViewModel(user);
            DataContext = _viewModel;

            _viewModel.NavigationRequested += ViewModel_NavigationRequested;
            _viewModel.LogoutRequested += ViewModel_LogoutRequested;
            _viewModel.SessionExpiredRequested += ViewModel_SessionExpiredRequested;
            _viewModel.GuestLoginRequested += ViewModel_GuestLoginRequested;

            this.MouseMove += (s, e) => OnUserActivity();
            this.KeyDown += (s, e) => OnUserActivity();

            NavigateToAlbums();
        }

        private void OnUserActivity()
        {

        }

        private void ViewModel_NavigationRequested(object? sender, string view)
        {
            switch (view)
            {
                case "Albums":
                    NavigateToAlbums();
                    break;
                case "Artists":
                    NavigateToArtists();
                    break;
                case "Playlists":
                    NavigateToPlaylists();
                    break;
                case "Users":
                    NavigateToUsers();
                    break;
            }
        }

        private void ViewModel_LogoutRequested(object? sender, System.EventArgs e)
        {
            var authView = new AuthView();
            authView.Show();
            this.Close();
        }

        private void ViewModel_SessionExpiredRequested(object? sender, System.EventArgs e)
        {
            var authView = new AuthView();
            authView.Show();
            this.Close();
        }

        private void ViewModel_GuestLoginRequested(object? sender, System.EventArgs e)
        {
            var authView = new AuthView();
            authView.Show();
            this.Close();
        }

        private void NavigateToAlbums()
        {
            var albumsView = new AlbumsView(_currentUser);
            ContentArea.Content = albumsView;
        }

        private void NavigateToArtists()
        {
            var artistsView = new ArtistsView(_currentUser);
            ContentArea.Content = artistsView;
        }

        private void NavigateToPlaylists()
        {
            var playlistsView = new PlaylistsView(_currentUser);
            ContentArea.Content = playlistsView;
        }

        private void NavigateToUsers()
        {
            var usersView = new UsersView();
            ContentArea.Content = usersView;
        }

        private void LogoImage_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is Image image)
            {
                try
                {

                    var exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                    var exeDir = Path.GetDirectoryName(exePath);
                    var logoPath = Path.Combine(exeDir ?? "", "Resources", "icon.png");
                    
                    if (File.Exists(logoPath))
                    {
                        var bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.UriSource = new Uri(logoPath, UriKind.Absolute);
                        bitmap.CacheOption = BitmapCacheOption.OnLoad;
                        bitmap.EndInit();
                        bitmap.Freeze();
                        image.Source = bitmap;
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Ошибка загрузки логотипа: {ex.Message}");
                }
            }
        }
    }
}
