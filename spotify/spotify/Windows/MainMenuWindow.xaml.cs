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
using spotify.Properties;
using spotify.Models;

namespace spotify.Windows
{
    /// <summary>
    /// Логика взаимодействия для MainMenuWindow.xaml
    /// </summary>
    public partial class MainMenuWindow : Window
    {
        User user1 = new();
        public MainMenuWindow()
        {
            InitializeComponent();
        }

        public MainMenuWindow(User user)
        {
            InitializeComponent();
            user1 = user;
            FullNameText.Text = user1.FullName;
        }

        public void Logout()
        {
            Application.Current.Properties["CurrentUser"] = null;
            Settings.Default.Reset();
            Window loginWindow = new LoginWindow();
            loginWindow.Show();
            foreach (Window window in Application.Current.Windows)
            {
                if (window is MainMenuWindow)
                {
                    window.Close();
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }

        private void NewAlbumButton_Click(object sender, RoutedEventArgs e)
        {
            NewAlbum newAlbum = new NewAlbum(user1);
            newAlbum.Show();
            this.Close();
        }

        private void NewTrackButton_Click(object sender, RoutedEventArgs e)
        {
            NewTrack newTrack = new NewTrack(user1);
            newTrack.Show();
            this.Close();
        }
    }
}
