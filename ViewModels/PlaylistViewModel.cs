using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SpotApp_wpf.Context;
using SpotApp_wpf.Models;
using SpotApp_wpf.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static SpotApp_wpf.ViewModels.AlbumViewModel;

namespace SpotApp_wpf.ViewModels
{
    public class PlaylistViewModel : BaseViewModel
    {
        private ObservableCollection<PlTemplate> _playlists { get; set; }
        public ObservableCollection<PlTemplate> playlists 
        {
            get => _playlists;
            set
            {
                _playlists = value;
                OnPropertyChanged();
            }
        }

        public class PlTemplate
        {
            public string id { get; set; }
            public string title { get; set; }
            public string likes { get; set; }
            public string subs { get; set; }
            public string tracksCount { get; set; }
            public string creationDate { get; set; }
            public List<TimeSpan> timeA {  get; set; }
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

        public ICommand addPlaylistCommand { get; set; }

        public PlaylistViewModel()
        {
            LoadPlaylists();
            addPlaylistCommand = new RelayCommand(addPlaylist);
        }

        public void ShowDetails(int id)
        {
            var win = new Views.PlaylistDetails();
            if (selectedPlaylist != null)
            {
                win.DataContext = new PlaylistDetailsViewModel(id, this);
            }
            win.Show();
        }

        public void LoadPlaylists()
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
                    timeA = p.TracksInPlaylists.Where(p => p.PlaylistId == p.PlaylistId).Select(pt => pt.Track.Duration.ToTimeSpan()).ToList(),
                    time = "",
                })
                .OrderBy(p => p.id)
                .ToList());
            foreach (var item in playlists)
            {
                TimeSpan t = item.timeA[0];
                for (int i = 1; i < item.timeA.Count; i++)
                {
                    t = t.Add(item.timeA[i]);
                }
                item.time = t.ToString(@"hh\:mm\:ss");
            }
        }

        public void addPlaylist()
        {
            Window add = new PlaylistAddition();
            add.DataContext = new PlaylistAdditionViewModel(null, this);
            add.Show();
        }
    }
}
