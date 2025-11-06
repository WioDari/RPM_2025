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

namespace Practos2.ViewModel
{
    class EnterViewModel : BaseViewModel
    {
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
        public EnterViewModel()
        {
            LoginCommand = new RelayCommand(OnLogin);
        }

        public void OnLogin()
        {
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
    }
}
