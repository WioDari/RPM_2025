using Microsoft.EntityFrameworkCore;
using MusicWpf.Context;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicWpf.ViewModel
{
    class AlbumViewModel_track_ : BaseViewModel
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



        public AlbumViewModel_track_(int id)
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
           
        }

    }
}
