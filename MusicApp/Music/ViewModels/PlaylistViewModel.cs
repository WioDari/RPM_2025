using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Music.Context;
using Music.Models;
using Music.Views.Windows;
using static Music.ViewModels.AlbumViewModel;

namespace Music.ViewModels
{
    public class PlaylistViewModel : BaseViewModel
    {
        public class PlaylistView()
        {   public int id {  set; get; } 
            public string name { get; set; }
            public string creatorName { get; set; }
            public string creation_date { get; set; }
            public string likes { get; set; }
            public string subscribers { get; set; }
            public string numberOfTrack { get; set; }
            public string duration { get; set; }    
            public int durationInt {  get; set; }
        }
        public PlaylistViewModel() {
            getPlaylist();
        }
        private PlaylistView _SelectedPlaylist;
        public PlaylistView SelectedPlaylist
        {
            get => _SelectedPlaylist;
            set
            {
                _SelectedPlaylist = value;
                if (SelectedPlaylist != null)
                    showPlaylistInfo(value);
                OnPropertyChanged();
            }
        }
        private void showPlaylistInfo(PlaylistView playlistView) {
            var w = new PlaylistInfoWindow();
            w.DataContext = new PlaylistInfoViewModel(playlistView);    
            w.ShowDialog();
        }
        public ObservableCollection<PlaylistView> data { get; set; }
        public void getPlaylist() {
        var context = new MusicDbContext();
            data = new ObservableCollection<PlaylistView>(context.Playlists
                   .Include(x => x.User)
                   .Include(x => x.UserPlaylistSubscriptions)
                   .Select(x => new PlaylistView
                   {
                       id = x.PlaylistId,
                       name = x.PlaylistName,
                       creatorName = x.User.FullName,
                       creation_date = x.CreationDate.ToString("dd.MM.yyyy"),
                       subscribers = x.UserPlaylistSubscriptions.Count.ToString(),
                       likes = x.Likes.ToString(),
                       numberOfTrack = x.Tracks.Count.ToString()
                   }
                   ).OrderBy(x => x.id).ToList());
            foreach (var p in data)
            {
                p.durationInt = context.Playlists.Include(x => x.Tracks).FirstOrDefault(x => x.PlaylistId == p.id).Tracks.Sum(x => x.Duration);
                TimeSpan timeSpan = TimeSpan.FromSeconds(p.durationInt);
                p.duration = $"{timeSpan:hh\\:mm\\:ss}";
            }

        }

    }
    }
