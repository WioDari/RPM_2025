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

        private TrackListView _selectedTrack;

        public TrackListView selectedTrack
        {
            get => _selectedTrack;
            set
            {
                _selectedTrack = value;
                OnPropertyChanged();
            }
        }

        public ICommand DeleteTrackCommand { get; }

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
            DeleteTrackCommand = new RelayCommand(DeleteTrack);

        }

        public ICommand GoTrack { get; }
        public class TrackListView
        {
            public int id { get; set; }
            public string namealbum { get; set; }
            public string tracknam { get; set; }
            public string artistrac { get; set; }
            public string rating { get; set; }
            public string duration { get; set; }
            public TimeSpan durations { get; set; }
            public List<string> authors { get; set; }
            public string countlisten { get; set; }
            public string imagePath { get; set; }

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
                    id = p.TrackId,
                    tracknam = p.TrackName,
                    namealbum = p.Album.AlbumName,
                    authors = p.TrackArtists.Where(tr => tr.TrackId == p.TrackId).Select(a => a.Artist.ArtistName).ToList(),
                    artistrac = "",
                    rating = p.Rating.ToString(),
                    duration = p.Duration.ToString("HH:mm:ss"),
                    durations = p.Duration.ToTimeSpan(),
                    countlisten = p.Playcount.ToString(),
                    imagePath = p.FilePath
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

        public void DeleteTrack()
        {
            using var context = new MusicContext();

            if (selectedTrack.id == null)
                return;

            var result = MessageBox.Show("Удалить трек?", "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes)
                return;

            var track = context.Tracks.FirstOrDefault(p => p.TrackId == selectedTrack.id);

            if (track == null)
                return;


            var artist = context.TrackArtists.Where(pt => pt.TrackId == selectedTrack.id);
            context.TrackArtists.RemoveRange(artist);


            var album = context.AlbumTracks.Where(tp => tp.TrackId == selectedTrack.id);
            context.AlbumTracks.RemoveRange(album);

            var playlist = context.PlayListTracks.Where(tp => tp.TrackId == selectedTrack.id);
            context.PlayListTracks.RemoveRange(playlist);

            var genre = context.GenreTracks.Where(tp => tp.TrackId == selectedTrack.id);
            context.GenreTracks.RemoveRange(genre);


            context.Tracks.Remove(track);

            context.SaveChanges();

            MessageBox.Show("Трек удалён");

            


        }

    }
}
