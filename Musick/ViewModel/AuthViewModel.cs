using Musick.Context;
using Musick.Views;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Musick.ViewModel
{
    class AuthViewModel : BaseViewsModel
    {
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

        public ICommand GuestCommand { get; }

        public ICommand UpdateCaptchaCommand { get; }

        public AuthViewModel()
        {
            LoginCommand = new RelayCommand(OnLogin);
            GuestCommand = new RelayCommand(OpenGuest);
            UpdateCaptchaCommand = new RelayCommand(GenerateCaptcha);
            GenerateCaptcha();
        }


        public void OpenGuest()
        {
            MainWindow wnd = new MainWindow();
            wnd.ShowDialog();

        }


        public void OnLogin()
        {

            if (!string.Equals(_captchaInput, _captchaText))
            {
                MessageBox.Show("Капча введена неверно", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                GenerateCaptcha();
                return;
            }


            MusicContext context = new MusicContext();

            var user = context.Users.FirstOrDefault(x => x.Password == password && x.UserLogin == login);
            if (user == null)
            {
                MessageBox.Show("Невверно введён логин или пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Application.Current.Properties["CurrentUser"] = user;

            Menu wnd = new Menu();
            wnd.Show();
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

        public string GenerateRandomString(int length)
        {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            string result = "";
            for (int i = 0; i < length; i++)
            {
                result += chars[RandomNumberGenerator.GetInt32(chars.Length)];
            }
            return result;
        }


        public ImageSource GenerateCaptchaImage(string text)
        {
            int width = 200;
            int height = 50;
            Random rnd = new Random();
            var image = new DrawingVisual();
            

             using (var dc = image.RenderOpen())
             {

                dc.DrawRectangle(Brushes.White, null, new Rect(0, 0, width, height));

                /*var formatted = new FormattedText(text,
                    CultureInfo.InvariantCulture,
                    FlowDirection.LeftToRight,
                    new Typeface("Times New Roman"),
                    36, Brushes.Black);

                

                dc.DrawText(formatted, new Point((width - formatted.Width)/2, (height - formatted.Height)/2));*/

                double x = 10;

                foreach(char c in text)
                {
                    int fontSize = rnd.Next(20, 40);
                    var typeFace = new Typeface("Times New Roman");
                    var formatted = new FormattedText(c.ToString(),
                    CultureInfo.InvariantCulture,
                    FlowDirection.LeftToRight,
                    typeFace,
                    fontSize, Brushes.Black);

                    var g = new GeometryDrawing(Brushes.Black, null, formatted.BuildGeometry(new Point(0, 0)));
                    var transformGroup = new TransformGroup();
                    transformGroup.Children.Add(new RotateTransform(rnd.NextDouble() *4  - 8 ));
                    transformGroup.Children.Add(new SkewTransform(rnd.NextDouble() * 13 - 7 , rnd.NextDouble() * 13 - 7));
                    transformGroup.Children.Add(new TranslateTransform(x, (height - fontSize) / 2 +rnd.NextDouble() * 10 - 5 ));
                    dc.PushTransform(transformGroup);
                    dc.DrawGeometry(new SolidColorBrush(Color.FromRgb((byte)RandomNumberGenerator.GetInt32(255),
                                                                        (byte)RandomNumberGenerator.GetInt32(255),
                                                                        (byte)RandomNumberGenerator.GetInt32(255))), null, formatted.BuildGeometry(new Point(0, 0)));
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

                for (int i = 0; i <500; i++)
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
        }
    }

