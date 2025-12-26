using SpotApp_wpf.Context;
using SpotApp_wpf.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SpotApp_wpf.ViewModels
{
    public class PlaylistAdditionViewModel : BaseViewModel
    {
        public ICommand AddTagCommand { get; set; }
        public ICommand AddTrackCommand { get; set; }
        public ICommand AddPlaylistCommand { get; set; }
        public ICommand DeleteTagCommand { get; set; }
        public ICommand DeleteTrackCommand { get; set; }
        public int plId;
        public PlaylistViewModel playlistVM;
        public PlaylistAdditionViewModel(int? id, PlaylistViewModel plvm)
        {
            AddTagCommand = new RelayCommand(AddTag);
            AddTrackCommand = new RelayCommand(AddTrack);
            AddPlaylistCommand = new RelayCommand(AddPlaylist);
            DeleteTagCommand = new RelayCommand(DeleteTag);
            DeleteTrackCommand = new RelayCommand(DeleteTrack);
            int? chkid = id;
            if (id != null)
            {
                plId = (int)id;
            }
            playlistVM = plvm;
            LoadData(id);
        }

        public string title { get; set; }
        public List<string> tags { get; set; }
        public List<string> tracks { get; set; }
        public List<string> tagsL { get; set; } = new List<string>();
        public List<string> tracksL { get; set; } = new List<string>();
        private string _tagsOut { get; set; }
        public string tagsOut
        {
            get => _tagsOut;
            set
            {
                _tagsOut = value;
                OnPropertyChanged();
            }
        }
        private string _tracksOut { get; set; }
        public string tracksOut
        {
            get => _tracksOut;
            set
            {
                _tracksOut = value;
                OnPropertyChanged();
            }
        }

        private string _selectedTag { get; set; }
        public string selectedTag
        {
            get => _selectedTag;
            set
            {
                _selectedTag = value;
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

        private string _name { get; set; }
        public string name
        {
            get => _name;
            set
            {
                _name = value;
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
                OnPropertyChanged();
            }
        }
        private string _btnText { get; set; }
        public string btnText
        {
            get => _btnText;
            set
            {
                _btnText = value;
                OnPropertyChanged();
            }
        }


        public void LoadData(int? id)
        {
            var context = new SpotifyContext();
            tags = new List<string>(context.Tags.Select(t => t.TagTittle).ToList());
            tracks = new List<string>(context.Tracks.Select(t => t.TrackName).ToList());
            int? idd = id;
            if (id != null)
            {
                title = "Редактирование Плейлиста";
                btnText = "Сохранить";
                name = context.Playlists.Where(p => p.PlaylistId == id).FirstOrDefault().PlaylistName;
                description = context.Playlists.Where(p => p.PlaylistId == id).FirstOrDefault().Description;
                tagsL = context.TagsInPlaylists.Where(t => t.PlaylistId == id).Select(t => t.Tag.TagTittle).ToList();
                tracksL = context.TracksInPlaylists.Where(t => t.PlaylistId == id).Select(t => t.Track.TrackName).ToList();
                UpdateInfo();
            }
            else
            {
                title = "Добавление Плейлиста";
                btnText = "Добавить";
            }
        }

        public void AddTag()
        {
            if (string.IsNullOrWhiteSpace(selectedTag))
            {
                MessageBox.Show("Выберите тэг", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                foreach (var tag in tagsL)
                {
                    if (tag == selectedTag)
                    {
                        MessageBox.Show("Тэг повторяется", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return; 
                    }
                }
                tagsL.Add(selectedTag);
            }
            UpdateInfo();
        }
        public void DeleteTag()
        {
            tagsL.Clear();
            UpdateInfo();
        }
        public void AddTrack()
        {
            if (string.IsNullOrWhiteSpace(selectedTrack))
            {
                MessageBox.Show("Выберите композицию", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                foreach (var track in tracksL)
                {
                    if (track == selectedTrack) 
                    {
                        MessageBox.Show("Композиция повторяется", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return; 
                    }
                }
                tracksL.Add(selectedTrack);
            }
            UpdateInfo();
        }
        public void DeleteTrack()
        {
            tracksL.Clear();
            UpdateInfo();
        }

        public void UpdateInfo()
        {
            if (tagsL.Count > 0)
            {
                tagsOut = tagsL[0];
                for (int i = 1; i < tagsL.Count; i++)
                {
                    tagsOut += ", " + tagsL[i];
                }
            }
            else
            {
                tagsOut = string.Empty;
            }
            if (tracksL.Count > 0)
            {
                tracksOut = tracksL[0];
                for (int i = 1; i < tracksL.Count; i++)
                {
                    tracksOut += ", " + tracksL[i];
                }
            }
            else
            {
                tracksOut = string.Empty;
            }
        }

        public void AddPlaylist()
        {
            var context = new SpotifyContext();
            User user = (User)Application.Current.Properties["CurrentUser"];
            Playlist pl;
            
            if (plId != 0)
            {
                List<TagsInPlaylist> tips = context.TagsInPlaylists.Where(p => p.PlaylistId == plId).ToList();
                List<TracksInPlaylist> trips = context.TracksInPlaylists.Where(t => t.PlaylistId == plId).ToList();
                context.TagsInPlaylists.RemoveRange(tips);
                context.TracksInPlaylists.RemoveRange(trips);
                context.SaveChanges();

                pl = context.Playlists.Where(p => p.PlaylistId == plId).FirstOrDefault();
                pl.PlaylistName = name;
                pl.Description = description;

                context.Playlists.Update(pl);
                MessageBox.Show("Плэйлист обновлён", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                pl = new Playlist
                {
                    PlaylistId = context.Playlists.OrderBy(p => p.PlaylistId).LastOrDefault().PlaylistId + 1,
                    PlaylistName = name,
                    UserId = user.UserId,
                    DateCreated = DateTime.Now,
                    Likes = 0,
                    Description = description,
                };
                context.Playlists.Add(pl);
                MessageBox.Show("Плэйлист добавлен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            context.SaveChanges();
            if(plId == 0)
            {
                plId = context.Playlists.OrderBy(p => p.PlaylistId).LastOrDefault().PlaylistId;
            }

            for (int i = 0; i < tagsL.Count; i++)
            {
                TagsInPlaylist tip = new TagsInPlaylist
                {
                    TiPId = context.TagsInPlaylists.OrderBy(t => t.TiPId).LastOrDefault().TiPId + 1,
                    PlaylistId = plId,
                    TagId = context.Tags.Where(t => t.TagTittle == tagsL[i]).FirstOrDefault().TagId
                };
                context.TagsInPlaylists.Add(tip);
                context.SaveChanges();
            }
            for (int i = 0; i < tracksL.Count; i++)
            {
                TracksInPlaylist trip = new TracksInPlaylist
                {
                    TiPId = context.TracksInPlaylists.OrderBy(t => t.TiPId).LastOrDefault().TiPId + 1,
                    PlaylistId = plId,
                    TrackId = context.Tracks.Where(t => t.TrackName == tracksL[i]).FirstOrDefault().TrackId
                };
                context.TracksInPlaylists.Add(trip);
                context.SaveChanges();
            }

            playlistVM.LoadPlaylists();
            foreach (Window w in Application.Current.Windows)
            {
                if (w is Views.PlaylistAddition)
                {
                    w.Close();
                }
            }
        }
    }
}
