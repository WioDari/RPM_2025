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

            data = new ObservableCollection<PlaylistItem>(context.TrackPlaylists
                .Include(p => p.PlaylistNavigation)
                .Include(t => t.TrackNavigation)
                .Include(y => y.PlaylistNavigation.UserCreatorNavigation)
                .Where(z => z.TrackNavigation != null && z.PlaylistNavigation != null && z.TrackNavigation.TrackDuration.HasValue)
                .Select(x => new PlaylistItem
                {
                    ID = x.PlaylistNavigation.PlaylistId,
                    Title = x.PlaylistNavigation.PlaylistName,
                    Author = x.PlaylistNavigation.UserCreatorNavigation.Fio,
                    Likes = x.PlaylistNavigation.Likes.ToString(),
                    Subs = context.UserPlaylists.Where(z => z.Uplaylist == x.PlaylistNavigation.PlaylistId).Count().ToString(),
                    Tracks = context.TrackPlaylists.Where(z => z.Playlist == x.PlaylistNavigation.PlaylistId).Count().ToString(),
                    CreatedAt = x.PlaylistNavigation.CreationDate.ToString(),
                    Duration = TimeSpan.FromSeconds(context.TrackPlaylists.Where(v => v.TrackNavigation != null && v.Playlist == x.PlaylistNavigation.PlaylistId && v.TrackNavigation.TrackDuration.HasValue == true)
                    .Select(b => b.TrackNavigation.TrackDuration.Value.TotalSeconds).Sum()).ToString(@"hh\:mm\:ss")
                }).OrderBy(a => a.ID).ToList());
        }
    }
}
