using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using Microsoft.EntityFrameworkCore;
using Spotify_wpf.Context;
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


        public PlaylistViewModel()
        {
            LoadPlaylists();
        }



        public class PlaylistView
        {


            public string id { get; set; }
            public string likes { get; set; }
            public string datecreate { get; set; }
            public string track {  get; set; }

            public string sub {  get; set; }
            public string title { get; set; }

            public string time { get; set; }

            public string time1 { get; set; }

            public string time2 { get; set; }

            public TimeSpan times { get; set; }


        }

        private TimeSpan allDur;

        private string _dur;

        public string dur
        {
            get => _dur;

            set
            {
                _dur = value;
                OnPropertyChanged();
            }
        }

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
            var context = new MusicContext();
            //dur = context.PlayListTracks.Include(t => t.Track).FirstOrDefault().Track.Duration.ToString();
            _playlists = new ObservableCollection<PlaylistView>(context.Playlists
                .Include(p => p.PlayListTracks).ThenInclude(pt => pt.Track)
                .Include(p => p.PlaylistUsers)
                .Include(p => p.User)
                
                .Select(p => new PlaylistView
                {
                    id = p.PlayListId.ToString(),
                    title = $"{p.PlaylistName} | {p.User.FullName}",
                    sub = p.PlaylistUsers.Where(up => up.PlaylistId == p.PlayListId).Count().ToString(),
                    likes = $"Нравится: {p.Likes}",
                    track = p.PlayListTracks.Where(pt => pt.PlaylistId == p.PlayListId).Count().ToString(),
                    datecreate = p.DataCreate.ToString("dd.MM.yyy"),
                    time = TimeSpan.FromHours(p.PlayListTracks.Select(pt => pt.Track.Duration).Sum(d => d.Hour)).ToString(@"hh\:") + TimeSpan.FromMinutes(p.PlayListTracks.Select(pt => pt.Track.Duration).Sum(d => d.Minute)).ToString(@"mm\:") + TimeSpan.FromSeconds(p.PlayListTracks.Select(pt => pt.Track.Duration).Sum(d => d.Second)).ToString(@"ss"),
                    
                    

                })
                .ToList());

        }
    }
}
