using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Spotify_wpf.Context;
using Spotify_wpf.ViewModels;
using Spotify_wpf.Views;
using Spotify_wpf.Properties;
using System.Net.WebSockets;

namespace Spotify_wpf
{
    class AuthViewModel : BaseViewModel
    {

        private bool _isChecked = false;

        public bool isChecked
        {
            get => _isChecked;
            set
            {
                _isChecked = value;
                Application.Current.Properties["isChecked"] = value;
                OnPropertyChanged();
               
            }
        }
        private ImageSource _captchaImage;

        public ImageSource captchaImage
        {
            get => _captchaImage;
            set
            {
                _captchaImage = value;
                OnPropertyChanged();
            }
        }
        private string _captchaInput;
        public string captchaInput
        {
            get => _captchaInput;
            set
            {
                _captchaInput = value;
            }
        }

        private string _captchaText;

        public string captchaText
        {
            get => _captchaText;
            set
            {
                _captchaText = value;
                OnPropertyChanged();
            }
        }

        private string _login = "apollinariya.allenova";

        public string login
        {
            get => _login;
            set
            {
                _login = value;
            }
        }
        private string _password = "sXFdIl0y";
        public string password
        {
            get => _password;
            set
            {
                _password = value;
            }
        }

        public ICommand AuthCommand { get; }

        public ICommand UpdateCaptchaCommand { get; }

        public AuthViewModel()
        {
            GenerateCaptcha();
            AuthCommand = new RelayCommand(OnAuth);
            UpdateCaptchaCommand = new RelayCommand(GenerateCaptcha);
            if (Settings.Default.userid != 0)
            {
                int userId = Settings.Default.userid;
                var context = new MusicContext();
                var user = context.Users.FirstOrDefault(u => u.UserId == userId);
                Application.Current.Properties["CurrentUser"] = user;
                Window menu = new Views.Menu();
                menu.Show();
                foreach (Window w in Application.Current.Windows)
                {
                    if (w is not Views.Menu)
                    {
                        w.Close();
                    }
                }

            }
        }
        public void OnAuth()
        {
            var date = DateTime.Now.Subtract(Settings.Default.ban_time);
            if (date != null) //MessageBox.Show(date.TotalHours.ToString());

            if (date.TotalHours < 0)
            {
                MessageBox.Show($"До разблокировки {(int)(date.TotalMinutes * -1)} мин", "Инфо", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            /* if (!string.Equals(_captchaInput, _captchaText))
             {
                 MessageBox.Show("Капча введена неправильно!");
                 GenerateCaptcha();
                 return;
             }*/
            var context = new MusicContext();
            var user = context.Users.FirstOrDefault(u => u.UserLogin == login && u.UserPassword == password);

            if (user == null)
            {
                Settings.Default.count++;
                MessageBox.Show("Неверные данные", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                if (Settings.Default.count == 3)
                {
                    MessageBox.Show("Осталось 2 попытки для входа до блокировки!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                if (Settings.Default.count == 5)
                {
                    MessageBox.Show($"Вы заблокированны \nВремя до разблокировки: {DateTime.Now + TimeSpan.FromHours(1)}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    Settings.Default.ban_time = DateTime.Now + TimeSpan.FromHours(1);
                    Settings.Default.Save();
                }
                return;
            }
            MessageBox.Show($"Добро пожаловать, {user.FullName}");
            Application.Current.Properties["CurrentUser"] = user;

            Settings.Default.count = 0;
            Settings.Default.userid = user.UserId;
            Settings.Default.Save();

            Window menu = new Views.Menu();
            menu.Show();
            foreach (Window w in Application.Current.Windows)
            {
                if (w is not Views.Menu)
                {
                    w.Close();
                }
            }
        }



        public void GenerateCaptcha()
        {
            captchaText = GenerateRandomString(5);
            captchaImage = GenerateCaptchaImage(captchaText);
        }

        public string GenerateRandomString(int len)
        {
            string chars = "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm1234567890";
            string result = "";
            for (int i = 0; i <len; i++)
            {
                result += chars[RandomNumberGenerator.GetInt32(chars.Length)];
            }
            return result;
        }

        public ImageSource GenerateCaptchaImage(string text)
        {
            int width = 200;
            int height = 50;
            var rnd = new Random();
            var image = new DrawingVisual();
            using (var dc = image.RenderOpen()) 
            { 
                dc.DrawRectangle(Brushes.White, null, new Rect(0, 0, width, height));
                /*var formatted = new FormattedText(text, CultureInfo.InvariantCulture, FlowDirection.LeftToRight, new Typeface("Times New Poman"), 36, Brushes.Black);
                
                dc.DrawText(formatted, new Point((width - formatted.Width)/2, (height - formatted.Height)/2));*/
                double x = 10;

                foreach (char c in text)
                {
                    var fontSize = rnd.Next(10, 40);
                    var typeface = new Typeface("Times New Roman");
                    var formatted = new FormattedText(c.ToString(), CultureInfo.InvariantCulture, FlowDirection.LeftToRight, typeface, fontSize, Brushes.Black);
                    var g = new GeometryDrawing(Brushes.Black, null, formatted.BuildGeometry(new Point(0, 0)));
                    var transformGroup = new TransformGroup();
                    transformGroup.Children.Add(new RotateTransform(rnd.NextDouble() * 13 - 9));
                    transformGroup.Children.Add(new SkewTransform(rnd.NextDouble(), rnd.NextDouble()));
                    transformGroup.Children.Add(new TranslateTransform(x, (height - fontSize) / 2 + rnd.NextDouble()  *  10 - 5));
                    dc.PushTransform(transformGroup);
                    dc.DrawGeometry(Brushes.Black,  null, formatted.BuildGeometry(new Point(0, 0)));
                    dc.Pop();
                    x += formatted.Width + rnd.Next(2, 10);

                }

                for (int i = 0; i < 300; i++)
                {
                    var px = RandomNumberGenerator.GetInt32(width);
                    var py = RandomNumberGenerator.GetInt32(height);
                    dc.DrawEllipse(Brushes.Gray, null, new Point(px, py), 1, 1);
                }

                for (int i = 0; i < RandomNumberGenerator.GetInt32(10); i++)
                {
                    var pen = new Pen(new SolidColorBrush(Color.FromRgb((byte)RandomNumberGenerator.GetInt32(255),
                                                                        (byte)RandomNumberGenerator.GetInt32(255),
                                                                        (byte)RandomNumberGenerator.GetInt32(255))), 1);
                    dc.DrawLine(pen, new Point(RandomNumberGenerator.GetInt32(width), RandomNumberGenerator.GetInt32(height)),
                                      new Point(RandomNumberGenerator.GetInt32(width), RandomNumberGenerator.GetInt32(height)));

                }

              
            }


            
            var bmp = new RenderTargetBitmap(width, height, 100, 100, PixelFormats.Pbgra32);
            bmp.Render(image);
            return bmp;
        }
    }
    
}
