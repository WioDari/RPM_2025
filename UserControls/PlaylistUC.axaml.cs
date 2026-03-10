using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Avalonia.Media;
using MusicPlusPlus.Context;
using MusicPlusPlus.Models;
using System;
using System.Linq;

namespace MusicPlusPlus;

public partial class PlaylistUC : UserControl
{
    public PlaylistUC()
    {
        InitializeComponent();
    }

    public class PlaylistView
    {
        public string PlaylistName { get; set; }
        public string PlaylistAuthor { get; set; }
        public string LikesCount { get; set; }
        public int TrackCount { get; set; }
        public string CreationDate { get; set; }
        public string Duration { get; set; }
    }

    public void LoadPlaylist(Playlist playlist)
    {
        using(var context = new MusicdbContext())
        {
            context.Entry(playlist).Collection(p => p.Tracks).Load();

            var user = context.Users.FirstOrDefault(x => x.Userid == playlist.Userid);
            int trackcount = playlist.Tracks.Count();
            TimeSpan plduration = TimeSpan.Zero;
            foreach (var track in playlist.Tracks)
            {
                plduration += TimeSpan.FromSeconds(Convert.ToInt32(track.Trackduration));
            }
            DataContext = new PlaylistView
            {
                PlaylistName = playlist.Playlistname,
                PlaylistAuthor = user.Login ?? "Неизвестен",
                LikesCount = playlist.Likes.ToString(),
                TrackCount = trackcount,
                CreationDate = playlist.Playlistcreationdate.ToString(),
                Duration = plduration.ToString()
                
            };
            if (user.Subscriptionid == 2)
            {
                AuthorTBlock.Foreground = new SolidColorBrush(Color.Parse("#ff943d"));
            }
            if (trackcount == 0)
            {
                TracksTBlock.Foreground = new SolidColorBrush(Color.Parse("#CC3366"));
            }
        }
    }
}