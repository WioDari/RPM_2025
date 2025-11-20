using Music__.Commands;
using Music__.Context;
using Music__.Properties;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Music__.Views;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Security.Cryptography;
using System.Globalization;

namespace Music__.ViewModels
{
    public class AuthorizationViewModel : BaseViewModel
    {
        private ImageSource _captchaimage;
        public ImageSource captchaimage
        {
            get => _captchaimage;
            set
            {
                _captchaimage = value;
                OnPropertyChanged();
            }
        }

        private string _captchainput;
        public string captchainput
        {
            get => _captchainput;
            set
            {
                _captchainput = value;
                OnPropertyChanged();
            }
        }

        private string _captchatext;
        public string captchatext
        {
            get => _captchatext;
            set
            {
                _captchatext = value;
                OnPropertyChanged();
            }
        }

        private string _login = "antonina.pushmenkova";
        public string login
        {
            get => _login;
            set
            {
                _login = value;
                OnPropertyChanged();
            }
        }

        private string _password = "it3cySf5";
        public string password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        private bool _ischecked;
        public bool IsChecked
        {
            get => _ischecked;
            set
            {
                _ischecked = value;
                Application.Current.Properties["IsChecked"] = value;
                OnPropertyChanged();
            }
        }



        public ICommand LoginCommand { get; }
        public ICommand GuestModeCommand { get; }
        public ICommand UpdateCaptchaCommand { get; }



        public AuthorizationViewModel()
        {
            GenerateCaptcha();


            LoginCommand = new RelayCommand(OnLogin);
            GuestModeCommand = new RelayCommand(OpenGuestMode);
            UpdateCaptchaCommand = new RelayCommand(GenerateCaptcha);


            if (Settings.Default.userid != 0)
            {
                int userid = Settings.Default.userid;
                var context = new SpotifyContext();
                var user = context.Users.FirstOrDefault(x => x.Userid == userid);
                Application.Current.Properties["CurrentUser"] = user;
                
                


                new MainWindow().Show();
                foreach (Window w in Application.Current.Windows)
                {
                    if (w is not MainWindow)
                    {
                        w.Close();
                    }
                }
            }
        }


