using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SpotApp_wpf.Context;
using SpotApp_wpf.Models;
using SpotApp_wpf.Views;

namespace SpotApp_wpf.ViewModels
{
    public class AuthorizeViewModel : BaseViewModel
    {
        private string _login = "aleksey.novojilov";
        public string login
        {
            get => _login;
            set
            {
                _login = value;
            }
        }
        private string _password = "1XF4kEOq";
        public string password
        {
            get => _password;
            set
            {
                _password = value;
            }
        }

        public ICommand LoginCommand { get; }
        public ICommand OpenGuestCommand { get; }
        public AuthorizeViewModel()
        {
            LoginCommand = new RelayCommand(Login);
            OpenGuestCommand = new RelayCommand(OpenGuest);
        }

        public void OpenGuest()
        {
            Window g = new Views.GuestWindow();
            g.Show();
        }

        public void Login()
        {
            var context = new SpotifyContext();
            var user = context.Users.FirstOrDefault(u => u.UserLogin == login && u.UserPassword == password);
            if (user == null)
            {
                MessageBox.Show("Неверные данные");
                return;
            }
            MessageBox.Show($"Добро пожаловать, {user.FullName}");
            Application.Current.Properties["CurrentUser"] = user;

            Window menu = new Views.Menu();
            menu.Show();
            foreach (Window w in Application.Current.Windows)
            {
                if (w is not Views.Menu)
                {
                    w.Close();
                }
            }
        }
    }
}
