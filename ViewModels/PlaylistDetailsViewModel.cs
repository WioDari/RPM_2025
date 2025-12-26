using Microsoft.EntityFrameworkCore;
using SpotApp_wpf.Context;
using SpotApp_wpf.Models;
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
    class PlaylistDetailsViewModel: BaseViewModel
    {
        private string _playlistName { get; set; }
        public string playlistName
        {
            get => _playlistName;
            set
            {
                _playlistName = value;
                OnPropertyChanged();
            }
        }
        private string _creationDate { get; set; }
        public string creationDate
        {
            get => _creationDate;
            set
            {
                _creationDate = value;
                OnPropertyChanged();
            }
        }
        private string _likes { get; set; }
        public string likes
        {
            get => _likes;
            set
            {
                _likes = value;
                OnPropertyChanged();
            }
        }
        private string _duration { get; set; }
        public string duration
        {
            get => _duration;
            set
            {
                _duration = value;
                OnPropertyChanged();
            }
        }
        public TimeSpan allDuration { get; set; }

        private string _tags { get; set; }
        public string tags
        {
            get => _tags;
            set
            {
                _tags = value;
                OnPropertyChanged();
            }
        }
        public List<string> tagsList { get ; set; }

        public class PlTrackDetails
        {
            public string id { get; set; }
            public string trackName { get; set; }
            public List<string> authors { get; set; }
            public string authorsl { get; set; }
            public string param { get; set; }
            public string rating { get; set; }
            public string duration { get; set; }
            public TimeSpan dur { get; set; }

        }
        public ObservableCollection<PlTrackDetails> plDetails { get; set; }
        private int _playlistId;
        private PlaylistViewModel _playlistViewModel;

        public ICommand deletePlaylistCommand { get; set; }
        public ICommand editPlaylistCommand { get; set; }
        public PlaylistDetailsViewModel(int id, PlaylistViewModel plvm)
        {
            LoadPlDetails(id);
            _playlistViewModel = plvm;
            _playlistId = id;
            deletePlaylistCommand = new RelayCommand(DeletePlaylist);
            editPlaylistCommand = new RelayCommand(EditPlaylist);
        }

        public void DeletePlaylist()
        {
            var context = new SpotifyContext();
            Playlist pl = context.Playlists.Where(p => p.PlaylistId == _playlistId).FirstOrDefault();
            List<TagsInPlaylist> tags = context.TagsInPlaylists.Where(t => t.PlaylistId == _playlistId).ToList();
            List<TracksInPlaylist> tracks = context.TracksInPlaylists.Where(tr => tr.PlaylistId == _playlistId).ToList();
            context.TagsInPlaylists.RemoveRange(tags);
            context.TracksInPlaylists.RemoveRange(tracks);
            context.SaveChanges();
            context.Playlists.Remove(pl);
            context.SaveChanges();
            _playlistViewModel.LoadPlaylists();
            foreach (Window w in Application.Current.Windows)
            {
                if (w is Views.PlaylistDetails)
                {
                    w.Close();
                }
            }
        }

        public void EditPlaylist()
        {
            Window edit = new PlaylistAddition();
            edit.DataContext = new PlaylistAdditionViewModel(_playlistId, _playlistViewModel);
            edit.Show();
            foreach (Window w in Application.Current.Windows)
            {
                if (w is Views.PlaylistDetails)
                {
                    w.Close();
                }
            }
        }

        public void LoadPlDetails(int id)
        {
            var context = new SpotifyContext();

            int uid = context.Playlists.Include(p => p.User).Where(p => p.PlaylistId == id).FirstOrDefault().UserId;
            playlistName = context.Playlists.Where(p => p.PlaylistId == id).FirstOrDefault().PlaylistName + " | " + context.Users.Where(u => u.UserId == uid).FirstOrDefault().FullName;
            creationDate = context.Playlists.Where(p => p.PlaylistId == id).FirstOrDefault().DateCreated.ToString("dd.MM.yyyy");
            likes = context.Playlists.Where(p => p.PlaylistId == id).FirstOrDefault().Likes.ToString();
            duration = allDuration.ToString();
            tagsList = context.TagsInPlaylists.Include(t => t.Tag).Where(t => t.PlaylistId == id).Select(t => t.Tag.TagTittle).ToList();
            if (tagsList.Count > 0)
            {
                tags = tagsList[0];
                for (int i = 1; i < tagsList.Count; i++)
                {
                    tags += ", " + tagsList[i];
                }
            }
            else
            {
                tags = "";
            }

            plDetails = new ObservableCollection<PlTrackDetails>(context.Tracks.Where(t => t.TracksInPlaylists.Any(tp => tp.PlaylistId == id))
                .Include(t => t.ArtistsInTracks)
                .Include(t => t.Album)
                .Include(t => t.TracksInPlaylists)
                .Select(t => new PlTrackDetails
                {
                    id = t.TrackId.ToString(),
                    trackName = t.TrackName,
                    authors = t.ArtistsInTracks.Where(at => at.TrackId == t.TrackId).Select(t => t.Artist.ArtistName).ToList(),
                    authorsl = "",
                    param = t.Album.AlbumTitle,
                    rating = t.Rating.ToString(),
                    duration = t.Duration.ToString("mm:ss"),
                    dur = t.Duration.ToTimeSpan(),
                })
                .OrderBy(t => t.id)
                .ToList());

            foreach (var track in plDetails)
            {
                track.authorsl = track.authors[0];
                for (int i = 1; i < track.authors.Count; i++)
                {
                    track.authorsl += ", " + track.authors[i];
                }
                allDuration = allDuration.Add(track.dur);
            }
            duration = allDuration.ToString();
        }
    }
}
