using Musick.Models;
using Musick.Properties;
using Musick.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Musick.ViewModel
{
    class MenuViewModel : BaseViewsModel
    {

        private Page _currentPage = new TrackPage();

        public Page currentPage
        {
            get => _currentPage;
            set
            {
                _currentPage = value;
                OnPropertyChanged();
            }
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



        public ICommand LogoutCommand { get; }

        public ICommand PlaylistNavigateCommand { get; }

        public ICommand TrackNavigateCommand { get; }

        public ICommand AlbumNavigateCommand { get; }


        public MenuViewModel()
        {
            User user = (User)Application.Current.Properties["CurrentUser"];
            name = user.FullName;


            PlaylistNavigateCommand = new RelayCommand(PlaylistNav);
            TrackNavigateCommand = new RelayCommand(TrackNav);
            LogoutCommand = new RelayCommand(Logout);
            AlbumNavigateCommand = new RelayCommand(AlbumNav);
        }


        public void Logout()
        {
            Application.Current.Properties["CurrentUser"] = null;

            Settings.Default.Reset();

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




        public void PlaylistNav()
        {
            currentPage = new PlaylistPage();
        }

        public void TrackNav() { currentPage = new TrackPage(); }
        public void AlbumNav() { currentPage = new AlnumsPage(); }



    }
}
