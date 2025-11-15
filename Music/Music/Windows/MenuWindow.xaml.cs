using Microsoft.EntityFrameworkCore;
using Music.Context;
using Music.Models;
using Music.Properties;
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
        private DispatcherTimer timer = new();
        private TimeSpan time = TimeSpan.FromMinutes(60);

        private User _user;

        public MenuWindow()
        {
            InitializeComponent();
            _user = new() { Id = 0, FullName = "Гость" };
            Load();
        }

        public MenuWindow(User user)
        {
            InitializeComponent();
            _user = user;
            Load();
        }

        public async void Load()
        {
            Timer.Text = time.ToString(@"mm\:ss");
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();

            UserFullNameTextBlock.Text = _user.FullName;

            await using MusicContext context = new();

            PlaylistsListBox.ItemsSource = await context.Playlists.Include(x => x.Tracks).Include(x => x.Tags).Include(x => x.CreatorUser).Include(x => x.Users).ToListAsync();
            AlbumsListBox.ItemsSource = context.Albums
            .Include(x => x.Tracks)
            .Include(x => x.Artist)
            .Include(x => x.Genres)
            .ToList();
        }

        public void Timer_Tick(object? sender, EventArgs e)
        {
            time -= TimeSpan.FromSeconds(1);
            Timer.Text = time.ToString(@"mm\:ss");
            if (time == TimeSpan.FromMinutes(10))
            {
                MessageBox.Show("До конца сессии осталось 10 минут", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            if (time == TimeSpan.Zero)
            {
                timer.Stop();
                MessageBox.Show("Время сессии истекло", "Сеанс завершен", MessageBoxButton.OK, MessageBoxImage.Information);
                Application.Current.Properties["CurrentUser"] = null;
                Settings.Default.Reset();
                MainWindow mainWindow = new();
                mainWindow.Show();
                Close();
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Properties["CurrentUser"] = null;
            Settings.Default.Reset();
            MainWindow mainWindow = new();
            mainWindow.Show();
            Close();
        }
    }
}
