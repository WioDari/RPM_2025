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

        private string _title;
        public string title
        {
            get => _title;
            set
            {
                _title = value;
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
            //page = new Views.UsersPage();
            title = "Меню";
            LogoutCommand = new RelayCommand(Logout);
            ShowAlbumsCommand = new RelayCommand(ShowAlbumsPage);
            ShowPlaylistsCommand = new RelayCommand(ShowPlaylistsPage);
            ShowUsersCommand = new RelayCommand(ShowUsersPage);
        }

        public ICommand LogoutCommand { get; }
        public ICommand ShowAlbumsCommand { get; }
        public ICommand ShowUsersCommand { get; }
        public ICommand ShowPlaylistsCommand { get; }
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

        public void ShowAlbumsPage()
        {
            title = "Альбомы";
            page = new Views.AlbumsPage();
        }
        public void ShowPlaylistsPage()
        {
            title = "Плэйлисты";
            page = new Views.PlaylistPage();
        }
        public void ShowUsersPage()
        {
            title = "Таблица Пользователей";
            page = new Views.UsersPage();
        }
    }
}
