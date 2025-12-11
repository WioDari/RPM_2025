using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.EntityFrameworkCore;
using Musick.Context;
using Musick.Models;
using Musick.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Musick.ViewModel
{
    partial class PlaylistInfoViewModel : ObservableObject
    {

      

        [ObservableProperty]
        private string _iD;
        [ObservableProperty]
        private string _name;
        [ObservableProperty]
        private string _duration;
       
        [ObservableProperty]
        private string _likes;
        [ObservableProperty]
        private string _dateCreated;



        

        public PlaylistInfoViewModel(int? Playlist_id = null) 
        {
            MusicContext context = new MusicContext();
            if (Playlist_id == null)
            {
                MessageBox.Show("Ошибка загрузки плей листа", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else 
            {
                var playlist = context.Playlists.Include(x => x.User).FirstOrDefault(p => p.PlaylistId == Playlist_id);
                Name = $"{playlist.PlaylistName} | {playlist.User.UserLogin}";
                DateCreated = $"Дата создания: {playlist.DateCreated.ToString("dd.MM.yyyy")}";
                Likes = $"Лайки: {playlist.Likes.ToString()}";
                Duration = $"Продолжительность: {TimeSpan.FromSeconds(context.Playlists.Include(x => x.Tracks).FirstOrDefault(x => x.PlaylistId == playlist.PlaylistId).Tracks.Sum(x => x.Duration)).ToString(@"hh\:mm\:ss")}";
                LoadTracks(Playlist_id.GetValueOrDefault());
            }
        }


        public class TrackView()
        {
            public int ID { get; set; }
            public string TrackName { get; set; }

            public string Artist {  get; set; } = "";

            public List<Artist> artists { get; set; }

            public string AlbumNameOrCoutntAuditions { get; set; }

            public decimal Rating { get; set; }

            public string DurationString { get; set; }
        }

        public ObservableCollection<TrackView> TracksList { get; set; }

        public void LoadTracks(int Playlist_id)
        {
            MusicContext context = new MusicContext();

            

            TracksList = new ObservableCollection<TrackView>(context.Playlists.Include(x => x.Tracks).FirstOrDefault(x => x.PlaylistId == Playlist_id).Tracks
                .Select(x => new TrackView
                {
                    ID = x.TrackId,
                    TrackName = x.TrackName,
                   
                    Rating = x.Rating,
                    DurationString = TimeSpan.FromSeconds(x.Duration).ToString(@"hh\:mm\:ss")

                }).ToList());


            foreach (var track in TracksList)
            {
                var jjj = context.Tracks.Include(x => x.Album).Include(x => x.Artists).FirstOrDefault(x => x.TrackId == track.ID);

                track.AlbumNameOrCoutntAuditions =  $"Альбом\n{jjj.Album.AlbumName}";
                track.artists = jjj.Artists.ToList();

                foreach (var a in track.artists) {
                    track.Artist += a.ArtistName +" " ;
                }
            }
           

        }












    }
}
