using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
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
using System.Windows.Shapes;
using spotify.Context;
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace spotify.Windows
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window, INotifyPropertyChanged
    {
        public LoginWindow()
        {
            InitializeComponent();
            DataContext = this;
            GenerateCaptha();

            var img = new Image {
                Source = new BitmapImage(new Uri("pack://siteoforigin:,,,/Resources/restart.png")),
                Width = 40, Height = 40
            };
            NewCapthaButton.Content = img;

            var img1 = new Image
            {
                Source = new BitmapImage(new Uri("pack://siteoforigin:,,,/Resources/cross.png")),
                Width = 40,
                Height = 40
            };
            CloseButton.Content = img1;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string _login;
        public string Login
        {
            get => _login;
            set
            {
                _login = value;
                OnPropertyChanged(nameof(Login));
            }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        private string _capthaText;
        public string CapthaText
        {
            get => _capthaText;
            set
            {
                _capthaText = value;
                OnPropertyChanged();
            }
        }

        private ImageSource _captchaImage;
        public ImageSource CaptchaImage
        {
            get => _captchaImage;
            set
            {
                _captchaImage = value;
                OnPropertyChanged(nameof(CaptchaImage));
            }
        }

        private string _capthaInput;
        public string CapthaInput
        {
            get => _capthaInput;
            set
            {
                _capthaInput = value;
                OnPropertyChanged(nameof(CapthaInput));
            }
        }

        private void GenerateCaptha()
        {
            CapthaText = GenerateRandomString(6);
            CaptchaImage = GenerateCaptchaImage(CapthaText);
        }

        private string GenerateRandomString(int lenth)
        {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            string result = "";
            for (int i = 0; i < lenth; i++)
            {
                result += chars[RandomNumberGenerator.GetInt32(chars.Length)];
            }
            return result;
        }

        public ImageSource GenerateCaptchaImage(string text)
        {
            int width = 300;
            int height = 40;
            var random = new Random();
            var image = new DrawingVisual();

            using (var dc = image.RenderOpen())
            {
                dc.DrawRectangle(Brushes.White, null, new Rect(0, 0, width, height));
                var formatted = new FormattedText(text, CultureInfo.InvariantCulture, FlowDirection.LeftToRight, new Typeface("TimesNewRoman"), 36, Brushes.Black);
                dc.DrawText(formatted, new Point(width / 2 - (formatted.Width / 2), height / 2 - (formatted.Height / 2)));

                for(int i = 0; i < RandomNumberGenerator.GetInt32(10); i++)
                {
                    var pen = new Pen(new SolidColorBrush(Color.FromRgb((byte)RandomNumberGenerator.GetInt32(255),
                                                                        (byte)RandomNumberGenerator.GetInt32(255),
                                                                        (byte)RandomNumberGenerator.GetInt32(255))), 1);

                    dc.DrawLine(pen, new Point(RandomNumberGenerator.GetInt32(width), RandomNumberGenerator.GetInt32(height)),
                                     new Point(RandomNumberGenerator.GetInt32(width), RandomNumberGenerator.GetInt32(height)));
                }

                /*double x = 10;

                foreach (char c in text)
                {
                    var fontSize = random.Next(20, 40);
                    var typeface = new Typeface("TimesNewRoman");
                    var formatted = new FormattedText(text, CultureInfo.InvariantCulture, FlowDirection.LeftToRight, typeface, fontSize, Brushes.Black);
                    var g = new GeometryDrawing(Brushes.Black, null, formatted.BuildGeometry(new Point(0, 0)));
                    var transformGroup = new TransformGroup();
                    transformGroup.Children.Add(new RotateTransform(random.Next(-30, 30)));
                    transformGroup.Children.Add(new SkewTransform(random.NextDouble() * 19 - 2, random.NextDouble() * 19 - 2));
                    transformGroup.Children.Add(new TranslateTransform(x, (height - fontSize) / 2 + random.NextDouble() * 10 - 5));
                    dc.PushTransform(transformGroup);
                    dc.DrawGeometry(Brushes.Black, null, formatted.BuildGeometry(new Point(0, 0)));
                    dc.Pop();
                    x += formatted.Width + random.Next(2, 10);
                }*/

                for (int i = 0; i < 2500; i++)
                {
                    var px = RandomNumberGenerator.GetInt32(width);
                    var py = RandomNumberGenerator.GetInt32(height);
                    dc.DrawEllipse(Brushes.Gray, null, new Point(px, py), 1, 1);
                }
            }
            var bmp = new RenderTargetBitmap(width, height, 100, 100, PixelFormats.Pbgra32);
            bmp.Render(image);
            return bmp;
        }

        private void GuestButton_Click(object sender, RoutedEventArgs e)
        {
            GuestWindow guest = new GuestWindow();
            guest.Show();
            this.Close();
        }

        private void EnterButton_Click(object sender, RoutedEventArgs e)
        {
            var context = new SpotifyContext();
            var user = context.Users.FirstOrDefault(u => u.Login == Login && u.Password == Password);

            if (!string.Equals(_capthaInput, _capthaText))
            {
                MessageBox.Show("Капча введена неправильно!");
                GenerateCaptha();
                return;
            }

            if (user == null || LoginTextBox.Text != user.Login || PasswordTextBox.Text != user.Password)
            {
                MessageBox.Show("Пользователь введен неправильно!");
                GenerateCaptha();
                return;
            }
            Application.Current.Properties["CurrentUser"] = user;

            MainMenuWindow menu = new MainMenuWindow();
            menu.Show();
            this.Close();
        }

        private void NewCapthaButton_Click(object sender, RoutedEventArgs e)
        {
            GenerateCaptha();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
