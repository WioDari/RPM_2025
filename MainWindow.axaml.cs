using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Spotify.Context;
using Spotify.Models;
using System;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Timers;
using Avalonia.Threading;

namespace Spotify;

public partial class MainWindow : Window
{
    private string _correctCaptcha = "";
    private int _failedLoginAttempts = 0;
    private DateTime _blockUntil = DateTime.MinValue;
    
    private readonly int _maxAttempts = 3; 
    private readonly TimeSpan _blockDuration = TimeSpan.FromMinutes(1); 
    private DispatcherTimer? _blockTimer;
    
    public MainWindow()
    {
        InitializeComponent();
        GenerateNewCaptcha();
        UpdateLoginState();

        RefreshBtn.Click += RefreshBtn_Click;
        CheckCaptcha.Click += CheckCaptcha_Click;
        LoginBtn.Click += LoginBtn_Click;
    }

    
 
    private void Show_Click(object sender, RoutedEventArgs e)
    {
        Password.RevealPassword = !Password.RevealPassword;
    }
    
    private void RefreshBtn_Click(object? sender, RoutedEventArgs e)
    {
        GenerateNewCaptcha();
        ErrorLabel.Text = "";
    }
    
    private void CheckCaptcha_Click(object? sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(CaptchaInput.Text))
        {
            ErrorLabel.Text = "Введите капчу";
            ErrorLabel.Foreground = Brushes.Red;
            return;
        }
    
