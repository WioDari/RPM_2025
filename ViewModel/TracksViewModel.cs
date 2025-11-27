using Microsoft.EntityFrameworkCore;
using MusicWpf.Context;
using MusicWpf.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicWpf.ViewModel
{
    public class TracksViewModel : BaseViewModel
    {
        private ObservableCollection<TracksVM> _tracks;
        public ObservableCollection<TracksVM> tracks
        {
            get => _tracks;
            set
            {
                _tracks = value;
                OnPropertyChanged();
            }
        }
        private string _image;
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

      

        public TracksViewModel(int id)
        {
            LoadTracks(id);
        }



        public string genre { get; set; }

        public class TracksVM
        {
            public string id { get; set; }
            public string countlisten { get; set; }


            public string tracknam { get; set; }

            public string artistrac { get; set; }


            public string rating { get; set; }

            public string duration { get; set; }

            public List<string> authors { get; set; }
        }

        private void LoadTracks(int? id = null) 
        {
            var context = new MusicContext();
            var album = context.Albums
                .Include(a => a.Artist)
                .FirstOrDefault(a => a.AlbumId == id);

            if (album != null)
            {
                Name = album.Artist?.ArtistName ?? string.Empty;
                namealbum = album.AlbumTitle;
                Image = album.CoverPath;
                genresses = context.AlbumGenres
                    .Include(ag => ag.Genre)
                    .Where(ag => ag.AlbumId == id)
                    .Select(ag => ag.Genre.GenreName)
                    .ToList();

                genre = genresses.Count > 0 ? string.Join(", ", genresses) : "";
            }

           
            var tracksData = context.AlbumTracks
                .Where(at => at.AlbumId == id)
                .Include(at => at.Tracks)
                    .ThenInclude(t => t.ArtistTracks)
                    .ThenInclude(at => at.Artist)
                .Select(at => new TracksVM
                {
                    tracknam = at.Tracks.TrackName,
                    countlisten = at.Tracks.PlayCount.ToString(),
                    authors = at.Tracks.ArtistTracks.Select(ta => ta.Artist.ArtistName).ToList(),
                    artistrac = "",
                    rating = at.Tracks.Rating.ToString(),
                    duration = at.Tracks.Duration.ToString(@"mm\:ss"),
                })
                .ToList();

            _tracks = new ObservableCollection<TracksVM>(tracksData);

            
            foreach (var track in _tracks)
            {
                track.artistrac = string.Join(" ", track.authors);
            }
            /*var context = new MusicContext();
            Name = context.Artists.Include(a => a.Albums.Where(a => a.AlbumId == id)).FirstOrDefault().ArtistName;
            namealbum = context.Albums.Where(a => a.AlbumId == id).FirstOrDefault().AlbumTitle;
            Image = context.Albums.Where(a => a.AlbumId == id).FirstOrDefault().CoverPath;
            genresses = context.AlbumGenres.Include(ag => ag.Genre).Where(ag => ag.AlbumId == id).Select(a => a.Genre.GenreName).ToList();


            if (genresses.Count == 0)
            {
                genre = "";
            }
            else
            {
                genre = genresses[0];
            }

            for (int i = 1; i < genresses.Count; i++)
            {
                genre += ", " + genresses[i];
            }
            _tracks = new ObservableCollection<TracksVM>(context.Tracks.Where(a => a.AlbumId == id)
                .Include(a => a.TrackArtists).Select(a => new TracksVM
                {
                    tracknam = a.TrackName,
                    countlisten = a.Playcount.ToString(),
                    authors = a.TrackArtists.Where(tr => tr.TrackId == a.TrackId).Select(a => a.Artist.ArtistName).ToList(),
                    artistrac = "",
                    rating = a.Rating.ToString(),
                    duration = a.Duration.ToString("mm:ss"),

                })
                .ToList());


            foreach (var track in tracks)
            {
                foreach (string t in track.authors)
                {

                    string ti = t;

                    track.artistrac += ti + " ";
                }  

            }*/

            /* var context = new MusicContext();
             _tracks = new ObservableCollection<TracksVM>(context.Tracks
                 .Include(a => a.ArtistTracks)
                 .ThenInclude(at => at.Artist)
                 .Select(a => new TracksVM
                 {
                     id = a.TracksId,
                     imageTrack = a.AlbumCoverPath,
                     nameTrack = a.TrackName,
                     artistTrack = string.Join(", ", a.ArtistTracks.Select(at => at.Artist.ArtistName)),
                     timeTrack = a.Duration,
                 })
                 .ToList());*/
        }

    }
}
