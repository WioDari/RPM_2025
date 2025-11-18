using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Spotify_wpf;
using Spotify_wpf.Models;
using Spotify_wpf.Properties;
using Spotify_wpf.Views;


namespace Spotify_wpf.ViewModels
{
    class MenuViewModel : BaseViewModel
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

        private Page _currentPage;

        public Page currentPage
        {
            get => _currentPage;
            set
            {
                _currentPage = value;
                OnPropertyChanged();

            }
        }

        public MenuViewModel()
        {
            User user = (User)Application.Current.Properties["CurrentUser"];
            name = user.FullName;
            LogoutCommand = new RelayCommand(Logout);
            GoPlaylist = new RelayCommand(GoPlaylists);
            GoAlbum = new RelayCommand(GoAlbums);
            GoUser = new RelayCommand(GoUsers);


        }

        public ICommand LogoutCommand { get; }

        public ICommand GoPlaylist {  get; }
        public ICommand GoAlbum { get; }

        public ICommand GoUser { get; } 

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
        public void GoPlaylists()
        {
            currentPage = new Views.Playlist();
        }

        public void GoAlbums()
        {
            currentPage = new Views.Album();
        }

        public void GoUsers()
        {
            currentPage = new Views.UserList();
        }


    }




}
