using SpotApp_wpf.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SpotApp_wpf.ViewModels
{
    public class MenuViewModel : BaseViewModel
    {
        private string _name;
        public string name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public MenuViewModel()
        {

            User user = (User)Application.Current.Properties["CurrentUser"];
            name = user.FullName;
            LogoutCommand = new RelayCommand(Logout);
        }

        public ICommand LogoutCommand { get; }
        public void Logout()
        {
            Application.Current.Properties["CurrentUser"] = null;
            Window auth = new Views.Authorize();
            auth.Show();
            foreach (Window w in Application.Current.Windows)
            {
                if (w is not Views.Authorize)
                {
                    w.Close();
                }
            }
        }
    }
}
