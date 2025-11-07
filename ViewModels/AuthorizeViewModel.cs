using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using SpotApp_wpf.Context;
using SpotApp_wpf.Models;
using SpotApp_wpf.Views;

namespace SpotApp_wpf.ViewModels
{
    public class AuthorizeViewModel : BaseViewModel
    {
        private ImageSource _capchaImage;
        public ImageSource capchaImage
        {
            get => _capchaImage;
            set
            {
                _capchaImage = value;
                OnPropertyChanged();
            }
        }
        private string _capchaInput;
        public string capchaInput
        {
            get => _capchaInput;
            set
            {
                _capchaInput = value;
                OnPropertyChanged();
            }
        }
        private string _capchaText;
        public string capchaText
        {
            get => _capchaText;
            set
            {
                _capchaText = value;
                OnPropertyChanged();
            }
        }

        private string _login = "aleksey.novojilov";
        public string login
        {
            get => _login;
            set
            {
                _login = value;
            }
        }
        private string _password = "1XF4kEOq";
        public string password
        {
            get => _password;
            set
            {
                _password = value;
            }
        }

        public ICommand LoginCommand { get; }
        public ICommand OpenGuestCommand { get; }
        public ICommand UpdateCapchaCommand { get; }
        public AuthorizeViewModel()
        {
            GenerateCapcha();
            LoginCommand = new RelayCommand(Login);
            OpenGuestCommand = new RelayCommand(OpenGuest);
            UpdateCapchaCommand = new RelayCommand(GenerateCapcha);
        }

        public void OpenGuest()
        {
            Window g = new Views.GuestWindow();
            g.Show();
        }

        public void Login()
        {
            if (!string.Equals(_capchaInput, _capchaText))
            {
                MessageBox.Show("Капча введена неверно");
                return;
            }

            var context = new SpotifyContext();
            var user = context.Users.FirstOrDefault(u => u.UserLogin == login && u.UserPassword == password);
            if (user == null)
            {
                MessageBox.Show("Неверные данные");
                return;
            }
            MessageBox.Show($"Добро пожаловать, {user.FullName}");
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

        public void GenerateCapcha()
        {
            capchaText = GenerateRandomString(5);
            capchaImage = GenerateCapchaImage(capchaText);
        }

        public string GenerateRandomString(int len)
        {
            string chars = "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm1234567890";
            /*string chars = "ЁЙЦУКЕНГШЩЗФЫВАПРОЛДЯЧСМИТЬйцукенгшщзхъфывапролджэячсмитьбюQWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm1234567890";*/
            string result = "";
            for (int i = 0; i < len; i++)
            {
                result += chars[RandomNumberGenerator.GetInt32(chars.Length)];
            }
            return result;
        }

        public ImageSource GenerateCapchaImage(string text)
        {
            int width = 200;
            int height = 50;
            var image = new DrawingVisual();
            var rnd = new Random();

            using (var dc = image.RenderOpen())
            {
                dc.DrawRectangle(Brushes.White, null, new Rect(0, 0, width, height));
                /*var formatted = new FormattedText(text, CultureInfo.InvariantCulture, FlowDirection.LeftToRight, new Typeface("Times New Poman"), 36, Brushes.Black);
                
                
                dc.DrawText(formatted, new Point((width - formatted.Width)/2, (height - formatted.Height)/2));*/
                double x = 10;
                string txt = text;
                foreach (char c in text)
                {
                    var fontSize = rnd.Next(20, 40);
                    var typeFace = new Typeface("Times New Roman");
                    var formatted = new FormattedText(c.ToString(), CultureInfo.InvariantCulture, FlowDirection.LeftToRight, typeFace, fontSize, Brushes.Black);
                    var g = new GeometryDrawing(Brushes.Black, null, formatted.BuildGeometry(new Point(0,0)));
                    var transFromGroup = new TransformGroup();
                    transFromGroup.Children.Add(new RotateTransform(rnd.NextDouble() * 12 - 6));
                    transFromGroup.Children.Add(new SkewTransform(rnd.NextDouble(), rnd.NextDouble()));
                    transFromGroup.Children.Add(new TranslateTransform(x, (height - fontSize) / 2 + rnd.NextDouble()));
                    dc.PushTransform(transFromGroup);
                    dc.DrawGeometry(Brushes.Black, null, formatted.BuildGeometry(new Point(0,0)));
                    dc.Pop();
                    x += formatted.Width + rnd.Next(2,10);
                }

                for (int i = 0; i < 500; i++)
                {
                    var px = RandomNumberGenerator.GetInt32(width);
                    var py = RandomNumberGenerator.GetInt32(height);
                    dc.DrawEllipse(Brushes.Gray, null, new Point(px, py), 1, 1);
                }
                for (int i = 0; i < RandomNumberGenerator.GetInt32(20); i++) 
                {
                    var pen = new Pen(new SolidColorBrush(Color.FromRgb((byte)RandomNumberGenerator.GetInt32(255),
                                                                         (byte)RandomNumberGenerator.GetInt32(255),
                                                                         (byte)RandomNumberGenerator.GetInt32(255))), 1);
                    dc.DrawLine(pen, new Point(RandomNumberGenerator.GetInt32(width), RandomNumberGenerator.GetInt32(height)),
                                     new Point(RandomNumberGenerator.GetInt32(width), RandomNumberGenerator.GetInt32(height))); 
                }
            }
            
            
            var tmp = new RenderTargetBitmap(width, height, 100, 100, PixelFormats.Pbgra32);
            tmp.Render(image);
            return tmp;
        }
    }
}



