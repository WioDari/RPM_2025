using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input.Manipulations;
using Microsoft.EntityFrameworkCore;
using Spotify_wpf.Context;
using static Spotify_wpf.ViewModels.TrackViewModel;

namespace Spotify_wpf.ViewModels
{
    internal class PlaylistListViewModel :BaseViewModel
    {
        private ObservableCollection<PlaylistListView> _playlistList;

        public ObservableCollection<PlaylistListView> playlistList
        {
            get => _playlistList;

            set
            {
                _playlistList = value;
                OnPropertyChanged();
            }
        }
        private string _title;

        public string title
        {
            get => _title;

            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        private string _author;

        public string author
        {
            get => _author;

            set
            {
                _author = value;
                OnPropertyChanged();
            }
        }

        private string _datecreate;

        public string datecreate
        {
            get => _datecreate;

            set
            {
                _datecreate = value;
                OnPropertyChanged();
            }
        }


        private string _likes;

        public string likes
        {
            get => _likes;

            set
            {
                _likes = value;
                OnPropertyChanged();
            }
        }


        private string _dur;

        public string dur
        {
            get => _dur;

            set
            {
                _dur = value;
                OnPropertyChanged();
            }
        }

        public TimeSpan allDur;

        public PlaylistListViewModel(int id)
        {
            LoadPlaylistList(id);
        }

        public class PlaylistListView
        {
            public string id { get; set; }

            public string namealbum { get; set; }

           
            public string tracknam { get; set; }

            public string artistrac { get; set; }


            public string rating { get; set; }

            public string duration { get; set; }

            public TimeSpan durations { get; set; }

            public List<string> authors { get; set; }

        }

        public void LoadPlaylistList(int? id = null)
        {
            var context = new MusicContext();
            author = context.Playlists.Include(p => p.User).FirstOrDefault().User.FullName;
            title = $"{context.Playlists.Where(p => p.PlayListId == id).FirstOrDefault().PlaylistName} | {author}";
            datecreate = $"Дата создания: {context.Playlists.Where(p => p.PlayListId == id).FirstOrDefault().DataCreate.ToString("dd.MM.yyyy")}";
            likes = $"Понравилось: {context.Playlists.Where(p => p.PlayListId == id).FirstOrDefault().Likes.ToString()}";


            _playlistList = new ObservableCollection<PlaylistListView>(context.Tracks
                .Where(t => t.PlayListTracks.Any(pt => pt.PlaylistId == id))
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
                    durations = p.Duration.ToTimeSpan()

                })
                .ToList());

            
            
            foreach (var track in playlistList)
            {
                foreach (string t in track.authors)
                {

                    string ti = t;

                    track.artistrac += ti + " ";
                    
                }

                allDur = allDur.Add(track.durations);
                
            }
            dur = $"Продолжительность: {allDur}";

        }
    }
}
