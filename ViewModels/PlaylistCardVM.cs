using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Microsoft.EntityFrameworkCore;
using PracticeWork.Context;
using PracticeWork.Models;
using static PracticeWork.ViewModels.TrackCardVM;

namespace PracticeWork.ViewModels
{
    internal class PlaylistCardVM : BaseVM
    {
        public class PlaylistItem
        {
            public int ID { get; set; }
            public string? Title { get; set; }
            public string? Author { get; set; }
            public string? Likes { get; set; }
            public string? Subs { get; set; }
            public string? Tracks { get; set; }
            public string? CreatedAt { get; set; }
            public string? Duration { get; set; }
        }

        private ObservableCollection<PlaylistItem> _data;
        public ObservableCollection<PlaylistItem> data
        {
            get => _data;
            set
            {
                _data = value;
                OnPropertyChanged();
            }
        }
        public PlaylistCardVM()
        {
            LoadPlaylists();
        }
        private void LoadPlaylists()
        {
            var context = new PostgresContext();
            
            _data = new ObservableCollection<PlaylistItem>(context.PlayLists
                .Include(p => p.UserCreatorNavigation).Select(p => new PlaylistItem
                {

                    ID = p.PlaylistId,
                    Title = p.PlaylistName,
                    Author = p.UserCreatorNavigation.Fio,
                    Likes = p.Likes.ToString(),
                    Subs = context.UserPlaylists.Where(up => up.Uplaylist == p.PlaylistId).Count().ToString(),
                    Tracks = context.TrackPlaylists.Where(tp => tp.Playlist == p.PlaylistId).Count().ToString(),
                    CreatedAt = p.CreationDate.ToString(),
                    Duration = ""//duration.ToString(@"hh\:mm\:ss")
                }).OrderBy(p => p.ID).ToList());
        }

        /*private TimeSpan? DurationSum(int id, PostgresContext context)
        {
            TimeSpan? duration = null;
            foreach(Track track in context.Tracks.Where(t => t.TrackId == id))
            {
                duration += track.TrackDuration;
            }
            return duration;
        }*/
    }
}
