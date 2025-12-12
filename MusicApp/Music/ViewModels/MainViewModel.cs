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
using static Music.ViewModels.AlbumViewModel;
using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using System.IO;
using static Music.ViewModels.PlaylistViewModel;


namespace Music.ViewModels
{
    class MainViewModel : BaseViewModel
    {
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
        private string _selectedButton;
        public string SelectedButton
        {
            get => _selectedButton;
            set
            {
                _selectedButton = value;
                OnPropertyChanged();
            }
        }
        public ICommand LogoutCommand { get; }
        public ICommand MainCommand { get; }
        public ICommand TrackCommand { get; }
        public ICommand AlbumCommand { get; }
        public ICommand PlaylistCommand { get; }
        public MainViewModel()
        {
            MainCommand = new RelayCommand(MainNav);
            TrackCommand = new RelayCommand(TrackNav);
            AlbumCommand = new RelayCommand(AlbumNav);
            PlaylistCommand = new RelayCommand(PlaylistNav);
            MainNav();
            User user = (User)Application.Current.Properties["CurrentUser"];
            name = user.FullName;
            SelectedButton = "Main";
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
        public void MainNav()
        {
            currentPage = new Views.Items.MainPage();
            SelectedButton = "Main";
        }
        public void AlbumNav()
        {
            currentPage = new Views.Items.AllAlbumPage();
            SelectedButton = "Album";
        }
        public void PlaylistNav()
        {
            currentPage = new Views.Items.AllPlaylistPage();
            SelectedButton = "Playlist";
        }
        public void TrackNav()
        {
            currentPage = new Views.Items.AllTrackPage();
            SelectedButton = "Track";
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
