using Microsoft.EntityFrameworkCore;
using Music.Context;
using Music.Models;
using Music.Properties;
using Music.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Music.Windows
{
    /// <summary>
    /// Логика взаимодействия для MenuWindow.xaml
    /// </summary>
    public partial class MenuWindow : Window
    {
        public AlbumsUserControl AlbumsUC = null!;
        public PlaylistsUserControl PlaylistsUC = null!;
        private UsersUserControl UsersUC = null!;

        private DispatcherTimer timer = new();
        private TimeSpan time = TimeSpan.FromMinutes(60);

        private DispatcherTimer afkTimer = new();
        private TimeSpan afkTime = TimeSpan.FromMinutes(5);

        public User User;

        public MenuWindow()
        {
            InitializeComponent();
            User = new() { Id = 0, FullName = "Гость", RoleId = 2 };
            LoadGuest();
        }

        public MenuWindow(User user)
        {
            InitializeComponent();
            User = user;
            Load();
        }

        private void Load()
        {
            AlbumsUC = new AlbumsUserControl(this, false);
            PlaylistsUC = new PlaylistsUserControl(this, false);
            Albums.Content = AlbumsUC;
            Playlists.Content = PlaylistsUC;
            Users.Content = UsersUC;

            Timer.Text = time.ToString(@"mm\:ss");
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();

            afkTimer.Tick += (_, _) => AfkTimer_Tick();
            afkTimer.Interval = TimeSpan.FromSeconds(1);

            InputManager.Current.PostProcessInput += GlobalInputHandler;

            afkTimer.Start();


            UserFullNameTextBlock.Text = User.FullName;
            UserRoleTextBlock.Text = User.Role.Name;

            switch (User.RoleId)
            {
                case 4: // user
                    NewPlaylistButton.Visibility = Visibility.Hidden;
                    NewAlbumButton.Visibility = Visibility.Hidden;
                    UsersTabItem.Visibility = Visibility.Hidden;
                    break;
                case 3: // manager
                    NewAlbumButton.Visibility = Visibility.Hidden;
                    UsersTabItem.Visibility = Visibility.Hidden;
                    break;
                case 1: // admin
                    UsersUC = new UsersUserControl(this);
                    Users.Content = UsersUC;
                    break;

            }
        }

        private void GlobalInputHandler(object sender, ProcessInputEventArgs e)
        {
            if (e.StagingItem.Input is MouseEventArgs || e.StagingItem.Input is KeyEventArgs)
            {
                afkTime = TimeSpan.FromMinutes(5);
            }
        }


        private void LoadGuest()
        {
            AlbumsUC = new AlbumsUserControl(this, true);
            PlaylistsUC = new PlaylistsUserControl(this, true);
            Albums.Content = AlbumsUC;
            Playlists.Content = PlaylistsUC;

            Timer.Visibility = Visibility.Hidden;
            time = TimeSpan.FromMinutes(5);
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += TimerGuest_Tick;
            timer.Start();

            UserFullNameTextBlock.Text = User.FullName;

            UserRoleTextBlock.Visibility = Visibility.Hidden;

            NewPlaylistButton.Visibility = Visibility.Hidden;
            NewAlbumButton.Visibility = Visibility.Hidden;
            UsersTabItem.Visibility = Visibility.Hidden;
        }

        private async void TimerGuest_Tick(object? sender, EventArgs e)
        {
            time -= TimeSpan.FromSeconds(1);
            if (time == TimeSpan.Zero)
            {
                timer.Stop();

                var result = MessageBox.Show(this, "Для авторизованых пользователей доступен больший функционал. Открыть окно авторизации?", "Авторизация", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    MainWindow mainWindow = new();
                    mainWindow.Show();
                    Close();
                }
                else
                {
                    time = TimeSpan.FromMinutes(5);
                    timer.Start();
                }
            }
        }

        private async void Timer_Tick(object? sender, EventArgs e)
        {
            time -= TimeSpan.FromSeconds(1);
            Timer.Text = time.ToString(@"mm\:ss");
            if (time == TimeSpan.FromMinutes(10))
            {
                await Task.Run(() =>
                    MessageBox.Show("До конца сессии осталось 10 минут", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning));
            }
            if (time == TimeSpan.Zero)
            {
                timer.Stop();

                Application.Current.Properties["CurrentUser"] = null;
                Settings.Default.Reset();

                InputManager.Current.PostProcessInput -= GlobalInputHandler;

                MainWindow mainWindow = new();
                mainWindow.Show();
                Close();

                await Task.Run(() => MessageBox.Show("Время сессии истекло.", "Сеанс завершен", MessageBoxButton.OK, MessageBoxImage.Information));
            }
        }

        private async void AfkTimer_Tick()
        {
            afkTime -= TimeSpan.FromSeconds(1);
            if (afkTime == TimeSpan.Zero)
            {
                afkTimer.Stop();
                InputManager.Current.PostProcessInput -= GlobalInputHandler;

                Application.Current.Properties["CurrentUser"] = null;
                Settings.Default.Reset();

                MainWindow mainWindow = new();
                mainWindow.Show();
                Close();

                await Task.Run(() => MessageBox.Show("Приложение завершило работу после 5 минут бездействия.", "Сеанс завершен", MessageBoxButton.OK, MessageBoxImage.Information));
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Properties["CurrentUser"] = null;
            Settings.Default.Reset();
            timer.Stop();
            afkTimer.Stop();
            InputManager.Current.PostProcessInput -= GlobalInputHandler;
            MainWindow mainWindow = new();
            mainWindow.Show();
            Close();
        }


        private void NewAlbumButton_Click(object sender, RoutedEventArgs e)
        {
            var addAlbumWindow = new AddAlbumWindow() { Owner = this };
            addAlbumWindow.ShowDialog();
            Albums.Content = new AlbumsUserControl(this, false);
        }

        private void NewPlaylistButton_Click(object sender, RoutedEventArgs e)
        {
            new AddPlaylistWindow(User) { Owner = this }.ShowDialog();
            Playlists.Content = new PlaylistsUserControl(this, false);
        }
    }
}
