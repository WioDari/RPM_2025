using Musick.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Musick.ViewModel
{
    internal class MainViewModel :BaseViewsModel
    {

        private string _AllAlbums {  get; set; }

        public string AllAlbums { 
            get => _AllAlbums; 
            set 
            { 
            _AllAlbums = value;
            OnPropertyChanged();
            
            } 
        }

        private string _AllTrack { get; set; }

        public string AllTrack
        {
            get => _AllTrack;
            set
            {
                _AllTrack = value;
                OnPropertyChanged();

            }
        }

        private string _AllPlay { get; set; }

        public string AllPlay
        {
            get => _AllPlay;
            set
            {
                _AllPlay = value;
                OnPropertyChanged();

            }
        }


        public MainViewModel() 
        {
            MusicContext context = new MusicContext();
            var AllAlbumss = context.Albums.OrderBy(x => x.AlbumId).Select(x => x.AlbumId).LastOrDefault();
            var AllTrackss = context.Tracks.OrderBy(x => x.TrackId).Select(x => x.TrackId).LastOrDefault();
            var AllPlaylistt = context.Playlists.OrderBy(x => x.PlaylistId).Select(x => x.PlaylistId).LastOrDefault();

            _AllAlbums = $"Альбомов: {AllAlbumss}";
            _AllTrack = $"Треков {AllTrackss}";
            _AllPlay = $"Плейлистов {AllPlaylistt}";
        }


    }
}
