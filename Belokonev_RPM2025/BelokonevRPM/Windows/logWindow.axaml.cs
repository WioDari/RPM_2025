using System;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using BelokonevPRM.Context;

namespace BelokonevPRM.Windows;

public partial class LogWindow : Window
{
    private Captcha _captcha = new Captcha();
    private string? _currCaptcha;
    
    public LogWindow()
    {
        InitializeComponent();
        GenerateNewCaptcha();
    }

    private void GenerateNewCaptcha()
    {
        var (text, image) = _captcha.GenerateCaptcha();
        _currCaptcha = text;
        CaptchaBox.Text = _currCaptcha;
        CaptchaImg.Source = image;
    }

    private void RefreshCaptchaButton_Click(object? sender, RoutedEventArgs e)
    {
        GenerateNewCaptcha();
    }

    private void ChangePasswordVisibility(object? sender, RoutedEventArgs e)
    {
        PasswordBox.RevealPassword = !PasswordBox.RevealPassword;
    }
    
    private void LoginOnClick(object? sender, RoutedEventArgs e)
    {
        var currLogin = LoginBox.Text ?? string.Empty;
        var currPassword = PasswordBox.Text ?? string.Empty;
        var currCaptcha = CaptchaBox.Text ?? string.Empty;

        using (var postgresContext = new PostgresContext())
        {
            var user = postgresContext.Users.FirstOrDefault(x => x.UserLogin == currLogin && x.UserPwd == currPassword && _currCaptcha == currCaptcha);
            if (user != null)
            {
                Console.WriteLine("User logged in");
                new MainWindow(user).Show();
            }
        }
        Close();
    }
}

