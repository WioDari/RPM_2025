using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.IO;
using MusicPlus.ViewModels;
using MusicPlus.Models;

namespace MusicPlus.Views
{
    public partial class AuthView : Window
    {
        private readonly AuthViewModel _viewModel;

        public AuthView()
        {
            InitializeComponent();
            _viewModel = new AuthViewModel();
            DataContext = _viewModel;

            _viewModel.LoginSuccessful += ViewModel_LoginSuccessful;
            _viewModel.GuestModeRequested += ViewModel_GuestModeRequested;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox && _viewModel != null)
            {
                _viewModel.Password = passwordBox.Password;
            }
        }

        private void ViewModel_LoginSuccessful(object? sender, User user)
        {

            var menuView = new MenuView(user);
            menuView.Show();
            this.Close();
        }

        private void ViewModel_GuestModeRequested(object? sender, System.EventArgs e)
        {

            var guestUser = new User
            {
                UserID = 0,
                Role = "Guest",
                FullName = "Гость"
            };
            var menuView = new MenuView(guestUser);
            menuView.Show();
            this.Close();
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
