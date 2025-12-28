using Microsoft.EntityFrameworkCore;
using MusicWpf.Context;
using MusicWpf.Models;
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
    class AddPlaylistViewModel : BaseViewModel
    {
        private string _titlePlaylist;
        public string TitlePlaylist
        {
            get => _titlePlaylist;

            set
            {
                _titlePlaylist = value;
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
        private DateOnly _createdData;
        public DateOnly CreatedData
        {
            get => _createdData;
            set
            {
                _createdData = value;
                OnPropertyChanged();
            }
        }

        private int? _avtor;
        public int? Avtor
        {
            get => _avtor;
            set
            {
                _avtor = value;
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




        public ICommand AddPlaylistCommand { get; private set; }

        public AddPlaylistViewModel()
        {
            LoadData();
            AddPlaylistCommand = new RelayCommand(PlaylistCreat);
        }


        private void LoadData()
        {
            try
            {
                using var db = new MusicContext();

                
                Tags = new ObservableCollection<Tag>(db.Tags.ToList());
                Tracks = new ObservableCollection<Track>(db.Tracks.ToList());
                CreatedData = DateOnly.FromDateTime(DateTime.Today);
                var firstUser = db.Users.FirstOrDefault();
                if (firstUser != null)
                {
                    Avtor = firstUser.UserId;        
                }

                UpdateSelectedTagsText();
                UpdateSelectedTrackText();
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}");
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

        public void PlaylistCreat()
        {
            if (string.IsNullOrWhiteSpace(TitlePlaylist))
            {
                MessageBox.Show("Введите название плейлиста");
                return;
            }

            if (DescriptionPlaylist == null)
            {
                MessageBox.Show("Введите описание");
                return;
            }


            try
            {
                using var db = new MusicContext();
                var playlist = new Playlist
                {
                    PlaylistName = TitlePlaylist,
                    Description = DescriptionPlaylist,                
                    DateCreated = CreatedData,
                    UserId = Avtor,
                    Likes = 0
                };

                db.Playlists.Add(playlist);
                db.SaveChanges();

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
                    db.PlaylistTags.Add(playlistTag);
                }

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
                    db.TracksPlaylists.Add(trackPlaylist);
                }

              
                db.SaveChanges();

                MessageBox.Show("Плейлист успешно создан");

                TitlePlaylist = "";
                DescriptionPlaylist = "";
                SelectedTags.Clear();
                SelectedTracks.Clear();

                CreatedData = DateOnly.FromDateTime(DateTime.Today);

                UpdateSelectedTagsText();
                UpdateSelectedTrackText();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании плейлиста: {ex.Message}");
            }           
        }
    }
}
