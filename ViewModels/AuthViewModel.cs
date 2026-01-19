using MusicPlus.Models;
using MusicPlus.Services;
using MusicPlus.Data;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Input;

namespace MusicPlus.ViewModels
{
    public class AuthViewModel : BaseViewModel
    {
        private readonly AuthService _authService;
        private string _login = string.Empty;
        private string _password = string.Empty;
        private string _captchaInput = string.Empty;
        private string _captchaAnswer = string.Empty;
        private ImageSource? _captchaImage;
        private string _errorMessage = string.Empty;
        private bool _isGuestMode = false;

        public AuthViewModel()
        {
            try
            {
                var connectionString = "Server=localhost;Database=musicplus_db;User=root;Password=12345;Port=3306;CharSet=utf8mb4;";
                var serverVersion = ServerVersion.Create(new Version(8, 0, 21), ServerType.MySql);
                var options = new Microsoft.EntityFrameworkCore.DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseMySql(connectionString, serverVersion)
                    .Options;
                _authService = new AuthService(new ApplicationDbContext(options));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка подключения к базе данных: {ex.Message}\n\nПроверьте, что MySQL запущен и база данных создана.", 
                    "Ошибка подключения", MessageBoxButton.OK, MessageBoxImage.Error);
                _authService = null!;
            }
            
            GenerateCaptcha();
            LoginCommand = new RelayCommand(OnLogin, CanLogin);
            GuestModeCommand = new RelayCommand(OnGuestMode);
            RefreshCaptchaCommand = new RelayCommand(OnRefreshCaptcha);
        }

        public string Login
        {
            get => _login;
            set
            {
                _login = value;
                OnPropertyChanged();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public string CaptchaInput
        {
            get => _captchaInput;
            set
            {
                _captchaInput = value;
                OnPropertyChanged();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public ImageSource? CaptchaImage
        {
            get => _captchaImage;
            set
            {
                _captchaImage = value;
                OnPropertyChanged();
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged();
            }
        }

        public bool IsGuestMode
        {
            get => _isGuestMode;
            set
            {
                _isGuestMode = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoginCommand { get; }
        public ICommand GuestModeCommand { get; }
        public ICommand RefreshCaptchaCommand { get; }

        public event EventHandler<User>? LoginSuccessful;
        public event EventHandler? GuestModeRequested;

        private bool CanLogin(object? parameter)
        {
            return !string.IsNullOrWhiteSpace(Login) && 
                   !string.IsNullOrWhiteSpace(Password) && 
                   !string.IsNullOrWhiteSpace(CaptchaInput);
        }

        private void OnLogin(object? parameter)
        {
            ErrorMessage = string.Empty;

            if (_authService == null)
            {
                MessageBox.Show("Сервис авторизации недоступен. Проверьте подключение к базе данных.", 
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                var result = _authService.Authenticate(Login, Password, CaptchaInput, _captchaAnswer);

                if (result.Success && result.User != null)
                {
                    LoginSuccessful?.Invoke(this, result.User);
                }
                else
                {
                    ErrorMessage = result.Message;
                    if (result.Warning)
                    {
                        MessageBox.Show(result.Message, "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    else
                    {
                        MessageBox.Show(result.Message, "Ошибка авторизации", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    GenerateCaptcha();
                    CaptchaInput = string.Empty;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Ошибка при авторизации: {ex.Message}";
                MessageBox.Show($"Ошибка при авторизации: {ex.Message}", 
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                GenerateCaptcha();
                CaptchaInput = string.Empty;
            }
        }

        private void OnGuestMode(object? parameter)
        {
            GuestModeRequested?.Invoke(this, EventArgs.Empty);
        }

        private void OnRefreshCaptcha(object? parameter)
        {
            GenerateCaptcha();
            CaptchaInput = string.Empty;
        }

        private void GenerateCaptcha()
        {

            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            _captchaAnswer = new string(Enumerable.Range(0, 5)
                .Select(_ => chars[random.Next(chars.Length)])
                .ToArray());

            var bitmap = new RenderTargetBitmap(200, 80, 96, 96, PixelFormats.Pbgra32);
            var visual = new DrawingVisual();
            using (var dc = visual.RenderOpen())
            {

                dc.DrawRectangle(Brushes.White, null, new Rect(0, 0, 200, 80));

                for (int i = 0; i < 100; i++)
                {
                    var x = random.Next(200);
                    var y = random.Next(80);
                    dc.DrawEllipse(Brushes.LightGray, null, new System.Windows.Point(x, y), 1, 1);
                }

                for (int i = 0; i < _captchaAnswer.Length; i++)
                {
                    var offsetX = 20 + i * 35 + random.Next(-5, 5);
                    var offsetY = 20 + random.Next(-5, 5);
                    var charText = new FormattedText(
                        _captchaAnswer[i].ToString(),
                        System.Globalization.CultureInfo.CurrentCulture,
                        FlowDirection.LeftToRight,
                        new Typeface("Arial"),
                        36,
                        Brushes.Black,
                        96);
                    dc.DrawText(charText, new System.Windows.Point(offsetX, offsetY));
                }
            }
            bitmap.Render(visual);
            CaptchaImage = bitmap;
        }
    }
}