        if (string.Equals(CaptchaInput.Text, _correctCaptcha, StringComparison.OrdinalIgnoreCase))
        {
            ErrorLabel.Text = "Капча верна!";
            ErrorLabel.Foreground = Brushes.Green;
        }
        else
        {
            ErrorLabel.Text = "Неверная капча!";
            ErrorLabel.Foreground = Brushes.Red;
            CaptchaInput.Text = "";
        }
    }
    
    private void LoginBtn_Click(object? sender, RoutedEventArgs e)
    {
        if (DateTime.Now < _blockUntil)
        {
            TimeSpan remaining = _blockUntil - DateTime.Now;
            ErrorLabel.Text = $"Аккаунт заблокирован. Разблокировка через: {remaining:mm\\:ss}";
            ErrorLabel.Foreground = Brushes.DarkRed;
            return;
        }
    
        // Проверка капчи обязательна
        if (!string.Equals(CaptchaInput.Text, _correctCaptcha, StringComparison.OrdinalIgnoreCase))
        {
            ErrorLabel.Text = "Сначала правильно введите капчу!";
            ErrorLabel.Foreground = Brushes.Red;
            CaptchaInput.Text = "";
            _failedLoginAttempts++; 
            
            if (_failedLoginAttempts >= _maxAttempts)
            {
                BlockAccount();
            }
            else
            {
                ErrorLabel.Text += $" (Попытка {_failedLoginAttempts} из {_maxAttempts})";
            }
            return;
        }
    
        string login = Email.Text?.Trim() ?? "";
        string pass = Password.Text ?? "";
    
        if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(pass))
        {
            ErrorLabel.Text = "Заполните логин и пароль";
            ErrorLabel.Foreground = Brushes.OrangeRed;
            return;
        }
    
        try
        {
            using var db = new PostgresContext();
            var user = db.Users.FirstOrDefault(u => u.UserLogin == login && u.UserPassword == pass);
            
            if (user != null)
            {
                _failedLoginAttempts = 0;
                _blockUntil = DateTime.MinValue;
                StopBlockTimer();

                ErrorLabel.Text = "Вход выполнен успешно!";
                ErrorLabel.Foreground = Brushes.Green;
                
                new UserMenu(user).Show();
            }
            else
            {
                _failedLoginAttempts++;
                
                if (_failedLoginAttempts >= _maxAttempts)
                {
                    BlockAccount();
                }
                else
                {
                    int remainingAttempts = _maxAttempts - _failedLoginAttempts;
                    ErrorLabel.Text = $"Неверный логин или пароль. Осталось попыток: {remainingAttempts}";
                    ErrorLabel.Foreground = Brushes.Red;
                    
                    if (remainingAttempts == 1)
                    {
                        ErrorLabel.Text += "\nВнимание! Следующая ошибка приведет к блокировке!";
                    }
                }
                
                GenerateNewCaptcha();
            }
        }
        catch (Exception ex)
        {
            ErrorLabel.Text = $"Ошибка подключения к базе данных";
            ErrorLabel.Foreground = Brushes.Red;
            Console.WriteLine($"Database error: {ex.Message}");
        }
    }
    
    private void BlockAccount()
    {
        _blockUntil = DateTime.Now + _blockDuration;
        ErrorLabel.Text = $"Слишком много неудачных попыток! Аккаунт заблокирован на {_blockDuration.TotalMinutes} минут.";
        ErrorLabel.Foreground = Brushes.DarkRed;
        
        UpdateLoginState();
        StartBlockTimer();
    }
    
    private void UpdateLoginState()
    {
        bool isBlocked = DateTime.Now < _blockUntil;
        
        LoginBtn.IsEnabled = !isBlocked;
        Email.IsEnabled = !isBlocked;
        Password.IsEnabled = !isBlocked;
        CaptchaInput.IsEnabled = !isBlocked;
        RefreshBtn.IsEnabled = !isBlocked;
        CheckCaptcha.IsEnabled = !isBlocked;
        
        if (isBlocked)
        {
            LoginBtn.Classes.Add("disabled");
            LoginBtn.Opacity = 0.5;
        }
        else
        {
            LoginBtn.Classes.Remove("disabled");
            LoginBtn.Opacity = 1.0;
        }
    }
    
    private void StartBlockTimer()
    {
        StopBlockTimer();
        
        _blockTimer = new DispatcherTimer();
        _blockTimer.Interval = TimeSpan.FromSeconds(1);
        _blockTimer.Tick += (s, e) =>
        {
            if (DateTime.Now >= _blockUntil)
            {
                StopBlockTimer();
                _failedLoginAttempts = 0;
                _blockUntil = DateTime.MinValue;
                
                ErrorLabel.Text = "Блокировка снята. Можно попробовать снова.";
                ErrorLabel.Foreground = Brushes.Green;
                UpdateLoginState();
            }
            else
            {
                TimeSpan remaining = _blockUntil - DateTime.Now;
                ErrorLabel.Text = $"Аккаунт заблокирован. Осталось: {remaining:mm\\:ss}";
                UpdateLoginState();
            }
        };
        _blockTimer.Start();
    }
    
    private void StopBlockTimer()
    {
        if (_blockTimer != null)
        {
            _blockTimer.Stop();
            _blockTimer = null;
        }
    }
    
    private void GenerateNewCaptcha()
    {
        _correctCaptcha = RandomCode(5);
        CaptchaImage.Source = DrawCaptcha(_correctCaptcha);
        CaptchaInput.Text = "";
    }
    
    private string RandomCode(int length)
    {
        const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
        var result = new char[length];
        for (int i = 0; i < length; i++)
            result[i] = chars[RandomNumberGenerator.GetInt32(chars.Length)];
        return new string(result);
    }
    
    private IImage DrawCaptcha(string text)
    {
        const int width = 250;
        const int height = 70;
    
        var bitmap = new RenderTargetBitmap(new PixelSize(width, height));
        using var ctx = bitmap.CreateDrawingContext();
    
        ctx.FillRectangle(Brushes.White, new Rect(0, 0, width, height));
    
        var rnd = Random.Shared;
    
        for (int i = 0; i < 8; i++)
        {
            var pen = new Pen(Brushes.LightGray, rnd.Next(2, 5));
            ctx.DrawLine(pen,
                new Point(rnd.Next(width), rnd.Next(height)),
                new Point(rnd.Next(width), rnd.Next(height)));
        }
    
        double x = 20;
        foreach (char c in text)
        {
            var color = Color.FromRgb(
                (byte)rnd.Next(0, 120),
                (byte)rnd.Next(0, 120),
                (byte)rnd.Next(0, 120));
    
            double size = 38 + rnd.Next(-8, 9);
    
            var formattedText = new FormattedText(
                c.ToString(),
                CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface("Arial", weight: FontWeight.Bold),
                size,
                new SolidColorBrush(color));
    
            double y = 12 + rnd.Next(-10, 10);
            ctx.DrawText(formattedText, new Point(x, y));
    
            x += size + rnd.Next(6, 15);
        }
    
        return bitmap;
    }
}