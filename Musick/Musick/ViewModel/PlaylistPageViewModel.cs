using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Musick.Context;
using Musick.Models;
using Musick.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Musick.ViewModel
{
    class PlaylistPageViewModel : ObservableObject
    {


        private PlaylistPageViewList _selectedPlaylist;

        public PlaylistPageViewList SelectedPlaylist
        {
            get => _selectedPlaylist;

            set
            {
                _selectedPlaylist = value;
                if (SelectedPlaylist != null)
                WatchPlaylistInfo(int.Parse(_selectedPlaylist.ID));
                OnPropertyChanged();
                
            }
        }






        public PlaylistPageViewModel() 
        {
            LoadPlaylist();
        }




        public class PlaylistPageViewList
        {
            public string ID { get; set; }

            public string Name { get; set; }

            public int Duration { get; set; }

            public string DurationString { get; set; }

            public string Subscribers { get; set; }

            public string Likes { get; set; }

            public string CountTrack {  get; set; }

            public string DateCreated { get; set; }
        }



        public void WatchPlaylistInfo(int playlist_id)
        {
            PlaylistInfo w = new PlaylistInfo();
            w.DataContext = new PlaylistInfoViewModel(playlist_id);
            w.ShowDialog();
            //LoadPlaylist();
        }


        public ObservableCollection<PlaylistPageViewList> playlists {  get; set; }

        public void LoadPlaylist()
        {
            MusicContext context = new MusicContext();
            
          
            
            playlists = new ObservableCollection<PlaylistPageViewList>(context.Playlists
                .Include(x => x.User)
                .Include(x =>x.Tracks)
                .Select(x => new PlaylistPageViewList
                {
                   
                    ID =  x.PlaylistId.ToString(),
                    Name = $"{x.PlaylistName} | {x.User.UserLogin}",
                    Likes = x.Likes.ToString(),
                    CountTrack = "0",
                    DateCreated =  x.DateCreated.ToString("dd.MM.yyyy")


                }).OrderBy(x => x.ID)
                .ToList());

            

            foreach (var p in playlists)
            {
                p.Subscribers = $"Подписчиков: {context.Playlists.Include(x => x.Users)
                    .FirstOrDefault(x => x.PlaylistId == int.Parse(p.ID))
                    .Users
                    .Count().ToString()}";
                
                p.CountTrack = context.Playlists.Include(x => x.Tracks)
                    .FirstOrDefault(x => x.PlaylistId == int.Parse(p.ID))
                    .Tracks
                    .Count.ToString();

                p.Duration = context.Playlists.Include(x => x.Tracks).FirstOrDefault(x => x.PlaylistId == int.Parse(p.ID)).Tracks.Sum(x => x.Duration);
                p.DurationString = TimeSpan.FromSeconds(p.Duration).ToString(@"hh\:mm\:ss");
            }
                     
                
        }

        
       

    }
}
