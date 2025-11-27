using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Music.Context;
using Music.Models;
using static Music.ViewModels.AlbumViewModel;

namespace Music.ViewModels
{
    partial class TrackViewModel : ObservableObject
    {
        public class TrackView()
        {
            public string name { get; set; }
            public List<Artist> artists { get; set; }
            public string artistString { get; set; }
            public string playCount { get; set; }
            public string rating { get; set; }
            public double durationMillis {  get; set; } 
            public string duration { get; set; }
        }
        public TrackViewModel() {
            getTracks();
        }
        public ObservableCollection<TrackView> data { get; set; }
        public void getTracks()
        {
            var context = new MusicDbContext();
            data = new ObservableCollection<TrackView>(context.Tracks
                    .Select(x => new TrackView
                    {
                        name = x.TrackName,
                        playCount = x.PlayCount.ToString(),
                        rating = x.Rating.ToString(),
                        durationMillis = x.Duration,
                        artists = x.Artists.ToList(),
                    }).ToList());
            foreach (var d in data)
            {
                
                foreach (var a in d.artists) {
                    d.artistString += a.ArtistName+ " ";
                }
                TimeSpan timeSpan =  TimeSpan.FromSeconds(d.durationMillis);
                 d.duration = $"{timeSpan:mm\\:ss}";
           
            }
        }
    }
}
