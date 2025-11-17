using Microsoft.EntityFrameworkCore;
using SpotApp_wpf.Context;
using SpotApp_wpf.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotApp_wpf.ViewModels
{
    public class PlaylistViewModel : BaseViewModel
    {
        public ObservableCollection<PlTemplate> playlists {  get; set; }

        public class PlTemplate
        {
            public string id { get; set; }
            public string title { get; set; }
            public string likes { get; set; }
            public string subs { get; set; }
            public string tracksCount { get; set; }
            public string creationDate { get; set; }
            public string time {  get; set; }
        }

        public PlaylistViewModel()
        {
            LoadPlaylists();
        }

        private void LoadPlaylists()
        {
            var context = new SpotifyContext();
            playlists = new ObservableCollection<PlTemplate>(context.Playlists
                .Include(p => p.User)
                .Include(p => p.UsersPlaylists)
                .Include(t => t.TracksInPlaylists)
                .Select(p => new PlTemplate
                {
                    id = p.PlaylistId.ToString(),
                    title = $"{p.PlaylistName} | {p.User.FullName}",
                    likes = p.Likes.ToString(),
                    subs = p.UsersPlaylists.Where(up => up.PlaylistId == p.PlaylistId).Count().ToString(),
                    creationDate = p.DateCreated.ToString("dd.MM.yyy"),
                    tracksCount = p.TracksInPlaylists.Where(tp => tp.PlaylistId == p.PlaylistId).Count().ToString(),
                    time = "0:00"
                })
                .OrderBy(p => p.id)
                .ToList());
        }
    }
}
