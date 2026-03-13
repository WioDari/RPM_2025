using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using Spotify_wpf.Context;
using Spotify_wpf.Models;
using Spotify_wpf.Views;
using static Spotify_wpf.ViewModels.AlbumViewModel;

namespace Spotify_wpf.ViewModels
{
    class PlaylistViewModel :BaseViewModel
    {
        private ObservableCollection<PlaylistView> _playlists;

        public ObservableCollection<PlaylistView> playlists
        {
            get => _playlists;

            set
            {
                _playlists = value;
                OnPropertyChanged();
            }
        }

        private Visibility _addVisibility = Visibility.Collapsed;
        public Visibility AddVisibility
        {
            get => _addVisibility;
            set
            {
                _addVisibility = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddPlaylistCommand {  get;  }
      


        public PlaylistViewModel()
        {
            LoadPlaylists();
            AddPlaylistCommand = new RelayCommand(AddPlaylists);

        }

        public void AddPlaylists()
        {
            Window window = new AddPlaylist();
            window.Show();
        }



        public class PlaylistView
        {


            public string id { get; set; }
            public string likes { get; set; }
            public string datecreate { get; set; }
            public int track {  get; set; }

            public string sub {  get; set; }
            public string title { get; set; }

            public string time { get; set; }

            public string subscription { get; set; }

            // public TimeSpan durations { get; set; }
            //public TimeSpan times { get; set; }


        }

        /*private TimeSpan allDur;

        private string _dur;

        public string dur
        {
            get => _dur;

            set
            {
                _dur = value;
                OnPropertyChanged();
            }
        }*/

       /* private TimeSpan _duration;

        public TimeSpan duration
        {
            get => _duration;

            set
            {
                _duration = value;
                OnPropertyChanged();
            }
        }*/

        private PlaylistView _selectedPlaylist { get; set; }
        public PlaylistView selectedPlaylist
        {
            get => _selectedPlaylist;
            set
            {
                _selectedPlaylist = value;
                ViewPlaylist(int.Parse(_selectedPlaylist.id));
                OnPropertyChanged();
            }
        }

        public void ViewPlaylist(int id)
        {
            var w = new Views.PlaylistList();
            if (selectedPlaylist != null)
            {
                w.DataContext = new PlaylistListViewModel(id);
            }
            w.Show();


        }


        public void LoadPlaylists()
        {
            var currentUser = Application.Current.Properties["CurrentUser"] as User;
            var context = new MusicContext();
            //dur = context.PlayListTracks.Include(t => t.Track).FirstOrDefault().Track.Duration.ToString();
            _playlists = new ObservableCollection<PlaylistView>(context.Playlists
                .Include(p => p.PlayListTracks).ThenInclude(pt => pt.Track)
                .Include(p => p.PlaylistUsers)
                .Include(p => p.User)
                .ThenInclude(u => u.Subscription)

                .Select(p => new PlaylistView
                {
                    id = p.PlayListId.ToString(),
                    title = $"{p.PlaylistName} | {p.User.FullName}",
                    sub = p.PlaylistUsers.Where(up => up.PlaylistId == p.PlayListId).Count().ToString(),
                    likes = $"Нравится: {p.Likes}",
                    track = p.PlayListTracks.Where(pt => pt.PlaylistId == p.PlayListId).Count(),
                    datecreate = p.DataCreate.ToString("dd.MM.yyy"),
                    time = TimeSpan.FromHours(p.PlayListTracks.Select(pt => pt.Track.Duration).Sum(d => d.Hour)).ToString(@"hh\:") + TimeSpan.FromMinutes(p.PlayListTracks.Select(pt => pt.Track.Duration).Sum(d => d.Minute)).ToString(@"mm\:") + TimeSpan.FromSeconds(p.PlayListTracks.Select(pt => pt.Track.Duration).Sum(d => d.Second)).ToString(@"ss"),
                    subscription = p.User.Subscription.Subscription1
                    /* duration = 
                     durations = */
                    /* durations = new TimeSpan(p.PlayListTracks.Sum(pt => pt.Track.Duration.Ticks)),
                     time = new TimeSpan(p.PlayListTracks.Sum(pt => pt.Track.Duration.Ticks)).ToString(@"hh\:mm\:ss")*/


                })
                .ToList());

            if (currentUser != null)
            {
                if (currentUser.Role.Role1 == "Admin")
                {
                    AddVisibility = Visibility.Visible;
                   
                }
                else if (currentUser.Role.Role1 == "Manager")
                {

                    AddVisibility = Visibility.Visible;

                }
            }

            /*   allDur = new TimeSpan(_playlists.Sum(pl => pl.durations.Ticks));
               dur = $"Продолжительность плейлистов: {allDur:hh\\:mm\\:ss}";
   */

        }

       
    }
}
