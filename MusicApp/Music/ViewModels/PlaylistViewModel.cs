using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Music.Context;
using Music.Models;

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
        public ObservableCollection<PlaylistView> data { get; set; }
        public void getPlaylist() {
        var context = new MusicDbContext();
            data = new ObservableCollection<PlaylistView>(context.Playlists
                   .Include(x => x.User)
                   .Select(x => new PlaylistView
                   {
                       id = x.PlaylistId,
                       name = x.PlaylistName,
                       creatorName = x.User.FullName,
                       creation_date = x.CreationDate.ToString("dd.MM.yyyy"),
                       likes = x.Likes.ToString(),
                       //subscribers = x.
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
