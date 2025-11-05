using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Spotify_wpf.Context;
using Spotify_wpf.ViewModels;
using Spotify_wpf.Views;

namespace Spotify_wpf
{
    class AuthViewModel : BaseViewModel
    {
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

        public ICommand AuthCommand { get; }

        public AuthViewModel()
        {
            AuthCommand = new RelayCommand(OnAuth);
        }
        public void OnAuth()
        {
            var context = new MusicContext();
            var user = context.Users.FirstOrDefault(u => u.UserLogin == login && u.UserPassword == password);
            if (user == null)
            {
                MessageBox.Show("Пароль введен неверно");
                return;
            }
            MessageBox.Show($"Добро пожаловать, {user.FullName}");
            Application.Current.Properties["CurrentUser"] = user;
            Window menu = new Menu();
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
