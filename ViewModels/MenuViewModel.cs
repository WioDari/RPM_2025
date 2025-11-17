using SpotApp_wpf.Models;
using SpotApp_wpf.Views;
using SpotApp_wpf.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;

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

        private Page _page;
        public Page page
        {
            get => _page;
            set
            {
                _page = value;
                OnPropertyChanged();
            }
        }

        public MenuViewModel()
        {

            User user = (User)Application.Current.Properties["CurrentUser"];
            name = user.FullName;
            page = new Views.PlaylistPage();
            LogoutCommand = new RelayCommand(Logout);
        }

        public ICommand LogoutCommand { get; }
        public void Logout()
        {
            Application.Current.Properties["CurrentUser"] = null;
            Settings.Default.Reset();

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
