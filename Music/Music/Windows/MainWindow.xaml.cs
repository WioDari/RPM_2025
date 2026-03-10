using Microsoft.EntityFrameworkCore;
using Music.Context;
using Music.Models;
using Music.Properties;
using Music.Resources;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Music.Windows;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
/// 
public partial class MainWindow : Window
{
    private string captchaText = "";

    public MainWindow()
    {
        InitializeComponent();
        Load();
        GenerateCaptcha();
    }

    private async void Load()
    {
        await using MusicContext context = new();

        if (!string.IsNullOrEmpty(Settings.Default.UserId.ToString()) && Settings.Default.UserId != 0)
        {
            var user = context.Users.Include(x => x.Role).First(x => x.Id == Settings.Default.UserId);
            if (user.BanDateTime + user.BanInterval - DateTime.Now > TimeSpan.Zero)
                return;
            new MenuWindow(context.Users.Include(x => x.Role).First(x => x.Id == Settings.Default.UserId)).Show();
            Close();
            return;
        }

        var u = await context.Users.FirstAsync(x => x.Id == 1);

        LoginTextBox.Text = u.Login;
        PassTextBox.Text = u.Password;
        PasswordBox.Password = u.Password;
    }

    [Obsolete]
    private void GenerateCaptcha()
    {
        captchaText = GenerateRandomString(6);
        CaptchaInputTextBox.Text = captchaText;
        CaptchaImage.Source = GenerateCaptchaImage(captchaText);
    }

    private static string GenerateRandomString(int length)
    {
        string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        string result = "";
        for (int i = 0; i < length; i++)
        {
            result += chars[RandomNumberGenerator.GetInt32(chars.Length)];
        }
        return result;
    }

