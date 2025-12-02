using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Drawing;
using System.IO;
using Microsoft.EntityFrameworkCore;
using PracticeWork.Context;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PracticeWork.ViewModels
{
    class TrackCardVM : BaseVM
    {
        public class TrackItem
        {
            public int ID { get; set; }
            public string name { get; set; }
            public string artist { get; set; }
            public string description { get; set; }
            public string duration { get; set; }
            public string releaseDate { get; set; }
            public string rate { get; set; }
            public string totalplays { get; set; }
            public BitmapImage image { get; set; }
        }

        private ObservableCollection<TrackItem> _data;
        public ObservableCollection<TrackItem> data
        {
            get => _data;
            set
            {
                _data = value; 
            }
        }

        public TrackCardVM()
        {
            LoadTracks();
        }

        private void LoadTracks()
        {
            var context = new PostgresContext();
            _data = new ObservableCollection<TrackItem>(context.Tracks
                .Include(t => t.ArtistNavigation).Select(t => new TrackItem
                {
                    ID = t.TrackId,
                    name = t.TrackName,
                    artist = t.ArtistNavigation.ArtistName,
                    description = t.Info,
                    duration = t.TrackDuration.ToString(),
                    releaseDate = t.ReleaseDate.ToString(),
                    rate = t.TrackRating.ToString(),
                    totalplays = t.TotalPlays.ToString(),
                    image = new BitmapImage(new Uri(t.AlbumcoverPath))
                }).OrderBy(t => t.ID).ToList());
        }
    }
}