        public void OnLogin()
        {
            if (Settings.Default.unblockdatetime != default && Settings.Default.unblockdatetime > DateTime.Now)
            {
                MessageBox.Show($"Вход заблокирован до {Settings.Default.unblockdatetime} из-за слишком большого кол-ва неудачных попыток.", "Авторизация временно заблокирована", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!string.Equals(_captchainput, _captchatext))
            {
                MessageBox.Show("Капча введена неверно", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                GenerateCaptcha();
                return;
            }


            var context = new SpotifyContext();
            var user = context.Users.FirstOrDefault(x => x.Login == _login);
            

            if (user == null)
            {
                MessageBox.Show("Неверный логин", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                var pword = context.Users.FirstOrDefault(x => user.Password == _password);
                if (pword == null)
                {
                    Settings.Default.failedloginattempts += 1;
                    Settings.Default.Save();
                    GenerateCaptcha();
                    if (Settings.Default.failedloginattempts >= 3)
                    {
                        if (Settings.Default.failedloginattempts >= 5)
                        {
                            Settings.Default.unblockdatetime = DateTime.Now + TimeSpan.FromHours(0.02);
                            Settings.Default.Save();
                            MessageBox.Show($"Вход заблокирован до {Settings.Default.unblockdatetime} из-за слишком большого кол-ва неудачных попыток.", "Авторизация временно заблокирована", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        else
                        {
                            MessageBox.Show($"Неверный пароль. Попыток до блокировки: {5 - Settings.Default.failedloginattempts} :)", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Неверный пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    return;
                }
                else
                {
                    if (Settings.Default.unblockdatetime != default && Settings.Default.unblockdatetime > DateTime.Now)
                    {
                        MessageBox.Show($"Вход заблокирован до {Settings.Default.unblockdatetime} из-за слишком большого кол-ва неудачных попыток.", "Авторизация временно заблокирована", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    Application.Current.Properties["CurrentUser"] = user;
                    Settings.Default.userid = user.Userid;
                    Settings.Default.failedloginattempts = 0;
                    Settings.Default.unblockdatetime = default;
                    Settings.Default.Save();

                    new MainWindow().Show();
                    foreach (Window w in Application.Current.Windows)
                    {
                        if (w is not MainWindow)
                        {
                            w.Close();
                        }
                    }
                }
            }
            
            
        }


        public void OpenGuestMode()
        {
            Application.Current.Properties["CurrentUser"] = "Guest";

            new MainWindow().Show();
            foreach (Window w in Application.Current.Windows)
            {
                if (w is not MainWindow)
                {
                    w.Close();
                }
            }
        }



        public string GenerateRandomText(int length)
        {
            string chars = "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm0123456789";
            string result = "";
            for (int i = 0; i< length; i++)
            {
                result += chars[RandomNumberGenerator.GetInt32(chars.Length)];
            }
            return result;
        }

        
        public ImageSource GenerateCaptchaImage(string text)
        {
            int width = 200;
            int height = 50;
            var random = new Random();
            var image = new DrawingVisual();

            using (var dc = image.RenderOpen())
            {
                dc.DrawRectangle(Brushes.White, null, new Rect(0, 0, width, height));

                double x = 10;
                foreach(char c in text)
                {
                    var fontsize = random.Next(28, 42);
                    var typeface = new Typeface("Times New Roman");
                    var fontcolor = Color.FromRgb((byte)random.Next(100, 255), (byte)random.Next(100, 255), (byte)random.Next(100,255));
                    var formatted = new FormattedText(c.ToString(), CultureInfo.InvariantCulture, FlowDirection.LeftToRight, typeface, fontsize, Brushes.Black);
                    var g = new GeometryDrawing(Brushes.Black, null, formatted.BuildGeometry(new Point(0, 0)));
                    var transformgroup = new TransformGroup();
                    transformgroup.Children.Add(new RotateTransform(random.NextDouble() * 13 - 9));
                    transformgroup.Children.Add(new SkewTransform(random.NextDouble() * 19 - 2, random.NextDouble() * 19 - 2));
                    transformgroup.Children.Add(new TranslateTransform(x, (height - fontsize) / 2 + random.NextDouble() * 10 - 5));
                    dc.PushTransform(transformgroup);
                    dc.DrawGeometry(Brushes.Black, null, formatted.BuildGeometry(new Point(0, 0)));
                    dc.Pop();
                    x += formatted.Width + random.Next(2, 10);
                }

                for (int i = 0; i < 1000; i++)
                {
                    var px = RandomNumberGenerator.GetInt32(width);
                    var py = RandomNumberGenerator.GetInt32(height);
                    dc.DrawEllipse(Brushes.Gray, null, new Point(px, py), 1, 1);
                }

                for (int i = 0; i < RandomNumberGenerator.GetInt32(10); i++)
                {
                    var pen = new Pen(new SolidColorBrush(Color.FromRgb((byte)RandomNumberGenerator.GetInt32(255), (byte)RandomNumberGenerator.GetInt32(255), (byte)RandomNumberGenerator.GetInt32(255))), 1);
                    dc.DrawLine(pen, new Point(RandomNumberGenerator.GetInt32(width), RandomNumberGenerator.GetInt32(height)), new Point(RandomNumberGenerator.GetInt32(width), RandomNumberGenerator.GetInt32(height)));
                }

            }
            var bmp = new RenderTargetBitmap(width, height, 100, 100, PixelFormats.Pbgra32);
            bmp.Render(image);
            return bmp;
        }
        

        public void GenerateCaptcha()
        {
            captchatext = GenerateRandomText(4);
            captchaimage = GenerateCaptchaImage(captchatext);
        }
    }
}
