using Musick.Models;
using Musick.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Musick.ViewModel
{
    class MenuViewModel : BaseViewsModel
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



        public ICommand LogoutCommand { get; }

        public MenuViewModel()
        {
            User user = (User)Application.Current.Properties["CurrentUser"];
            name = user.FullName;


            LogoutCommand = new RelayCommand(Logout);
        }


        public void Logout()
        {
            Application.Current.Properties["CurrentUser"] = null;
            Auth wnd = new Auth();
            wnd.Show();
            foreach (Window w in Application.Current.Windows)
            {
                if (w is not Views.Auth)
                {
                    w.Close();
                }
            }
        }


    }
}
