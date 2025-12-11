using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.EntityFrameworkCore;
using Musick.Context;
using Musick.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Musick.ViewModel
{
    internal class TrackPageViewModel : ObservableObject
    {


        public TrackPageViewModel()
        {
            LoadTracks();
        }

        public class TrackView()
        {
            public int ID { get; set; }
            public string TrackName { get; set; }

            public string Artist { get; set; } = "";

            public List<Artist> artists { get; set; }

            public string AlbumNameOrCoutntAuditions { get; set; }

            public decimal Rating { get; set; }

            public string DurationString { get; set; }
        }

        public ObservableCollection<TrackView> TracksList { get; set; }

        public void LoadTracks()
        {
            MusicContext context = new MusicContext();



            TracksList = new ObservableCollection<TrackView>(context.Tracks
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

                track.AlbumNameOrCoutntAuditions = $"Альбом\n{jjj.Album.AlbumName}";
                track.artists = jjj.Artists.ToList();

                foreach (var a in track.artists)
                {
                    track.Artist += a.ArtistName + " ";
                }
            }


        }


    }
}
