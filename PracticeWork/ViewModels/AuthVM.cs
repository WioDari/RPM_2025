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
using PracticeWork.Context;
using PracticeWork.Models;
using PracticeWork.Properties;
using PracticeWork.Windows;
using PracticeWork.Commands;

using static System.Net.Mime.MediaTypeNames;

namespace PracticeWork.ViewModels
{
    class AuthVM : BaseVM
    {
        private string _login = "antonina.pushmenkova";
        public string Login
        {
            get => _login;
            set
            {
                _login = value;
                OnPropertyChanged();
            }
        }

        private string _password = "it3cy55";
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        private string _captcha;
        public string Captcha
        {
            get => _captcha;
            set
            {
                _captcha = value;
                OnPropertyChanged();
            }
        }

        private string _capchaTry;
        public string CaptchaTry
        {
            get => _capchaTry;
            set
            {
                _capchaTry = value;
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
                OnPropertyChanged();
            }
        }

        public ICommand LoginCommand { get; }
        public ICommand GuestModeCommand { get; }
        public ICommand ReGenCapchaCommand { get; }
        
        public AuthVM()
        {
            GenCapcha();
            LoginCommand = new RelayCommand(OnLogin);
            ReGenCapchaCommand = new RelayCommand(GenCapcha);
        }

        public void GenCapcha()
        {
            Captcha = GenRandomString(7);
            CaptchaImage = GenCapchaImage(Captcha);
        }

        public string GenRandomString(int len)
        {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890";
            string result = "";
            for (int i = 0; i < len; i++)
            {
                result += chars[RandomNumberGenerator.GetInt32(chars.Length)];
            }
            return result;
        }

        public ImageSource GenCapchaImage(string capcha)
        {
            int width = 200;
            int height = 50;
            var image = new DrawingVisual();
            var rnd = new Random();

            using (var dc = image.RenderOpen())
            {
                dc.DrawRectangle(Brushes.White, null, new Rect(0, 0, width, height));

                double x = 10;
                string txt = capcha;
                foreach (char c in capcha)
                {
                    var fontSize = rnd.Next(20, 40);
                    var typeFace = new Typeface("Times New Roman");
                    var formatted = new FormattedText(c.ToString(), CultureInfo.InvariantCulture, FlowDirection.LeftToRight, typeFace, fontSize, Brushes.Black);
                    var g = new GeometryDrawing(Brushes.Black, null, formatted.BuildGeometry(new Point(0, 0)));
                    var transFromGroup = new TransformGroup();
                    transFromGroup.Children.Add(new RotateTransform(rnd.NextDouble() * 12 - 6));
                    transFromGroup.Children.Add(new SkewTransform(rnd.NextDouble(), rnd.NextDouble()));
                    transFromGroup.Children.Add(new TranslateTransform(x, (height - fontSize) / 2 + rnd.NextDouble()));
                    dc.PushTransform(transFromGroup);
                    dc.DrawGeometry(Brushes.Black, null, formatted.BuildGeometry(new Point(0, 0)));
                    dc.Pop();
                    x += formatted.Width + rnd.Next(2, 10);
                }

                for (int i = 0; i < 500; i++)
                {
                    var px = RandomNumberGenerator.GetInt32(width);
                    var py = RandomNumberGenerator.GetInt32(height);
                    dc.DrawEllipse(Brushes.Gray, null, new Point(px, py), 1, 1);
                }
                for (int i = 0; i < RandomNumberGenerator.GetInt32(10, 20); i++)
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

        public void GuestEnter()
        {

        }

        public void OnLogin()
        {
            var context = new PostgresContext();
            var user = context.Users.FirstOrDefault(u => u.UserLogin == Login && u.UserPassword == Password);
            var date = DateTime.Now.Subtract(Settings.Default.ban_time);
            if (date != null)
            {
                if (date.TotalHours > 0)
                {
                    MessageBox.Show($"Вы забанены\nДо разблокировки {date.TotalMinutes * -1}", "Ошибка", MessageBoxButton.OK);
                }
            }
            else
            {
                if (CaptchaTry != Captcha)
                {
                    MessageBox.Show("Вы ошиблись при вводе капчи", "Не верная капча", MessageBoxButton.OK, MessageBoxImage.Error);
                    CaptchaTry = null;
                GenCapcha();
                return;
                }
                if (user == null)
                {
                    Settings.Default.EnterTries += 1;
                    if (Settings.Default.EnterTries == 3)
                    {
                        MessageBox.Show("Осталось 2 попытки до блокировки", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    if (Settings.Default.EnterTries == 5)
                    {
                        MessageBox.Show("Вы заблокированы");
                    }
                }
                else
                {
                    System.Windows.Application.Current.Properties["CurUser"] = user;
                    Settings.Default.EnterTries = 0;
                    Settings.Default.userId = user.UserId;
                    Settings.Default.Save();
                    var mainpage = new MainPage();
                    mainpage.Show();
                    foreach (Window w in App.Current.Windows)
                    {
                        if (w is not MainPage)
                        {
                            w.Close();
                        }
                    }
                }
            }
        }
    }
}
