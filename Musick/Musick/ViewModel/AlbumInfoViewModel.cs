using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.EntityFrameworkCore;
using Musick.Context;
using Musick.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Musick.ViewModel
{
    public partial class AlbumInfoViewModel : ObservableObject
    {
        [ObservableProperty]
        public string _name;
        [ObservableProperty]
        public string _artist;
        [ObservableProperty]
        public string _genres;
        [ObservableProperty]
        public string _photo;


        public AlbumInfoViewModel(int? Album_id = null)
        {
            MusicContext context = new MusicContext();

            var CurAlbum = context.Albums.Include(x => x.Artist).Include(x => x.Genres).FirstOrDefault(x => x.AlbumId == Album_id);


            Name = CurAlbum.AlbumName;
            Artist = $"Артист: {CurAlbum.Artist.ArtistName}";
            foreach (var el in CurAlbum.Genres)
            {
                Genres += el.GenresName.ToString() + "/";
            }
            if (File.Exists($"C:\\Users\\DODATKINS\\Desktop\\Musick\\Musick\\Resources\\covers\\{CurAlbum.AlbumName}.jpg") == true)
            {
                Photo = $"/Resources/covers/{CurAlbum.AlbumName}.jpg";
            }
            else
            {
                Photo = "/Resources/Flow.png";
            }

            LoadTracks(Album_id.GetValueOrDefault());
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

        public void LoadTracks(int Album_ID)
        {
            MusicContext context = new MusicContext();



            TracksList = new ObservableCollection<TrackView>(context.Albums.Include(x => x.Tracks).FirstOrDefault(x => x.AlbumId == Album_ID).Tracks
                .Select(x => new TrackView
                {
                    ID = x.TrackId,
                    TrackName = x.TrackName,
                    AlbumNameOrCoutntAuditions = $"Прослушиваний\n {x.PlayCount.ToString()}",
                    Rating = x.Rating,
                    DurationString = TimeSpan.FromSeconds(x.Duration).ToString(@"hh\:mm\:ss")

                }).ToList());


            foreach (var track in TracksList)
            {
                var jjj = context.Tracks.Include(x => x.Album).Include(x => x.Artists).FirstOrDefault(x => x.TrackId == track.ID);

                
                track.artists = jjj.Artists.ToList();

                foreach (var a in track.artists)
                {
                    track.Artist += a.ArtistName + " ";
                }
            }


        }




        
    }


    
}
