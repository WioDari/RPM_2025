using DemoExam.Models;
using System;
using System.Windows;
using System.Windows.Input;
namespace DemoExam
{
    public partial class Menu : Window
    {
        private readonly User _currentUser;
        private readonly System.Timers.Timer _inactivityTimer;
        private readonly System.Timers.Timer _sessionTimer;
        public Menu(User currentUser = null)
        {
            InitializeComponent();
            _currentUser = currentUser;
            if (_currentUser != null)
            {
                txtUserInfo.Text = $"{_currentUser.FullName} ({GetRoleName(_currentUser.RoleId)})";
                Title = $"Меню — {_currentUser.FullName}";
            }
            else
            {
                txtUserInfo.Text = "Гость";
                Title = "Меню — Гость";
            }
            UpdateUIByRole();
        }
        private string GetRoleName(int roleId)
        {
            return roleId switch
            {
                1 => "Админ",
                2 => "Менеджер",
                3 => "Пользователь",
                _ => "Неизвестно"
            };
        }
        private void UpdateUIByRole()
        {
            if (_currentUser == null)
            {
                btnSearch.IsEnabled = false;
                btnAdmin.Visibility = Visibility.Collapsed;
            }
            else
            {
                btnSearch.IsEnabled = true;
                btnAdmin.Visibility = (_currentUser.RoleId == 1) ? Visibility.Visible : Visibility.Collapsed;
            }
        }
        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите выйти?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Logout();
            }
        }
        private void Logout()
        {
            _inactivityTimer?.Stop();
            _sessionTimer?.Stop();

            var loginWindow = new Aurh();
            loginWindow.Show();
            this.Close();
        }
        private void BtnAlbums_Click(object sender, RoutedEventArgs e) => MessageBox.Show("Открыть список альбомов");
        private void BtnPlaylists_Click(object sender, RoutedEventArgs e) => MessageBox.Show("Открыть список плейлистов");
        private void BtnSearch_Click(object sender, RoutedEventArgs e) => MessageBox.Show("Открыть поиск и фильтрацию");
        private void BtnAdmin_Click(object sender, RoutedEventArgs e) => MessageBox.Show("Открыть панель управления");
    }
}