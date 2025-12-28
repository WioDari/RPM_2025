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
    public class TracksViewModel : BaseViewModel
    {
        private ObservableCollection<TrackVM> _tracks;
        public ObservableCollection<TrackVM> track
        {
            get => _tracks;
            set
            {
                _tracks = value;
                OnPropertyChanged();
            }
        }

        private TrackVM _selectedTrack;
        public TrackVM selectedTrack
        {
            get => _selectedTrack;
            set
            {
                _selectedTrack = value;
                OnPropertyChanged();
            }
        }

        public TracksViewModel()
        {
            LoadTracks();
        }

        public class TrackVM
        {
            public string nameTrack { get; set; }
            public string artistTrack { get; set; }
            public string durationTrack { get; set; }
            public string image { get; set; }
        }

        public ICommand AddTrackCommand { get; private set; }


        private void LoadTracks()
        {

            try
            {
                using var context = new MusicContext();

                var tracksData = context.Tracks
                    .Include(t => t.ArtistTracks)
                        .ThenInclude(at => at.Artist)
                    .Select(t => new TrackVM
                    {
                        nameTrack = t.TrackName,
                        artistTrack = //GetArtistsForTrack(t)
                        string.Join(", ", t.ArtistTracks
                                .Where(at => at.Artist != null)
                                .Select(at => at.Artist.ArtistName)
                                .ToList()),

                        durationTrack = FormatDuration(t.Duration),
                        image = t.AlbumCoverPath
                    })
                    .OrderBy(t => t.nameTrack)
                    .ToList();

                track = new ObservableCollection<TrackVM>(tracksData);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке треков: {ex.Message}");
                track = new ObservableCollection<TrackVM>();
            }

            AddTrackCommand = new RelayCommand(OpenAddTrackWindow);
        }

        private void OpenAddTrackWindow()
        {
            var window = new Views.AddTrack();
            window.Show();

        }

        private static string FormatDuration(TimeOnly duration)
        {
            if (duration.Hour > 0)
            {
                return duration.ToString(@"hh\:mm\:ss");
            }
            else
            {
                return duration.ToString(@"mm\:ss");
            }
        }

    }
}
