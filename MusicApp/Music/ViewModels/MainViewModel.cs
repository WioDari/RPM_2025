using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Xml.Linq;
using Music.Context;
using Music.Models;
using Music.Views;
using Music.Views.Items;
using Music.Properties;
using System.Windows.Controls;


namespace Music.ViewModels
{
    class MainViewModel : BaseViewModel
    {
     
        private Page _albumPage;
        public Page albumPage
        {
            get => _albumPage;
            set
            {
                _albumPage = value;
                OnPropertyChanged();
            }
        }
        private Page _playlistPage;
        public Page playlistPage
        {
            get => _playlistPage;
            set
            {
                _playlistPage = value;
                OnPropertyChanged();
            }
        }

        public class PlaylistView()
        {
            public string coverPath { get; set; }
            public string name { get; set; }
            public string likes { get; set; }
            public string numberOfSubscribers { get; set; }
            public string numberOfTrack { get; set; }
            public string creationDate { get; set; }
            public string totalDuration { get; set; }
        }
        public ICommand LogoutCommand { get; }

        public MainViewModel()
        {
            playlistPage = new Views.Items.AllPlaylistPage();
            albumPage = new Views.Items.AllAlbumPage();
           
            User user = (User)Application.Current.Properties["CurrentUser"];
            name = user.FullName;
            LogoutCommand = new RelayCommand(Logout);
        }
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
        


        public void Logout()
        {
            Application.Current.Properties["CurrentUser"] = null;
            
            Settings.Default.Reset();

            AuthWindow wnd = new AuthWindow();
            wnd.Show();
            foreach (Window w in Application.Current.Windows)
            {
                if (w is not Views.AuthWindow)
                {
                    w.Close();
                }
            }
        }
    }
}
