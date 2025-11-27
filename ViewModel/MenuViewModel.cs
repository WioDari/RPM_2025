using MusicWpf.Models;
using MusicWpf.Properties;
using MusicWpf.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace MusicWpf.ViewModel
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
        public ICommand PlayList { get; }
        public ICommand AlbumList { get; }
        public ICommand LogoutCommand { get; }
        public ICommand UserList { get; }

        //public ICommand TrackList { get; }
        public MenuViewModel()
        {
            User user = (User)Application.Current.Properties["CurrentUser"];
            name = user.FullName;
            //page = new Views.PlaylistPage();
            LogoutCommand = new RelayCommand(Logout);
            PlayList = new RelayCommand(PlayLists);
            AlbumList = new RelayCommand(AlbumsLists);
            UserList = new RelayCommand(UsersLists);
            //TrackList = new RelayCommand(TracksLists);
        }
        public void Logout()
        {
            Application.Current.Properties["CurrentUser"] = null;
            Settings.Default.Reset();
            Settings.Default.Save();
            Window auth = new Auth();
            auth.Show();
            foreach (Window w in Application.Current.Windows)
            {
                if (w is not Auth)
                {
                    w.Close();
                }
            }

        }

        public void PlayLists()
        {
            page = new Views.PlaylistPage();
        }

        public void AlbumsLists()
        {
            page = new Views.AlbumPage();
        }

       public void UsersLists()
        {
            page = new Views.UserPage();
        }

       /* public void TracksLists()
        {
            page = new Views.TracksPage();
        }*/
    }


}

