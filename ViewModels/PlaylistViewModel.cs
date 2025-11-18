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
            
         
            public string likes { get; set; }
            public string datecreate { get; set; }
            public string track {  get; set; }

            public string sub {  get; set; }
            public string title { get; set; }

            public string time { get; set; }


        }


        public void LoadPlaylists()
        {
            var context = new MusicContext();
            _playlists = new ObservableCollection<PlaylistView>(context.Playlists
                .Include(p => p.PlayListTracks)
                .Include(p => p.PlaylistUsers)
                .Include(p => p.User)
                .Select(p => new PlaylistView
                {
                    title = $"{p.PlaylistName} | {p.User.FullName}",
                    sub = p.PlaylistUsers.Where(up => up.PlaylistId == p.PlayListId).Count().ToString(),
                    likes = $"Нравится: {p.Likes}",
                    track = p.PlayListTracks.Where(pt => pt.PlaylistId == p.PlayListId).Count().ToString(),
                    datecreate = p.DataCreate.ToString("dd.MM.yyy"),
                    time = "0:00"
                })
                .ToList());

        }
    }
}
