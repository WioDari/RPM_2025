using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Microsoft.EntityFrameworkCore;
using MsBox.Avalonia;
using MsBox.Avalonia.Dto;
using MusicPlusPlus.Context;
using MusicPlusPlus.Properties;
using System;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace MusicPlusPlus
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        //СОЗДАНИЕ КАПЧИ, ФОКУС НА ЛОГИН ПРИ ОТКРЫТИИ ОКНА, ОТКРЫТИЕ МЕНЮ, ЕСЛИ РАНЕЕ АВТОРИЗОВАН
        protected override void OnLoaded(RoutedEventArgs e)
        {
            base.OnLoaded(e);

            LoginTB.Focus();
            GenerateCaptcha();

            if (Settings.Default.userid != 0)
            {
                using (var context = new SpotifyContext())
                {
                    var user = context.Users.FirstOrDefault(x => x.Userid == Settings.Default.userid);
                    user.Lastlogindate = DateOnly.FromDateTime(DateTime.Now);
                    context.SaveChanges();
                }
                new MenuW().Show();
                this.Close();
            }
        }



        //ПЕРЕКЛЮЧЕНИЕ ВИДИМОСТИ ПАРОЛЯ
        private void PasswordVisibilityRB_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            switch (PasswordTB.RevealPassword)
            {
                case true: PasswordTB.RevealPassword = false; break;
                case false: PasswordTB.RevealPassword = true; break;
            }
        }


        //ОБНОВЛЕНИЕ КАПЧИ ПО КНОПКЕ
        private void RefreshCaptchaB_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            GenerateCaptcha();
        }


        //АВТОРИЗАЦИЯ
        private void LoginB_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if(Settings.Default.UnlockDT > DateTime.Now)
            {
                new MessageWindow("Вход заблокирован", $"Вы были заблокированы из-за слишком большого количества неудачных попыток ввода пароля. Время разблокировки: { Settings.Default.UnlockDT }").Show();
                return;
            }
            if (Settings.Default.UnlockDT <= DateTime.Now)
            {
                Settings.Default.failedattempts = 0;
                Settings.Default.Save();
            }

            string login = LoginTB.Text ?? string.Empty;
            string password = PasswordTB.Text ?? string.Empty;
            string captchatext = CaptchaTB.Text ?? string.Empty;

            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(captchatext))
            {
                new MessageWindow("Некорректный ввод", "Заполните все поля").Show();
                CaptchaTB.Text = "";
                return;
            }

            if (captchatext != CaptchaText)
            {
                new MessageWindow("Некорректный ввод", "Капча была введена неверно, попробуйте ещё раз").Show();
                GenerateCaptcha();
                CaptchaTB.Text = "";
                CaptchaTB.Focus();
                return;
            }

            try
            {
                using (var context = new SpotifyContext())
                {

                    var user = context.Users.FirstOrDefault(x => x.Login == login);

                    if (user == null)
                    {
                        new MessageWindow("Некорректный ввод", "Такого пользователя не существует").Show();
                        LoginTB.Focus();
                        CaptchaTB.Text = "";
                        return;
                    }
                    if (user != null && password != Convert.ToString(user.Password))
                    {
                        
                        Settings.Default.failedattempts++;
                        Settings.Default.Save();
                        if (Settings.Default.failedattempts >= 3 && Settings.Default.failedattempts < 5)
                        {
                            new MessageWindow("Предупреждение", "У вас осталось ещё две попытки ввода пароля до блокировки.").Show();
                            GenerateCaptcha();
                            CaptchaTB.Text = "";
                            PasswordTB.Focus();
                            return;
                        }
                        else if (Settings.Default.failedattempts >= 5)
                        {
                            Settings.Default.UnlockDT = DateTime.Now + TimeSpan.FromHours(1);
                            Settings.Default.Save();
                            new MessageWindow("Вход заблокирован", $"Слишком много неудачных попыток ввода пароля. Попытки входа заблокированы на час. Время разблокировки: {Settings.Default.UnlockDT}").Show();
                            return;
                        }
                        else
                        {
                            new MessageWindow("Некорректный ввод", "Пароль был введён неверно, попробуйте ещё раз").Show();
                            GenerateCaptcha();
                            CaptchaTB.Text = "";
                            PasswordTB.Focus();
                            return;
                        }
                    }
                    user.Lastlogindate = DateOnly.FromDateTime(DateTime.Now);
                    context.SaveChanges();
                    Settings.Default.userid = Convert.ToInt32(user.Userid);
                    Settings.Default.Save();
                    Settings.Default.failedattempts = 0;
                    Settings.Default.Save();

                    new MenuW().Show();
                    this.Close();

                }
            }
            catch (Exception error)
            {
                new MessageWindow("Ошибка", error.Message).Show();
            }


        }



        //КАПЧА
        #region
        //генерация текста
        private string GenerateCaptchaText(int length)
        {
            string Chars = "йцукенгшщзхъфывапролджэячсмитьбю1234567890";
            string Result = "";
            for (int i = 0; i < length; i++)
            {
                Result += Chars[RandomNumberGenerator.GetInt32(Chars.Length)];
            }
            return Result;
        }

        //генерация картинки
        public static RenderTargetBitmap GenerateCaptchaImage(string text)
        {
            int width = 200;
            int height = 50;
            var random = new Random();

            var bitmap = new RenderTargetBitmap(new PixelSize(width, height));

            using (var context = bitmap.CreateDrawingContext())
            {
                context.FillRectangle(Brushes.White, new Rect(0, 0, width, height));

                double x = 30;

                foreach (char c in text)
                {
                    double Fontsize = random.Next(29, 35);
                    var color = Color.FromRgb((byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255));
                    var typeface = new Typeface("Times New Roman");
                    var brush = new SolidColorBrush(color);

                    var formattedtext = new FormattedText(c.ToString(), CultureInfo.InvariantCulture, FlowDirection.LeftToRight, typeface, Fontsize, brush);

                    var translation = Matrix.CreateTranslation(x + random.Next(-9, 10), (height - 1.5 * Fontsize));

                    var skew = Matrix.CreateSkew(Math.PI / 180 * random.Next(-9, 10), Math.PI / 180 * random.Next(-9, 10));

                    var rotation = Matrix.CreateRotation(Math.PI / 180 * random.Next(-9, 10));

                    var FinalMatrix = translation * skew * rotation;


                    using (context.PushPreTransform(FinalMatrix))
                    {
                        context.DrawText(formattedtext, new Point(0, 0));
                    }

                    x += formattedtext.Width + random.Next(5, 15);


                    for (int i = 0; i < 10; i++)
                    {
                        int px = random.Next(width);
                        int py = random.Next(height);
                        var nc = Color.FromArgb(70, (byte)random.Next(50, 180), (byte)random.Next(50, 180), (byte)random.Next(50, 180));
                        context.DrawEllipse(new SolidColorBrush(nc), null, new Point(px, py), 1, 1);
                    }


                    for (int i = 0; i < random.Next(1, 4); i++)
                    {
                        var pen = new Pen(new SolidColorBrush(Color.FromArgb(90,
                            (byte)random.Next(0, 80),
                            (byte)random.Next(0, 80),
                            (byte)random.Next(0, 80))), 2);

                        context.DrawLine(pen,
                            new Point(random.Next(30, width - 30), random.Next(height)),
                            new Point(random.Next(30, width - 30), random.Next(height)));
                    }
                }
                return bitmap;
            }
        }

        //создание капчи
        public string CaptchaText;
        public void GenerateCaptcha()
        {
            CaptchaText = GenerateCaptchaText(4);
            var captchaimage = GenerateCaptchaImage(CaptchaText);
            CaptchaImage.Source = captchaimage;
        } 
        #endregion
        


        //ГОСТЕВОЙ РЕЖИМ
        private void GuestModeB_Click(object? sender, RoutedEventArgs e)
        {
            string captchatext = CaptchaTB.Text ?? string.Empty;

            if (string.IsNullOrWhiteSpace(captchatext))
            {
                new MessageWindow("Некорректный ввод", "Сначала введите капчу.").Show();
                return;
            }

            if (captchatext != CaptchaText)
            {
                new MessageWindow("Некорректный ввод", "Капча была введена неверно, попробуйте ещё раз").Show();
                GenerateCaptcha();
                CaptchaTB.Focus();
                return;
            }


            Settings.Default.failedattempts = 0;
            Settings.Default.Save();

            new MenuW().Show();
            this.Close();
        }
        


    }
}