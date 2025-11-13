using Music__.Commands;
using Music__.Context;
using Music__.Properties;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Music__.Views;

namespace Music__.ViewModels
{
    public class AuthorizationViewModel : BaseViewModel
    {
        private string _login = "antonina.pushmenkova";
        public string login
        {
            get => _login;
            set
            {
                _login = value;
                OnPropertyChanged();
            }
        }
        private string _password = "it3cySf5";
        public string password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }


        public ICommand LoginCommand { get; }
        public ICommand GuestModeCommand { get; }


        public AuthorizationViewModel()
        {
            LoginCommand = new RelayCommand(OnLogin);
            GuestModeCommand = new RelayCommand(OpenGuestMode);


            if (Settings.Default.userid != 0)
            {
                int userid = Settings.Default.userid;
                var context = new SpotifyContext();
                var user = context.Users.FirstOrDefault(x => x.Userid == userid);
                Application.Current.Properties["CurrentUser"] = user;


                new MainWindow().Show();
                foreach (Window w in Application.Current.Windows)
                {
                    if (w is not MainWindow)
                    {
                        w.Close();
                    }
                }
            }
        }


        public void OnLogin()
        {
            var context = new SpotifyContext();
            var user = context.Users.FirstOrDefault(x => x.Login == _login);

            if (user == null)
            {
                MessageBox.Show("Неверный логин", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                var pword = context.Users.FirstOrDefault(x => user.Password == _password);
                if (pword == null)
                {
                    MessageBox.Show("Неверный пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else
                {
                    Application.Current.Properties["CurrentUser"] = user;

                    new MainWindow().Show();
                    foreach (Window w in Application.Current.Windows)
                    {
                        if (w is not MainWindow)
                        {
                            w.Close();
                        }
                    }
                }
            }
            
            
        }


        public void OpenGuestMode()
        {
            Application.Current.Properties["CurrentUser"] = "Guest";

            new MainWindow().Show();
            foreach (Window w in Application.Current.Windows)
            {
                if (w is not MainWindow)
                {
                    w.Close();
                }
            }
        }
    }
}
