using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Xml.Linq;
using Music.Context;
using Music.Models;
using Music.Views;
using Music.Views.Items;
using Music.Properties;
using System.Windows.Controls;
using static Music.ViewModels.AlbumViewModel;
using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using System.IO;
using static Music.ViewModels.PlaylistViewModel;


namespace Music.ViewModels
{
    internal class MainPageViewModel : BaseViewModel
    {
        public ObservableCollection<AlbumView> AlbumMain { get; set; }
        public ObservableCollection<PlaylistView> PlaylistMain { get; set; }

        public MainPageViewModel() {
            showAlbum();
            showPlaylist();
        }
        private void showPlaylist()
        {
            User user = (User)Application.Current.Properties["CurrentUser"];
            var context = new MusicDbContext();
            PlaylistMain = new ObservableCollection<PlaylistView>(context.Playlists
                   .Include(x => x.User)
                   .Where(x => x.UserId == user.UserId)
                   .Select(x => new PlaylistView
                   {
                       id = x.PlaylistId,
                       name = x.PlaylistName,
                       creatorName = x.User.FullName,
                       creation_date = x.CreationDate.ToString("dd.MM.yyyy"),
                       likes = x.Likes.ToString(),
                       subscribers = x.UserPlaylistSubscriptions.Count.ToString(),
                       numberOfTrack = x.Tracks.Count.ToString()
                   }
                   ).OrderBy(x => x.id).ToList());
            foreach (var p in PlaylistMain)
            {
                p.durationInt = context.Playlists.Include(x => x.Tracks).FirstOrDefault(x => x.PlaylistId == p.id).Tracks.Sum(x => x.Duration);
                TimeSpan timeSpan = TimeSpan.FromSeconds(p.durationInt);
                p.duration = $"{timeSpan:hh\\:mm\\:ss}";
            }
        }
        private void showAlbum()
        {
            var context = new MusicDbContext();
            AlbumMain = new ObservableCollection<AlbumView>(context.Albums
                   .Include(x => x.Artist)
                   .Include(x => x.Tracks)
                   .Select(x => new AlbumView
                   {
                       id = x.AlbumId.ToString(),
                       name = x.AlbumName,
                       artistName = x.Artist.ArtistName,
                       numberOfTrack = x.Tracks.Count.ToString(),
                       coverPath = File.Exists($"C:\\Users\\glagol\\source\\repos\\Music\\Music\\Resources\\covers\\{x.AlbumName}.jpg")
                       ? $"/Resources/covers/{x.AlbumName}.jpg"
                       : "/Resources/covers/placeholder_cover.png"

                   }
                   ).OrderBy(x => x.id).ToList().TakeLast(3));

        }

    }
}
