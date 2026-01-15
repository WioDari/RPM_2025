using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Music.Context;
using Music.Models;
using static Music.ViewModels.PlaylistViewModel;

namespace Music.ViewModels
{
    public class PlaylistInfoViewModel : BaseViewModel
    {
        private string _name;
        public string name
        {
            get => _name;
            set
            {
                _name = value;
            }
        }
        private string _creatorName;
        public string creatorName
        {
            get => _creatorName;
            set
            {
                _creatorName = value;
            }
        }
        private string _creation_date;
        public string creation_date
        {
            get => _creation_date;
            set
            {
                _creation_date = value;
            }
        }
        private string _likes;
        public string likes
        {
            get => _likes;
            set
            {
                _likes = value;
            }
        }
        private string _duration;
        public string duration
        {
            get => _duration;
            set
            {
                _duration = value;
            }
        }
        public ObservableCollection<TrackView> data { get; set; }

        private int _id;
        public PlaylistInfoViewModel(PlaylistView playlist) {
            likes = playlist.likes;
            duration = playlist.duration;
            creation_date = playlist.creation_date;
            creatorName = playlist.creatorName;
            name = playlist.name;
            _id = playlist.id;
            getTracks();
        }
       
       private void getTracks()
        {
            var context = new MusicDbContext();
            data = new ObservableCollection<TrackView>(context.Tracks
                    .Where(t => t.Playlists.Any(p => p.PlaylistId == _id))
                    .Select(x => new TrackView
                    {   playlists = x.Playlists.ToList(),
                        name = x.TrackName,
                        playCount = x.PlayCount.ToString(),
                        rating = x.Rating.ToString(),
                        durationMillis = x.Duration,
                        artists = x.Artists.ToList(),
                        albId = x.AlbumId,
                        albumName = x.Album.AlbumName
                    }).ToList());
            foreach (var d in data)
            {

                foreach (var a in d.artists)
                {
                    d.artistString += a.ArtistName + " ";
                }
                TimeSpan timeSpan = TimeSpan.FromSeconds(d.durationMillis);
                d.duration = $"{timeSpan:mm\\:ss}";

            }
        }
    }
}
