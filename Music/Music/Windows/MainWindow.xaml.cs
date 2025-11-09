using Microsoft.EntityFrameworkCore;
using Music.Context;
using Music.Models;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
        DataContext = await context.Users.FirstOrDefaultAsync(x => x.Id == 1);
    }

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
        if (!string.Equals(CaptchaInputTextBox.Text, captchaText))
        {
            MessageBox.Show("Каптча введена неверно", "Ошибка входа", MessageBoxButton.OK, MessageBoxImage.Error);
            GenerateCaptcha();
            return;
        }
        User currentUser = (DataContext as User)!;

        await using MusicContext context = new();

        User? user = await context.Users.FirstOrDefaultAsync(u => u.Login == currentUser.Login && u.Password == currentUser.Password);
        if (user != null)
        {
            MessageBox.Show("Вход выполнен успешно", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            
        }
        else
        {
            MessageBox.Show("Неверный логин или пароль", "Ошибка входа", MessageBoxButton.OK, MessageBoxImage.Error);
            (DataContext as User)!.Login = "";
            (DataContext as User)!.Password = "";
            GenerateCaptcha();
        }
    }

    [Obsolete]
    private ImageSource GenerateCaptchaImage(string text)
    {
        int width = 215;
        int height = 65;
        var image = new DrawingVisual();
        List<string> fonts = ["Calibri", "Times New Roman", "Arial", "Segoe UI", "Verdana", "Georgia", "Courier New", "Impact", "Comic Sans MS"];

        var rnd = new Random();

        using (var dc = image.RenderOpen())
        {
            double x = 30;

            //dc.DrawRectangle(Brushes.Red, null, new Rect(0, 0, width, height));

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

        var bmp = new RenderTargetBitmap(width*2, height*2, 200, 200, PixelFormats.Pbgra32);
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
}