    private async void LoginButton_Click(object sender, RoutedEventArgs e)
    {
        var unbanTime = (Settings.Default.LastEntryDateTime + Settings.Default.BanInterval) - DateTime.Now;

        if (unbanTime > TimeSpan.Zero)
        {
            MessageBox.Show($"Доступ для вашего устройства временно ограничен из-за большого количества неверных попыток входа.\nПовторите попытку через {unbanTime:mm\\:ss}", "Ошибка входа", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        if (!string.Equals(CaptchaInputTextBox.Text, captchaText))
        {
            MessageBox.Show($"Неверно введена каптча.\n{FailedEntryAttempt(null)}", "Ошибка входа", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        await using MusicContext context = new();

        User? user = await context.Users.Include(x => x.Role).FirstOrDefaultAsync(u => u.Login == LoginTextBox.Text);
        if (user != null)
        {
            if (PasswordBox.Password != user.Password)
            {
                MessageBox.Show($"Неверный логин или пароль{FailedEntryAttempt(user)}", "Ошибка входа", MessageBoxButton.OK, MessageBoxImage.Error);
                context.Users.Update(user);
                await context.SaveChangesAsync();
                return;
            }
            var banTimeSpan = user.BanDateTime + user.BanInterval - DateTime.Now;

            if (banTimeSpan < TimeSpan.Zero)
            {
                Settings.Default.UserId = user.Id;
                Settings.Default.FailedEntriesCount = 0;
                Settings.Default.BanInterval = TimeSpan.Zero;
                Settings.Default.LastEntryDateTime = DateTime.Now;
                Settings.Default.Save();

                user.LastLoginDateTime = DateTime.Now;
                context.Users.Update(user);
                await context.SaveChangesAsync();

                MenuWindow menuWindow = new(user);
                menuWindow.Show();
                Close();
            }
            else
            {
                MessageBox.Show($"Ваш аккаунт временно заблокирован.\nПовторите попытку через {banTimeSpan:mm\\:ss}", "Ошибка входа", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

        }
        else
        {
            MessageBox.Show($"Неверный логин или пароль\n{FailedEntryAttempt(null)}", "Ошибка входа", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private string FailedEntryAttempt(User? user)
    {
        int maxFailedAttemps = 3;

        /*LoginTextBox.Text = "";
        PasswordBox.Password = "";*/
        GenerateCaptcha();

        Settings.Default.FailedEntriesCount += 1;
        Settings.Default.Save();

        if (Settings.Default.FailedEntriesCount < maxFailedAttemps)
        {
            return $"\nОсталось попыток: {maxFailedAttemps - Settings.Default.FailedEntriesCount}";
        }

        if (Settings.Default.FailedEntriesCount == maxFailedAttemps)
        {
            if (user != null)
            {
                user.BanInterval = TimeSpan.FromHours(1);
                user.BanDateTime = DateTime.Now;
                return $"\nВаш аккаунт временно заблокирован.\nПовторите попытку через {(user.BanDateTime + user.BanInterval - DateTime.Now):mm\\:ss}";
            }
            else
            {
                Settings.Default.BanInterval = TimeSpan.FromHours(1);
                Settings.Default.LastEntryDateTime = DateTime.Now;
                Settings.Default.Save();
                return $"\nДоступ для вашего устройства временно ограничен из-за большого количества неверных попыток входа.\nПовторите попытку через {(Settings.Default.LastEntryDateTime + Settings.Default.BanInterval - DateTime.Now):mm\\:ss}";
            }

        }

        return "";
    }

    private void GuestLoginButton_Click(object sender, RoutedEventArgs e)
    {
        MenuWindow menuWindow = new();
        menuWindow.Show();
        Close();
    }

    [Obsolete]
    private RenderTargetBitmap GenerateCaptchaImage(string text)
    {
        int width = 215;
        int height = 65;
        var image = new DrawingVisual();
        List<string> fonts = ["Calibri", "Times New Roman", "Arial", "Segoe UI", "Verdana", "Georgia", "Courier New", "Impact", "Comic Sans MS"];

        var rnd = new Random();

        using (var dc = image.RenderOpen())
        {
            double x = 30;

            foreach (char c in text)
            {
                var fontSize = rnd.Next(38, 42);
                var typeFace = new Typeface(fonts[rnd.Next(0, fonts.Count - 1)]);

                var formatted = new FormattedText(
                                        c.ToString(),
                                        CultureInfo.InvariantCulture,
                                        FlowDirection.LeftToRight,
                                        typeFace,
                                        fontSize,
                                        Brushes.Black
                                    );

                var g = new GeometryDrawing(Brushes.Black, null, formatted.BuildGeometry(new Point(0, 0)));

                var transformGroup = new TransformGroup();
                transformGroup.Children.Add(new RotateTransform(rnd.NextDouble() + 13 - 9));
                transformGroup.Children.Add(new SkewTransform(rnd.NextDouble() + 19 - 2, rnd.NextDouble() + 19 - 2));
                transformGroup.Children.Add(new TranslateTransform(x, (height - fontSize) / 2 - rnd.NextDouble() + 19 - 2));
                dc.PushTransform(transformGroup);
                dc.DrawGeometry(Brushes.Black, null, formatted.BuildGeometry(new Point(0 - formatted.Width / 2, 0 - formatted.Height / 2)));
                dc.Pop();
                x += formatted.Width + rnd.Next(2, 10);
            }

            for (int i = 0; i < RandomNumberGenerator.GetInt32(3, 10); i++)
            {
                var pen = new Pen(new SolidColorBrush(Color.FromRgb(
                    (byte)RandomNumberGenerator.GetInt32(256),
                    (byte)RandomNumberGenerator.GetInt32(256),
                    (byte)RandomNumberGenerator.GetInt32(256))),
                3);
                dc.DrawLine(pen,
                    new Point(RandomNumberGenerator.GetInt32(width), RandomNumberGenerator.GetInt32(height)),
                    new Point(RandomNumberGenerator.GetInt32(width), RandomNumberGenerator.GetInt32(height)));
            }

            for (int i = 0; i < 2000; i++)
            {
                var px = RandomNumberGenerator.GetInt32(width);
                var py = RandomNumberGenerator.GetInt32(height);
                dc.DrawEllipse(Brushes.Gray, null, new Point(px, py), 1.0, 1.0);
            }

        }

        var bmp = new RenderTargetBitmap(width * 2, height * 2, 200, 200, PixelFormats.Pbgra32);
        bmp.Render(image);

        return bmp;
    }

    private void ExitButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void RefreshButton_Click(object sender, RoutedEventArgs e)
    {
        GenerateCaptcha();
    }


    private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
    {
        if (PasswordBox.Visibility == Visibility.Visible)
            PassTextBox.Text = PasswordBox.Password;
    }

    private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (PassTextBox.Visibility == Visibility.Visible)
            PasswordBox.Password = PassTextBox.Text;
    }

    private void ToggleButton_Checked(object sender, RoutedEventArgs e)
    {
        PassTextBox.Text = PasswordBox.Password;
        PasswordBox.Visibility = Visibility.Collapsed;
        PassTextBox.Visibility = Visibility.Visible;
        ShowPasswordImage.Source = new BitmapImage(new Uri("pack://application:,,,/Music;component/Resources/show.png"));
    }

    private void ToggleButton_Unchecked(object sender, RoutedEventArgs e)
    {
        PasswordBox.Password = PassTextBox.Text;
        PassTextBox.Visibility = Visibility.Collapsed;
        PasswordBox.Visibility = Visibility.Visible;
        ShowPasswordImage.Source = new BitmapImage(new Uri("pack://application:,,,/Music;component/Resources/hide.png"));
    }

}