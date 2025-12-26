using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Spotify_wpf.Context;
using Spotify_wpf.Models;

namespace Spotify_wpf.ViewModels
{
    internal class AddPlaylistViewModel :BaseViewModel
    {
        public ObservableCollection<string> tags { get; set; }

        private ObservableCollection<Tag> _allTags;

        public ObservableCollection<string> track { get; set; }
        private ObservableCollection<Track> _allTracks;

     


        public Playlist selectedPlaylist { get; set; }
        private int _id { get; set; }
        public int id
        {
            get => _id;
            set
            {
                _id = value;
               
                OnPropertyChanged();
            }
        }
        private string _user { get; set; }
        public string user
        {
            get => _user;
            set
            {
                _user = value;
                //selectedAlbum.AlbumName = value;
                OnPropertyChanged();
            }
        }


        private string _nameaPlaylist { get; set; }
        public string nameaPlaylist
        {
            get => _nameaPlaylist;
            set
            {
                _nameaPlaylist = value;
                //selectedAlbum.AlbumName = value;
                OnPropertyChanged();
            }
        }

        private int _likes { get; set; }
        public int likes
        {
            get => _likes;
            set
            {
                _likes = value;
                //selectedAlbum.AlbumName = value;
                OnPropertyChanged();
            }
        }

        private string _description { get; set; }
        public string description
        {
            get => _description;
            set
            {
                _description = value;
                //selectedAlbum.AlbumName = value;
                OnPropertyChanged();
            }
        }

        private DateOnly _datacreate = DateOnly.FromDateTime(DateTime.UtcNow);
        public DateOnly datacreate 
        { 
            get => _datacreate;
            set
            {
                _datacreate = value;
                //selectedAlbum.AlbumName = value;
                OnPropertyChanged();
            }
        }

        private string _selectedTags { get; set; }
        public string selectedTags
        {
            get => _selectedTags;
            set
            {
                _selectedTags = value;
                OnPropertyChanged();
            }
        }

        private string _selectedTrack { get; set; }
        public string selectedTrack
        {
            get => _selectedTrack;
            set
            {
                _selectedTrack = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddTagsCommand { get; }
        public ICommand AddTrackCommand { get; }

        private ObservableCollection<string> chooseTrack { get; set; } = new ObservableCollection<string>();
        private string _chooseTrack { get; set; }
        public string chooseTracks
        {
            get => _chooseTrack;
            set
            {
                _chooseTrack = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<string> chooseTags { get; set; } = new ObservableCollection<string>();
        private string _chooseTags { get; set; }
        public string chooseTag
        {
            get => _chooseTags;
            set
            {
                _chooseTags = value;
                OnPropertyChanged();
            }
        }


        public void LoadData()
        {
            var context = new MusicContext();
            tags = new ObservableCollection<string>(context.Tags.Select(g => g.TagsName));
            track = new ObservableCollection<string>(context.Tracks.Select(g => g.TrackName));

        }

        public void AddTags()
        {
            chooseTags.Add(selectedTags);
            _chooseTags += selectedTags + " ";
            OnPropertyChanged(nameof(chooseTag));

        }

        public void AddTrack()
        {
            chooseTrack.Add(selectedTrack);
            _chooseTrack += selectedTrack + " ";
            
            OnPropertyChanged(nameof(chooseTracks));

        }

        public ICommand SelectImageCommand { get; }
        public ICommand DeleteAlbumCommand { get; }
        public ICommand SavePlaylistCommand { get; }

        public AddPlaylistViewModel()
        {
            var context = new MusicContext();
            var tagsList = context.Tags
                .Select(g => g.TagsName)
                .Distinct()
                .OrderBy(s => s)
                .ToList();
            tags = new ObservableCollection<string>(tagsList);

            var trackList = context.Tracks
                .Select(s => s.TrackName)
                .Distinct()
                .OrderBy(s => s)
                .ToList();
            track = new ObservableCollection<string>(trackList);
            AddTagsCommand = new RelayCommand(AddTags);
            AddTrackCommand = new RelayCommand(AddTrack);
            SavePlaylistCommand = new RelayCommand(SavePlaylist);

        }

        private void SavePlaylist()
        {
            MusicContext context = new MusicContext();
            Playlist playlist = new Playlist
            {
                UserId = Properties.Settings.Default.userid,
                PlaylistName = nameaPlaylist,
                DataCreate = datacreate,
                Likes = likes,
                Description = description,


            };
            context.Playlists.Add(playlist);
            context.SaveChanges();
            int playlistId = playlist.PlayListId;

            var trackIds = context.Tracks
                .Where(t => chooseTrack.Contains(t.TrackName))
                .Select(t => t.TrackId)
                .ToList();

            foreach (var trackId in trackIds)
            {
                context.PlayListTracks.Add(new PlayListTrack
                {
                   PlayListTrackId = context.PlayListTracks.OrderBy(c => c.PlayListTrackId).LastOrDefault().PlayListTrackId + 1,
                    PlaylistId = playlistId,
                    TrackId = trackId
                });
                context.SaveChanges();
            }

            var tagIds = context.Tags
                .Where(t => chooseTags.Contains(t.TagsName))
                .Select(t => t.TagsId)
                .ToList();

            foreach (var tagId in tagIds)
            {
                context.TagsPlaylists.Add(new TagsPlaylist
                {
                    TagsPlaylistId = context.TagsPlaylists.OrderBy(c => c.TagsPlaylistId).LastOrDefault().TagsPlaylistId + 1,
                    PlaylistId = playlistId,
                    TagsId = tagId
                });
                context.SaveChanges();
            }

           

            MessageBox.Show("Успех", "Вы успешно добавили плейлист ");
        }

    }
}
