using MusicWpf.Context;
using MusicWpf.Views;
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
using static System.Net.Mime.MediaTypeNames;
using Application = System.Windows.Application;
using MusicWpf.Properties;

namespace MusicWpf.ViewModel
{
    public class AuthViewModel : BaseViewModel
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

        private ImageSource _capchaImage;
        public ImageSource CapchaImage
        {
            get => _capchaImage;
            set
            {
                _capchaImage = value;
                OnPropertyChanged();
            }
        }

        private string _capcha;
        public string Capcha
        {
            get => _capcha;
            set
            {
                _capcha = value;
            }
        }

        private string _capchaK;
        public string CapchaK
        {
            get => _capchaK;
            set
            {
                _capchaK = value;
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
        public ICommand LoginCommand { get; }
        public ICommand OpenGuestCommand { get; }
        public ICommand UpdateCapchaCommand { get; }
        public AuthViewModel()
        {
            GaneratCapcha();
            LoginCommand = new RelayCommand(OnLogin);
            OpenGuestCommand = new RelayCommand(OpenGuest);
            UpdateCapchaCommand = new RelayCommand(GaneratCapcha);

            if(Settings.Default.userid != 0)
            {
                int userId = Settings.Default.userid;
                var context = new MusicContext();
                var user = context.Users.FirstOrDefault(u => u.UserId == userId);
                Application.Current.Properties["CurrentUser"] = user;
                Window menu = new Views.MenuWindow();
                menu.Show();
                foreach (Window w in Application.Current.Windows)
                {
                    if (w is not Views.MenuWindow)
                    {
                        w.Close();
                    }
                }
            }
        }
        public void OpenGuest()
        {
            Window gues = new Gost();
            gues.Show();
        }

        public void OnLogin()
        {
            var date = DateTime.Now.Subtract(Settings.Default.ban_time);
            if (date != null) 

                if (date.TotalHours < 0)
                {
                    MessageBox.Show($"До разблокировки {(int)(date.TotalMinutes * -1)} мин", "Инфо", MessageBoxButton.OK, MessageBoxImage.Information);
                }

            if (!string.Equals(_capchaK, _capcha))
            {

                MessageBox.Show("Капча введена не правлено");
                GaneratCapcha();
                return;
            }

            var context = new MusicContext();
            var user = context.Users.FirstOrDefault(u => u.UserLogin == login && u.UserPassword == password);
            if (user == null)
            {
                Settings.Default.count++;
                MessageBox.Show("Логин или пароль введены неправильно!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
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

            Window menu = new MenuWindow();
            menu.Show();
            foreach (Window w in Application.Current.Windows)
            {
                if (w is not Views.MenuWindow)
                {
                    w.Close();
                }
            }

        }





        public void GaneratCapcha()
        {
            Capcha = GaneratsRoundString(5);
            CapchaImage = GeneratsCapchaImage(Capcha);
        }
        private string GaneratsRoundString(int len)
        {
            string chars = "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm1234567890";
            string result = "";
            for (int i = 0; i < len; i++)
            {
                result += chars[RandomNumberGenerator.GetInt32(chars.Length)];
            }
            return result;
        }
        public ImageSource GeneratsCapchaImage(string text)
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
                    var formatted = new FormattedText(text, CultureInfo.InvariantCulture, FlowDirection.LeftToRight, typeface, fontSize, Brushes.Black);
                    var g = new GeometryDrawing(Brushes.Black, null, formatted.BuildGeometry(new Point(0, 0)));
                    var transformGroup = new TransformGroup();
                    transformGroup.Children.Add(new RotateTransform(rnd.NextDouble() * 13 - 9));
                    transformGroup.Children.Add(new SkewTransform(rnd.NextDouble(), rnd.NextDouble()));
                    transformGroup.Children.Add(new TranslateTransform(x, (height - fontSize) / 2 + rnd.NextDouble() * 10 - 5));
                    dc.PushTransform(transformGroup);
                    dc.DrawGeometry(Brushes.Black, null, formatted.BuildGeometry(new Point(0, 0)));
                    dc.Pop();
                    x += formatted.Width + rnd.Next(2, 10);

                }

                for (int i = 0; i < 1000; i++)
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

                //var dc = image.RenderOpen();
                // image.RenderOpen().DrawRectangle(Brushes.White, null, new Rect(0, 0, width, height));
                /*  using (var dc = image.RenderOpen())
                  {
                      dc.DrawRectangle(Brushes.White, null, new Rect(0, 0, width, height));
                      var formatted = new FormattedText(text, CultureInfo.InvariantCulture, FlowDirection.LeftToRight, new Typeface("Times New Poman"), 36, Brushes.Black);
                      dc.DrawText(formatted, new Point((width - formatted.Width) / 2, (height - formatted.Height) / 2));

                      for (int i = 0; i < RandomNumberGenerator.GetInt32(10); i++)
                      {
                          var pen = new Pen(new SolidColorBrush(Color.FromRgb((byte)RandomNumberGenerator.GetInt32(255),
                                                                              (byte)RandomNumberGenerator.GetInt32(255),
                                                                              (byte)RandomNumberGenerator.GetInt32(255))), 1);
                          dc.DrawLine(pen, new Point(RandomNumberGenerator.GetInt32(width), RandomNumberGenerator.GetInt32(height)),
                                            new Point(RandomNumberGenerator.GetInt32(width), RandomNumberGenerator.GetInt32(height)));

                      }

                      for (int i = 0; i < 1000; i++)
                      {
                          var px = RandomNumberGenerator.GetInt32(width);
                          var py = RandomNumberGenerator.GetInt32(height);
                          dc.DrawEllipse(Brushes.Gray, null, new Point(px, py), 1, 1);
                      }
                  }*/



                var imp = new RenderTargetBitmap(width, height, 100, 100, PixelFormats.Pbgra32);
            imp.Render(image);
            return imp;


        }
    }
}

    

