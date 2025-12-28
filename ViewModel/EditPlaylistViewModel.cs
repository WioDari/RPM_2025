using Microsoft.EntityFrameworkCore;
using MusicWpf.Context;
using MusicWpf.Models;
using MusicWpf.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MusicWpf.ViewModel
{
    class EditPlaylistViewModel : BaseViewModel
    {
        private string _namePlaylist;
        public string NamePlaylist
        {
            get => _namePlaylist;

            set
            {
                _namePlaylist = value;
                OnPropertyChanged();
            }

        }

        private string _descriptionPlaylist;
        public string DescriptionPlaylist 
        {
            get => _descriptionPlaylist;
            set
            {
                _descriptionPlaylist = value;
                OnPropertyChanged();
            }
        }


        private ObservableCollection<Tag> _tags;
        public ObservableCollection<Tag> Tags
        {
            get => _tags;
            set
            {
                _tags = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Track> _tracks;
        public ObservableCollection<Track> Tracks
        {
            get => _tracks;
            set
            {
                _tracks = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Track> SelectedTracks { get; } = new ObservableCollection<Track>();
        public ObservableCollection<Tag> SelectedTags { get; } = new ObservableCollection<Tag>();

        private Track _selectedTrack;
        public Track SelectedTrack
        {
            get => _selectedTrack;
            set
            {
                _selectedTrack = value;
                OnPropertyChanged();
                if (value != null && !SelectedTracks.Contains(value))
                {
                    SelectedTracks.Add(value);
                    UpdateSelectedTrackText();
                    _selectedTrack = null;
                    OnPropertyChanged(nameof(SelectedTrack));
                }
            }
        }

        private Tag _selectedTag;
        public Tag SelectedTag
        {
            get => _selectedTag;
            set
            {
                _selectedTag = value;
                OnPropertyChanged();
                if (value != null && !SelectedTags.Contains(value))
                {
                    SelectedTags.Add(value);
                    UpdateSelectedTagsText();
                    _selectedTag = null;
                    OnPropertyChanged(nameof(SelectedTag));
                }
            }
        }


        private string _selectedTagsText;
        public string SelectedTagsText
        {
            get => _selectedTagsText;
            set
            {
                _selectedTagsText = value;
                OnPropertyChanged();
            }
        }
        private string _selectedTracksText;
        public string SelectedTracksText
        {
            get => _selectedTracksText;
            set
            {
                _selectedTracksText = value;
                OnPropertyChanged();
            }
        }

        private int _playlistId;


        public ICommand EditCommandPlaylist { get; private set; }
        public ICommand DeleteTrackCommand { get; private set; }
        public ICommand DeleteTagCommand { get; private set; }

        public EditPlaylistViewModel(int playlistId)
        {
            _playlistId = playlistId;     
            EditCommandPlaylist = new RelayCommand(EditPlaylist);
            DeleteTrackCommand = new RelayCommand(DeleteSelectedTrack);
            DeleteTagCommand = new RelayCommand(DeleteSelectedTag);
            LoadData();
        }

        public void LoadData()
        {
            try
            {
                using var db = new MusicContext();

                var allTags = db.Tags.ToList();
                var allTracks = db.Tracks.ToList();

                Tags = new ObservableCollection<Tag>(db.Tags.ToList());
                Tracks = new ObservableCollection<Track>(db.Tracks.ToList());

                var playlist = db.Playlists
                    .Include(p => p.TracksPlaylists)
                    .ThenInclude(tp => tp.Tracks)
                    .Include(p => p.PlaylistTags)
                    .ThenInclude(p => p.Tags)
                    .FirstOrDefault(p => p.PlaylistId == _playlistId);

                NamePlaylist = playlist.PlaylistName;
                DescriptionPlaylist = playlist.Description;

                SelectedTracks.Clear();
                if (playlist.TracksPlaylists != null)
                {
                    foreach (var tp in playlist.TracksPlaylists)
                    {
                        if (tp.Tracks != null)
                        {
                            SelectedTracks.Add(tp.Tracks);
                        }
                    }
                }

                SelectedTags.Clear();
                if(playlist.PlaylistTags != null)
                {
                    foreach(var pt in playlist.PlaylistTags)
                    {
                        if (pt.Tags != null)
                        {
                            SelectedTags.Add(pt.Tags);
                        }
                    }
                }

                UpdateSelectedTrackText();
                UpdateSelectedTagsText();
            }
            catch
            {

            }
        }

        private void UpdateSelectedTagsText()
        {
            if (SelectedTags.Any())
            {
                SelectedTagsText = string.Join(", ", SelectedTags.Select(t => t.TagsName));
            }
            else
            {
                SelectedTagsText = "Теги не выбраны";
            }
        }

        private void UpdateSelectedTrackText()
        {
            if (SelectedTracks.Any())
            {
                SelectedTracksText = string.Join(", ", SelectedTracks.Select(t => t.TrackName));
            }
            else
            {
                SelectedTracksText = "Треки не выбраны";
            }
        }


        private void DeleteSelectedTrack()
        {

            if (SelectedTracks.Any())
            {
                SelectedTracks.Clear();
                UpdateSelectedTrackText();
            }
            else
            {
                MessageBox.Show("Нет треков для удаления");
            }
        }

        private void DeleteSelectedTag()
        {
            if (SelectedTags.Any())
            {
                SelectedTags.Clear();
                UpdateSelectedTagsText();
            }
            else
            {
                MessageBox.Show("Нет тегов для удаления");
            }
        }

        public void EditPlaylist()
        {
            try
            {
                using var db = new MusicContext();
                var playlist = db.Playlists
                        .Include(p => p.TracksPlaylists)
                    .Include(p => p.PlaylistTags)
                    .FirstOrDefault(p => p.PlaylistId == _playlistId);
                /*
                                {
                                    PlaylistName = TitlePlaylist,
                                    Description = DescriptionPlaylist,
                                    DateCreated = CreatedData,
                                    UserId = Avtor,
                                    Likes = 0
                                };*/

                playlist.PlaylistName = NamePlaylist;
                playlist.Description = DescriptionPlaylist;
   
                db.PlaylistTags.RemoveRange(playlist.PlaylistTags);
                /*  if (SelectedTags.Any())
                  {
                      var tagIds = SelectedTags.Select(t => t.TagsId).ToList();

                      var tagsFromDb = db.Tags
                          .Where(t => tagIds.Contains(t.TagsId))
                          .AsNoTracking()
                          .ToList();

                      foreach (var tag in tagsFromDb)
                      {
                          var playlistTag = new PlaylistTag
                          {
                              PlaylistId = playlist.PlaylistId,
                              TagsId = tag.TagsId
                          };
                         // db.PlaylistTags.Add(playlistTag);
                      }
                  }*/

                if (SelectedTags.Any())
                {
                    var tagIds = SelectedTags.Select(t => t.TagsId).ToList();

                    var playlistTags = tagIds.Select(tagId => new PlaylistTag
                    {
                        PlaylistId = playlist.PlaylistId,
                        TagsId = tagId
                    }).ToList();

                    db.PlaylistTags.AddRange(playlistTags);
                }

                db.TracksPlaylists.RemoveRange(playlist.TracksPlaylists);
                /* if (SelectedTracks.Any())
                 {
                     var trackIds = SelectedTracks.Select(t => t.TracksId).ToList();

                     var tracksFromDb = db.Tracks
                         .Where(t => trackIds.Contains(t.TracksId))
                         .AsNoTracking()
                         .ToList();

                     foreach (var track in tracksFromDb)
                     {
                         var trackPlaylist = new TracksPlaylist
                         {
                             PlaylistId = playlist.PlaylistId,
                             TracksId = track.TracksId
                         };
                        // db.TracksPlaylists.Add(trackPlaylist);
                     }

                 }*/

                if (SelectedTracks.Any())
                {
                    var trackIds = SelectedTracks.Select(t => t.TracksId).ToList();

                    var tracksPlaylists = trackIds.Select(trackId => new TracksPlaylist
                    {
                        PlaylistId = playlist.PlaylistId,
                        TracksId = trackId
                    }).ToList();

                    db.TracksPlaylists.AddRange(tracksPlaylists);
                }
                db.SaveChanges();

                MessageBox.Show("Плейлист успешно отредактирован");

             
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании плейлиста: {ex.Message}");
            }
         

        }

    }
}
