using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace musicapp
{
    public partial class MainWindow : Window
    {
        
        private static readonly (string login, string password, string fullName)[] Users = {
            ("admin", "123", "admin"),
            ("user", "456", "user")
        };

        private string currentCaptcha = "";
        private bool isCaptchaCorrect = false;
        private string currentUserFullName = "";

        
        public MainWindow()
        {
            InitializeComponent();
            
            LoginGrid.Visibility = Visibility.Visible;
            MainGrid.Visibility = Visibility.Collapsed;
            Title = "Авторизация";
            GenerateCaptcha();
        }

        
        public MainWindow(string fullName) : this()
        {
            currentUserFullName = fullName;
            Loaded += (s, e) =>
            {
                LoginGrid.Visibility = Visibility.Collapsed;
                MainGrid.Visibility = Visibility.Visible;
                WelcomeText.Text = "Привет, " + currentUserFullName + "!";
            };
        }

        private void GenerateCaptcha()
        {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random rand = new Random();
            currentCaptcha = "";
            for (int i = 0; i < 5; i++)
            {
                currentCaptcha += chars[rand.Next(chars.Length)];
            }

            CaptchaImage.Source = CreateCaptchaImage(currentCaptcha);
            CaptchaBox.Clear();
            isCaptchaCorrect = false;
            LoginButton.IsEnabled = false;
        }

        private ImageSource CreateCaptchaImage(string text)
        {
            int width = 180;
            int height = 60;

            DrawingVisual drawingVisual = new DrawingVisual();
            using (DrawingContext dc = drawingVisual.RenderOpen())
            {
                dc.DrawRectangle(Brushes.White, null, new Rect(0, 0, width, height));
                double x = 15;
                double y = 10;
                foreach (char c in text)
                {
                    FormattedText ft = new FormattedText(
                        c.ToString(),
                        System.Globalization.CultureInfo.CurrentCulture,
                        FlowDirection.LeftToRight,
                        new Typeface("Consolas"),
                        32,
                        Brushes.Black);
                    dc.DrawText(ft, new Point(x, y));
                    x += 35;
                }
            }

            RenderTargetBitmap bitmap = new RenderTargetBitmap(width, height, 96, 96, PixelFormats.Pbgra32);
            bitmap.Render(drawingVisual);
            return bitmap;
        }

        private void RefreshCaptcha_Click(object sender, RoutedEventArgs e)
        {
            GenerateCaptcha();
        }

        private void VerifyCaptcha_Click(object sender, RoutedEventArgs e)
        {
            string input = CaptchaBox.Text.Trim().ToUpper();
            if (string.IsNullOrEmpty(input))
            {
                MessageBox.Show("Пожалуйста, введите капчу.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (input == currentCaptcha)
            {
                isCaptchaCorrect = true;
                LoginButton.IsEnabled = true;
                MessageBox.Show("Капча верна! Теперь можно войти.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Неверная капча.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                GenerateCaptcha();
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (!isCaptchaCorrect)
            {
                MessageBox.Show("Сначала подтвердите капчу!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string login = LoginBox.Text.Trim();
            string password = PasswordBox.Password;

            foreach (var user in Users)
            {
                if (user.login == login && user.password == password)
                {
                    MainWindow mainWin = new MainWindow(user.fullName);
                    mainWin.Show();
                    this.Close();
                    return;
                }
            }

            MessageBox.Show("Неверный логин или пароль.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void GuestButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow guestWin = new MainWindow("Гость");
            guestWin.Show();
            this.Close();
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow loginWin = new MainWindow();
            loginWin.Show();
            this.Close();
        }
    }
}