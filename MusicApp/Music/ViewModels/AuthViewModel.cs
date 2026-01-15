using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using Music.Context;
using Music.Views;
using Music.ViewModels;
using Music;
using System.Globalization;
using System.Security.Cryptography;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using Music.Properties;
using System.DirectoryServices.ActiveDirectory;

namespace Music.ViewModels
{
    class AuthViewModel : BaseViewModel
    {

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


        private string _captchaInput;

        public string captchaInput
        {
            get => _captchaInput;
            set
            {
                _captchaInput = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoginCommand { get; }

        public ICommand GuestCommand { get; }

        public ICommand UpdateCaptchaCommand { get; }

        public AuthViewModel()
        {
            var context = new MusicDbContext();
            if (Settings.Default.UserId != 0)
            {
                var userId = Settings.Default.UserId;
                var user = context.Users.FirstOrDefault(x => x.UserId == userId);
                Application.Current.Properties["CurrentUser"] = user;

                var mainWindow = new MainWindow();
                mainWindow.Show();
                foreach (Window w in Application.Current.Windows)
                {
                    if (w is not MainWindow)
                    {
                        w.Close();
                    }
                }
            }
            LoginCommand = new RelayCommand(OnLogin);
            GuestCommand = new RelayCommand(OpenGuest);
            UpdateCaptchaCommand = new RelayCommand(GenerateCaptcha);
            GenerateCaptcha();
        }



        public void OpenGuest()
        {


        }
        public void OnLogin()
        {
            var context = new MusicDbContext();


            _captchaInput = _captchaText;


            var date = DateTime.Now.Subtract(Settings.Default.Ban_time);
           
            if (date.TotalHours <0)
            {
              
                MessageBox.Show($"До разблокировки {(int)(date.TotalMinutes * -1)}мин", "Вы заблокированы!", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            else
            {
                var user = context.Users.FirstOrDefault(u => u.UserLogin == _login && u.UserPassword == _password);
                if (user == null)
                {
                    switch (Settings.Default.EnteranceTry)
                    {
                        case 5:
                            MessageBox.Show("Авторизация недоступна на 1 час \nПопробуйте войти позже", "Неверный логин или пароль", MessageBoxButton.OK, MessageBoxImage.Error);
                            Settings.Default.EnteranceTry = 6;
                            Settings.Default.Ban_time = DateTime.Now + TimeSpan.FromHours(1);
                            Settings.Default.Save();
                            break;
                        case 0:
                        case 1:
                        case 2:
                            ++Settings.Default.EnteranceTry;
                            break;
                        case 3:
                        case 4:
                            MessageBox.Show($"Осталось попыток: {5 - Settings.Default.EnteranceTry}", "Неверный логин или пароль", MessageBoxButton.OK, MessageBoxImage.Warning);
                            ++Settings.Default.EnteranceTry;
                            break;
                        default:
                            MessageBox.Show("Попробуйте войти позже", "Вход заблокирован", MessageBoxButton.OK, MessageBoxImage.Error);
                            break;
                    }
                    Settings.Default.Save();
                }
                else

                {
                    if (!string.Equals(_captchaInput, _captchaText))
                    {
                        MessageBox.Show("Капча введена неверно", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        GenerateCaptcha();
                        return;
                    }
                    else
                    {
                        Settings.Default.UserId = user.UserId;
                        Settings.Default.Save();
                        Application.Current.Properties["CurrentUser"] = user;

                        var mainWindow = new MainWindow();
                        mainWindow.Show();
                        foreach (Window w in Application.Current.Windows)
                        {
                            if (w is not MainWindow)
                            {
                                w.Close();
                            }
                        }
                        Settings.Default.EnteranceTry = 0;



                    }

                }
            }
            }
        public ImageSource GenerateCaptchaImage(string text)
        {
            int width = 200;
            int height = 80;
            Random rnd = new Random();
            var image = new DrawingVisual();


            using (var dc = image.RenderOpen())
            {

                dc.DrawRectangle(Brushes.White, null, new Rect(0, 0, width, height));

          

                double x = 10;

                foreach (char c in text)
                {
                    int fontSize = rnd.Next(20, 40);
                    var typeFace = new Typeface("Times New Roman");
                    var formatted = new FormattedText(c.ToString(),
                    CultureInfo.InvariantCulture,
                    FlowDirection.LeftToRight,
                    typeFace,
                    fontSize, Brushes.Black);

                    var g = new GeometryDrawing(Brushes.Red, null, formatted.BuildGeometry(new Point(0, 0)));
                    var transformGroup = new TransformGroup();
                    transformGroup.Children.Add(new RotateTransform(rnd.NextDouble() * 4 - 8));
                    transformGroup.Children.Add(new SkewTransform(rnd.NextDouble() * 13 - 7, rnd.NextDouble() * 13 - 7));
                    transformGroup.Children.Add(new TranslateTransform(x, (height - fontSize) / 2 + rnd.NextDouble() * 10 - 5));
                    dc.PushTransform(transformGroup);
                    dc.DrawGeometry(new SolidColorBrush(Color.FromRgb((byte)RandomNumberGenerator.GetInt32(255),
                                                                        (byte)RandomNumberGenerator.GetInt32(255),
                                                                        (byte)RandomNumberGenerator.GetInt32(150))), null, formatted.BuildGeometry(new Point(0, 0)));
                    dc.Pop();
                    x += formatted.Width + rnd.Next(2, 10);
                }




                for (int i = 0; i < RandomNumberGenerator.GetInt32(10); i++)
                {
                    var pen = new Pen(new SolidColorBrush(Color.FromRgb((byte)RandomNumberGenerator.GetInt32(255),
                                                                        (byte)RandomNumberGenerator.GetInt32(255),
                                                                        (byte)RandomNumberGenerator.GetInt32(255))), 4);
                    dc.DrawLine(pen,
                        new Point(RandomNumberGenerator.GetInt32(width), RandomNumberGenerator.GetInt32(width)),
                        new Point(RandomNumberGenerator.GetInt32(height), RandomNumberGenerator.GetInt32(height))
                        );
                }

                for (int i = 0; i < 500; i++)
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
        public void GenerateCaptcha()
        {
            captchaText = GenerateRandomString(5);
            captchaImage = GenerateCaptchaImage(captchaText);
        }

        public string GenerateRandomString(int length)
        {
            string chars = "AБВГДЕЁЖЗИКЛМНОПРСТУФХЦЧЩШЬЭЮЯабвгдеёжзиклмнопрстуфхцчшщьэюя0123456789";
            string result = "";
            for (int i = 0; i < length; i++)
            {
                result += chars[RandomNumberGenerator.GetInt32(chars.Length)];
            }
            return result;
        }

    }
}
