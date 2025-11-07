using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Practos2.Context;
using System.Windows.Controls;
using System.Windows.Input;
using Practos2.Views;
using Practos2.Commands;
using System.Security.Cryptography;
using System.Windows.Media;
using System.Globalization;
using System.Windows.Media.Imaging;
using Microsoft.EntityFrameworkCore.Diagnostics.Internal;
using System.Runtime.Serialization;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Speech;
using System.Speech.Synthesis;

namespace Practos2.ViewModel
{
    class EnterViewModel : BaseViewModel
    {
        private ImageSource? _captchaImage;
        public ImageSource? captchaImage
        {
            get => _captchaImage;
            set
            {
                _captchaImage = value;
                OnPropertyChanged();
            }
        }

        private string? _captchaInput;
        public string? captchaInput
        {
            get => _captchaInput;
            set
            {
                _captchaInput = value;
                OnPropertyChanged();
            }
        }

        private string? _captchaText;
        public string? captchaText
        {
            get => _captchaText;
            set
            {
                _captchaText = value;
                OnPropertyChanged();
            }
        }

        private string? _login;
        public string? login
        {
            get => _login;
            set
            {
                _login = value;
            }
        }

        private string? _password;
        public string? password
        {
            get => _password;
            set
            {
                _password = value;
            }
        }
        public ICommand LoginCommand { get; }
        public ICommand UpdateCaptchaCommand { get; }
        public EnterViewModel()
        {
            GenerateCaptcha();

            LoginCommand = new RelayCommand(OnLogin);
            UpdateCaptchaCommand = new RelayCommand(GenerateCaptcha);
        }

        public void OnLogin()
        {
            if (string.Equals(_captchaInput, _captchaText))
            {
                MessageBox.Show("Каптча введена неверно!", "Ошибка!");
                GenerateCaptcha();
                return;
            }

            var context = new NewDemkaContext();
            var user = context.Users.FirstOrDefault(u => u.Userlogin == login && u.Userpassword == password);
            if (user == null)
            {
                Warning_Image warn = new();
                warn.Show();
                return;
            }
            Application.Current.Properties["CurrentUser"] = user;

            Window main = new MainWindow();
            main.Show();
            foreach (Window w in Application.Current.Windows)
            {
                if (w is not Views.MainWindow)
                {
                    w.Close();
                }
            }
        }

        public void GenerateCaptcha()
        {
            SpeechSynthesizer sent = new();
            sent.Rate = 1;
            sent.Volume = 100;

            captchaText = GenerateRandomString(5);
            captchaImage = GenerateCaptchaImage(captchaText);

            sent.Speak(captchaText);
        }

        public string GenerateRandomString(int len)
        {
            string chars = "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm1234567890";
            string result = "";
            for (int i=0;i<len;i++)
            {
                result += chars[RandomNumberGenerator.GetInt32(chars.Length)];
            }
            return result;
        }

        public ImageSource GenerateCaptchaImage(string text)
        {
            int width = 200;
            int height = 50;
            var rmd = new Random();
            var image = new DrawingVisual();
            List<FontFamily> font = Fonts.SystemFontFamilies.ToList();

            using (var dc = image.RenderOpen())
            {
                dc.DrawRectangle(Brushes.White, null, new Rect(0, 0, width, height));

                double x = 10;
                foreach(char c in text)
                {
                    var fontColors = Color.FromRgb((byte)RandomNumberGenerator.GetInt32(255),
                                                  (byte)RandomNumberGenerator.GetInt32(255),
                                                  (byte)RandomNumberGenerator.GetInt32(255));

                    var fontSize = rmd.Next(20, 40);
                    var typeFace = new Typeface(font[rmd.Next(0, 20)].ToString());
                    var format = new FormattedText(text, CultureInfo.InvariantCulture, FlowDirection.LeftToRight, new Typeface("Times new Roman"), 30, new SolidColorBrush(fontColors));
                    var g = new GeometryDrawing(Brushes.Black, null, format.BuildGeometry(new Point(0, 0)));
                    var transformGroup = new TransformGroup();
                    transformGroup.Children.Add(new RotateTransform(rmd.NextDouble() * 13 - 9)); // Формулы необязательны
                    transformGroup.Children.Add(new SkewTransform(rmd.NextDouble() * 19 - 2, rmd.NextDouble() * 19 - 2));
                    transformGroup.Children.Add(new TranslateTransform(x, (height - fontSize) / 2 + rmd.NextDouble() * 10 - 5));
                    dc.PushTransform(transformGroup);
                    dc.DrawGeometry(Brushes.Black, null, format.BuildGeometry(new Point(0, 0)));
                    dc.Pop();
                    x += format.Width + rmd.Next(2, 10);
                }

                for (int i = 0; i < 4500; i++)
                {
                    var px = RandomNumberGenerator.GetInt32(width);
                    var py = RandomNumberGenerator.GetInt32(height);
                    dc.DrawEllipse(Brushes.Gray, null, new Point(px, py), 1, 1);
                }

                for (int i = 0; 1 < RandomNumberGenerator.GetInt32(10); i++)
                {
                    var pen = new Pen(new SolidColorBrush(Color.FromRgb((byte)RandomNumberGenerator.GetInt32(255),
                                                                        (byte)RandomNumberGenerator.GetInt32(255),
                                                                        (byte)RandomNumberGenerator.GetInt32(255))), 1);

                    dc.DrawLine(pen, new Point(RandomNumberGenerator.GetInt32(width), RandomNumberGenerator.GetInt32(height)),
                                     new Point(RandomNumberGenerator.GetInt32(width), RandomNumberGenerator.GetInt32(height)));
                }
            }

            var bap = new RenderTargetBitmap(width, height, 100, 100, PixelFormats.Pbgra32);
            bap.Render(image);
            return bap;
        }
    }
}
