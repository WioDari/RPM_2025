using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using PracticeWork.Commands;
using PracticeWork.Context;
using PracticeWork.Models;

namespace PracticeWork.ViewModels
{
    partial class EditPlaylistVM : BaseVM
    {
        TimeOnly time = new TimeOnly(0, 0, 0);
        PostgresContext? context { get; set; }

        public ICommand AcseptChangesCommand { get; set; }
        public ICommand AddTrackCommand { get; set; }
        public ICommand AddTagCommand { get; set; }
        public ICommand ChangeAutorCommand { get; set; }

        public int selectedTag { get; set; }
        public int selectedTrack { get; set; }
        public int selectedAutor { get; set; }

        private int? _ID;
        public int? ID
        {
            get => _ID;
            set
            {
                _ID = value;
                OnPropertyChanged();
            }
        }
        private string _title;
        public string title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }
        private DateTime _selectedDate;
        public DateTime selectedDate
        {
            get => _selectedDate;
            set
            {
                _selectedDate = value;
                OnPropertyChanged();
            }
        }

        public class autoR
        {
            public int? ID { get; set; }
            public string? AuName { get; set; }
        }
        public class tracK
        {
            public int? ID { get; set; }
            public string? NameTr { get; set; }
        }
        public class taG
        {
            public int? ID { get; set; }
            public string? NameTa { get; set; }
        }
        private ObservableCollection<autoR> _autors;
        public ObservableCollection<autoR> autors
        {
            get => _autors;
            set
            {
                _autors = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<tracK> _tracks;
        public ObservableCollection<tracK> tracks
        {
            get => _tracks;
            set
            {
                _tracks = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<taG> _tags;
        public ObservableCollection<taG> tags
        {
            get => _tags;
            set
            {
                _tags = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<taG> _selectedTags;
        public ObservableCollection<taG> selectedTags
        {
            get => _selectedTags;
            set
            {
                _selectedTags = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<tracK> _selectedTracks;
        public ObservableCollection<tracK> selectedTracks
        {
            get => _selectedTracks;
            set
            {
                _selectedTracks = value;
                OnPropertyChanged();
            }
        }


        private void GetSelectedTracks(PostgresContext? contex, int? id)
        {
            selectedTracks = new ObservableCollection<tracK>(contex.TrackPlaylists
                    .Include(p => p.PlaylistNavigation)
                    .Include(p => p.TrackNavigation)
                    .Where(p => p.PlaylistNavigation.PlaylistId == id)
                    .Select(t => new tracK
                    {
                        ID = t.TrackNavigation.TrackId,
                        NameTr = t.TrackNavigation.TrackName
                    }).ToList()
                );
        }
        private void GetSelectedTags(PostgresContext contex, int? id)
        {
            selectedTags = new ObservableCollection<taG>(contex.PlayistTags
                    .Include(p => p.PlayListNavigation)
                    .Include(p => p.TagNavigation)
                    .Where(p => p.TagNavigation != null && p.PlayListNavigation != null && p.PlayListNavigation.PlaylistId == id)
                    .Select(t => new taG
                    {
                        ID = t.TagNavigation.TagId,
                        NameTa = t.TagNavigation.TagName
                    }).ToList()
                );
        }

        public EditPlaylistVM(int? PLid)
        {
            ID = PLid;
            context = new PostgresContext();
            LoadInfo();
            AddTagCommand = new Commands.RelayCommand(AddTag);
            AddTrackCommand = new Commands.RelayCommand(AddTrack);
            AcseptChangesCommand = new Commands.RelayCommand(AcseptChanges);
            ChangeAutorCommand = new Commands.RelayCommand(ChangeAutor);
        }
        private void LoadInfo()
        {
            
            if (ID != 0)
            {
                var P = context.PlayLists.FirstOrDefault(p => p.PlaylistId == ID);

                title = P.PlaylistName;

                autors = new ObservableCollection<autoR>(context.Users.Select(a => new autoR
                {
                    ID = a.UserId,
                    AuName = a.Fio
                }));

                tracks = new ObservableCollection<tracK>(context.Tracks.Select(tr => new tracK
                {
                    ID = tr.TrackId,
                    NameTr = tr.TrackName
                }));

                tags = new ObservableCollection<taG>(context.Tags.Select(ta => new taG
                {
                    ID = ta.TagId,
                    NameTa = ta.TagName
                }));

                GetSelectedTracks(context, ID);

                GetSelectedTags(context, ID);

                selectedDate = (context.PlayLists.FirstOrDefault(p => p.PlaylistId == ID).CreationDate).ToDateTime(time);
            }
        }

        public void AddTrack()
        {
            var ST = context.TrackPlaylists.FirstOrDefault(e => e.Track == selectedTrack && e.Playlist == ID);
            if (ST == null)
            {
                context.TrackPlaylists.Add(new TrackPlaylist
                {
                    Track = selectedTag,
                    Playlist = ID
                });
            }
        }

        [RelayCommand]
        public void RemoveTrack(tracK? tracK)
        {
            context.TrackPlaylists.Remove(context.TrackPlaylists.FirstOrDefault(e => e.Track == tracK.ID && e.Playlist == ID));
            GetSelectedTracks(context, ID);
        }

        [RelayCommand]
        public void RemoveTag(taG? taG)
        {
            context.PlayistTags.Remove(context.PlayistTags.FirstOrDefault(e => e.Tag == taG.ID && e.PlayList == ID));
            GetSelectedTags(context, ID);
        }

        public void AddTag()
        {
            var ST = context.PlayistTags.FirstOrDefault(e => e.PlayList == ID && e.Tag == selectedTag);
            if (ST == null)
            {
                context.PlayistTags.Add(new PlayistTag
                {
                    Tag = selectedTag,
                    PlayList = ID
                });
            }
            GetSelectedTags(context, ID);
        }

        public void ChangeAutor()
        {
            context.PlayLists.FirstOrDefault(e => e.PlaylistId == ID).UserCreator = selectedAutor;
        }

        public void AcseptChanges()
        {
            context.SaveChanges();
        }
    }
}
