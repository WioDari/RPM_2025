using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SpotApp_wpf.Context;
using SpotApp_wpf.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SpotApp_wpf.ViewModels.AlbumViewModel;

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
            public List<TimeOnly> timeA {  get; set; }
            public string time {  get; set; }
        }

        public List<TimeOnly> timet { get; set; }

        private PlTemplate _selectedPlaylist { get; set; }
        public PlTemplate selectedPlaylist
        {
            get => _selectedPlaylist;
            set
            {
                _selectedPlaylist = value;
                ShowDetails(int.Parse(selectedPlaylist.id));
                OnPropertyChanged();
            }
        }

        public PlaylistViewModel()
        {
            LoadPlaylists();
        }

        public void ShowDetails(int id)
        {
            var win = new Views.PlaylistDetails();
            if (selectedPlaylist != null)
            {
                win.DataContext = new PlaylistDetailsViewModel(id);
            }
            win.Show();
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
                    timeA = p.TracksInPlaylists.Where(p => p.PlaylistId == p.PlaylistId).Select(pt => pt.Track.Duration).ToList(),
                    time = TimeSpan.FromHours(p.TracksInPlaylists.Select(pt => pt.Track.Duration).Sum(d => d.Hour)).ToString(@"hh\:") + TimeSpan.FromMinutes(p.TracksInPlaylists.Select(pt => pt.Track.Duration).Sum(d => d.Minute)).ToString(@"mm\:") + TimeSpan.FromSeconds(p.TracksInPlaylists.Select(pt => pt.Track.Duration).Sum(d => d.Second)).ToString(@"ss"),
                })
                .OrderBy(p => p.id)
                .ToList());
            foreach (var item in playlists)
            {
                
            }
        }
    }
}
