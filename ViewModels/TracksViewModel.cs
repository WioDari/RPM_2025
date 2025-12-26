using Microsoft.EntityFrameworkCore;
using SpotApp_wpf.Context;
using SpotApp_wpf.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static SpotApp_wpf.ViewModels.AlbumDetailViewModel;

namespace SpotApp_wpf.ViewModels
{
    public class TracksViewModel
    {
        public ICommand AddTrackCommand { get; }
        public TracksViewModel() 
        { 
            LoadTracks();
            AddTrackCommand = new RelayCommand(AddTrack);
        }

        public class Track
        {
            public int Id { get; set; }
            public string trackName { get; set; }
            public string authorsl { get; set; }
            public string param { get; set; }
            public string rating { get; set; }
            public string duration { get; set; }
            public List<string> authors { get; set; }
        }

        public ObservableCollection<Track> tracks { get; set; }

        public void AddTrack()
        {
            Window add = new TracksAddition();
            add.DataContext = new TracksAdditionViewModel();
            add.Show();
        }

        public void LoadTracks()
        {
            var context = new SpotifyContext();
            tracks = new ObservableCollection<Track>(context.Tracks
                .Include(t => t.ArtistsInTracks)
                .Select(t => new Track
                {
                    Id = t.TrackId,
                    trackName = t.TrackName,
                    authors = t.ArtistsInTracks.Where(at => at.TrackId == t.TrackId).Select(t => t.Artist.ArtistName).ToList(),
                    authorsl = "",
                    param = t.PlayCount.ToString(),
                    rating = t.Rating.ToString(),
                    duration = t.Duration.ToString("mm:ss"),
                })
                .OrderBy(t => t.Id)
                .ToList());

            foreach (var track in tracks)
            {
                track.authorsl = track.authors[0];
                for (int i = 1; i < track.authors.Count; i++)
                {
                    track.authorsl += ", " + track.authors[i];
                }
            }
        }
    }
}
