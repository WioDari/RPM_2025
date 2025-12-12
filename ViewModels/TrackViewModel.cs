using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Spotify_wpf.Context;
using Spotify_wpf.Views;

namespace Spotify_wpf.ViewModels
{
    class TrackViewModel : BaseViewModel
    {
        private ObservableCollection<TrackView> _trackList;

        public ObservableCollection<TrackView> trackLists
        {
            get => _trackList;

            set
            {
                _trackList = value;
                OnPropertyChanged();
            }
        }
        
        public string _image;

        public string Image
        {
            get => _image;

            set
            {
                _image = value;
                OnPropertyChanged();
            }
        }

        private string _name;

        public string Name
        {
            get => _name;

            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }
        private string _namealbum;
        public string namealbum
        {
            get => _namealbum;

            set
            {
                _namealbum = value;
                OnPropertyChanged();
            }
        }

        public List<string> _genresses;

        public List<string> genresses
        {
            get => _genresses;

            set
            {
                _genresses = value;
                OnPropertyChanged();
            }
        }

        public TrackViewModel(int id)
        {
            LoadTrackList(id);
           
        }

        public string genre { get; set; } 

        public class TrackView
        {
            public string id { get; set; }
            public string countlisten { get; set; }


            public string tracknam { get; set; }

            public string artistrac { get; set; }


            public string rating { get; set; }

            public string duration { get; set; }

            public List<string> authors { get; set; }

            


        }


        public void LoadTrackList(int? id = null)
        {
            var context = new MusicContext();
            Name = context.Albums.Where(a => a.AlbumId == id).Select(a => a.Artist.ArtistName).FirstOrDefault();
            namealbum = context.Albums.Where (a => a.AlbumId == id).FirstOrDefault().AlbumName;
            Image = context.Albums.Where (a => a.AlbumId == id).FirstOrDefault().CoverPath;
            genresses = context.AlbumGenres.Include(ag => ag.Genre).Where(ag => ag.AlbumId == id).Select(a => a.Genre.GenreName).ToList();
            if (genresses.Count == 0)
            {
                genre = "";
            }
            else {
            genre = genresses[0];
            }

            for (int i= 1; i < genresses.Count; i++ )
            {
                genre += ", " + genresses[i];
            }
            _trackList = new ObservableCollection<TrackView>(context.Tracks.Where(a => a.AlbumId == id)
                .Include(a => a.TrackArtists)
                .Select(a => new TrackView
                {
                    tracknam = a.TrackName,
                    countlisten = a.Playcount.ToString(),
                    authors = a.TrackArtists.Where(tr => tr.TrackId == a.TrackId).Select(a => a.Artist.ArtistName).ToList(),
                    artistrac = "",
                    rating = a.Rating.ToString(),
                    duration = a.Duration.ToString("mm:ss"),

                })
                .ToList());


            foreach (var track in trackLists)
            {
                foreach (string t in track.authors)
                {
                    
                    string ti = t;
                    
                    track.artistrac += ti + " ";
                }

            }



        }

        /* public class AlbumView
     {


         public string nameartist { get; set; }
         public string namealbum { get; set; }

         public string imageLink { get; set; }

         public string nametrack { get; set; }

         public string genre { get; set; }


         }


         public void LoadAlbumsList()
         {
             var context = new MusicContext();
             _albumList = new ObservableCollection<AlbumView>(context.Albums
                 .Include(a => a.Tracks)
                 .Include(a => a.AlbumTracks)
                 .Include(a => a.Artist)
                 .Include(a => a.AlbumGenres)

                 .Select(a => new AlbumView
                 {
                     namealbum = a.AlbumName,
                     nameartist = a.Artist.ArtistName,
                     genre = a.AlbumGenres.ToString(),
                     imageLink = a.CoverPath == null ? $"/data/icon.png" : a.CoverPath


                 })
                 .ToList());



         }

     } */
    }

}
