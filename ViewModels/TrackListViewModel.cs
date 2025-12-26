using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using Spotify_wpf.Context;
using Spotify_wpf.Models;
using Spotify_wpf.Views;
using static Spotify_wpf.ViewModels.AlbumViewModel;
using static Spotify_wpf.ViewModels.PlaylistListViewModel;

namespace Spotify_wpf.ViewModels
{
    class TrackListViewModel : BaseViewModel
    {
        private ObservableCollection<TrackListView> _trackList;

        public ObservableCollection<TrackListView> trackLists
        {
            get => _trackList;
            set
            {
                _trackList = value;
                OnPropertyChanged();
            }
        }

        public TrackListViewModel()
        {
            LoadTrack();
            GoTrack = new RelayCommand(GoTracks);
        }

        public ICommand GoTrack { get; }
        public class TrackListView
        {
            public string id { get; set; }

            public string namealbum { get; set; }


            public string tracknam { get; set; }

            public string artistrac { get; set; }


            public string rating { get; set; }

            public string duration { get; set; }

            public TimeSpan durations { get; set; }

            public List<string> authors { get; set; }

            public string countlisten { get; set; }

        }

        public void LoadTrack()
        {
            var context = new MusicContext();
            _trackList = new ObservableCollection<TrackListView>(context.Tracks
                .Include(p => p.Album)
                .Include(p => p.PlayListTracks)
                .Include(p => p.TrackArtists)
                .Include(p => p.AlbumTracks)
                .Select(p => new TrackListView
                {
                    tracknam = p.TrackName,
                    namealbum = p.Album.AlbumName,
                    authors = p.TrackArtists.Where(tr => tr.TrackId == p.TrackId).Select(a => a.Artist.ArtistName).ToList(),
                    artistrac = "",
                    rating = p.Rating.ToString(),
                    duration = p.Duration.ToString("mm:ss"),
                    durations = p.Duration.ToTimeSpan(),
                    countlisten = p.Playcount.ToString()
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

        public void GoTracks()
        {
            Window window = new AddTrack();
            window.Show();
        }




    }
}
