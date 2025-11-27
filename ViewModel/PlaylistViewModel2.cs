using Microsoft.EntityFrameworkCore;
using MusicWpf.Context;
using MusicWpf.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicWpf.ViewModel
{
    internal class PlaylistViewModel2 : BaseViewModel
    {

        private ObservableCollection<PlaylistViewModel2> _playlistList;

        public ObservableCollection<PlaylistViewModel2> playlistList
        {
            get => _playlistList;

            set
            {
                _playlistList = value;
                OnPropertyChanged();
            }
        }
        private string _nameplaylist;

        public string nameplaylist
        {
            get => _nameplaylist;

            set
            {
                _nameplaylist = value;
                OnPropertyChanged();
            }
        }

        private string _nameavtor;

        public string nameavtor
        {
            get => _nameavtor;

            set
            {
                _nameavtor = value;
                OnPropertyChanged();
            }
        }

        private string _date;

        public string date
        {
            get => _date;

            set
            {
                _date = value;
                OnPropertyChanged();
            }
        }


        private string _like;

        public string like
        {
            get => _like;

            set
            {
                _like = value;
                OnPropertyChanged();
            }
        }


        private string _duration;

        public string duration
        {
            get => _duration;

            set
            {
                _duration = value;
                OnPropertyChanged();
            }
        }

        public PlaylistViewModel2(int id)
        {
            LoadPlaylist2(id);
        }
        public class PlaylistListView
        {
            public string id { get; set; }

            public string namealbum { get; set; }


            public string tracknam { get; set; }

            public string artistrac { get; set; }


            public string rating { get; set; }

            public string duration { get; set; }

            public List<string> authors { get; set; }

        }

        public void LoadPlaylist2(int? id = null)
        {
            var context = new MusicContext();
            nameavtor = context.Playlists.Include(p => p.User).FirstOrDefault().User.FullName;
            nameplaylist = $"{context.Playlists.Where(p => p.PlaylistId == id).FirstOrDefault().PlaylistName} | {nameavtor}";
            date = $"Дата создания: {context.Playlists.Where(p => p.PlaylistId == id).FirstOrDefault().DateCreated.ToString("dd.MM.yyyy")}";
            like = $"Понравилось: {context.Playlists.Where(p => p.PlaylistId == id).FirstOrDefault().Likes.ToString()}";
            duration = $"Продолжительность: {0.ToString()}";

                /*  _playlistList = new ObservableCollection<PlaylistListView>(context.Tracks
                       .Where(t => t.TracksPlaylists.Any(pt => pt.PlaylistId == id))
                       .Include(p => p.Album)
                       .Include(p => p.PlayListTracks)
                       .Include(p => p.TrackArtists)
                       .Include(p => p.AlbumTracks)
                       .Select(p => new PlaylistListView
                       {
                           tracknam = p.TrackName,
                           namealbum = p.Album.AlbumName,
                           authors = p.TrackArtists.Where(tr => tr.TrackId == p.TrackId).Select(a => a.Artist.ArtistName).ToList(),
                           artistrac = "",
                           rating = p.Rating.ToString(),
                           duration = p.Duration.ToString("mm:ss"),

                       })
                       .ToList());
*/
            /*  foreach (var track in playlistList)
              {
                  foreach (string t in track.authors)
                  {

                      string ti = t;

                      track.artistrac += ti + " ";
                  }

              }*/


        }
    }
}
