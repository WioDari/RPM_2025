using Microsoft.EntityFrameworkCore;
using MusicWpf.Context;
using MusicWpf.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MusicWpf.ViewModel
{
    internal class PlaylistViewModel2 : BaseViewModel
    {
        private ObservableCollection<PlayTrackVM> _playlistList;
        public ObservableCollection<PlayTrackVM> playlistList
        {
            get => _playlistList;
            set
            {
                _playlistList = value;
                OnPropertyChanged();
            }
        }

        private string _nameplaylist;
        public string nameplaylist
        {
            get => _nameplaylist;
            set
            {
                _nameplaylist = value;
                OnPropertyChanged();
            }
        }

        private string _nameavtor;
        public string nameavtor
        {
            get => _nameavtor;
            set
            {
                _nameavtor = value;
                OnPropertyChanged();
            }
        }

        private string _date;
        public string date
        {
            get => _date;
            set
            {
                _date = value;
                OnPropertyChanged();
            }
        }

        private string _like;
        public string like
        {
            get => _like;
            set
            {
                _like = value;
                OnPropertyChanged();
            }
        }

        private string _duration;
        public string duration
        {
            get => _duration;
            set
            {
                _duration = value;
                OnPropertyChanged();
            }
        }


        public PlaylistViewModel2(int playlistId)
        {
            _playlistId = playlistId;
            LoadPlaylist2(playlistId);
            EditPaylisyCommand = new RelayCommand(OpenEditPlaylisyWindow);
        }

        public class PlayTrackVM
        {
            public string trackNamePl { get; set; }
            public string trackArtistPl { get; set; }
            public string countlistenPl { get; set; }
            public string ratingPl { get; set; }
            public string durationPl { get; set; }
        }

        public ICommand EditPaylisyCommand { get; set; }
        private int _playlistId;

        private void OpenEditPlaylisyWindow()
        {
            var window = new EditPlaylist(_playlistId);
            window.Show();

            if (window.DialogResult == true)
            {
                LoadPlaylist2(_playlistId);
            }

        }
        public void LoadPlaylist2(int id)
        {
            try
            {
                var context = new MusicContext();

                var playlist = context.Playlists
                    .Include(p => p.User)
                    .Include(p => p.TracksPlaylists)
                        .ThenInclude(tp => tp.Tracks)
                    .FirstOrDefault(p => p.PlaylistId == id);

                if (playlist == null)
                {
                    nameplaylist = "Плейлист не найден";
                    return;
                }

                nameavtor = playlist.User?.FullName ?? "Неизвестный автор";
                nameplaylist = $"{playlist.PlaylistName} | {nameavtor}";
                date = $"Дата создания: {playlist.DateCreated:dd.MM.yyyy}";
                like = $"Понравилось: {playlist.Likes}";

                var tracksData = context.TracksPlaylists
                    .Where(tp => tp.PlaylistId == id)
                    .Include(tp => tp.Tracks)
                        .ThenInclude(t => t.ArtistTracks)
                        .ThenInclude(at => at.Artist)
                    .Select(tp => new PlayTrackVM
                    {
                        trackNamePl = tp.Tracks.TrackName,
                        countlistenPl = tp.Tracks.PlayCount.ToString(),
                        ratingPl = tp.Tracks.Rating.ToString("F1"),
                        durationPl = tp.Tracks.Duration.ToString(@"mm\:ss"),
                        trackArtistPl = string.Join(", ", tp.Tracks.ArtistTracks.Select(at => at.Artist.ArtistName))
                    })
                    .ToList();

                playlistList = new ObservableCollection<PlayTrackVM>(tracksData);

                var totalDuration = CalculateTotalDuration(playlist);
                duration = $"Продолжительность: {totalDuration}";
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Ошибка при загрузке плейлиста: {ex.Message}");
            }
        }

        private string CalculateTotalDuration(Models.Playlist playlist)
        {
            try
            {
                var tracks = playlist.TracksPlaylists?
                    .Where(tp => tp.Tracks != null)
                    .Select(tp => tp.Tracks)
                    .ToList();

                if (tracks == null || !tracks.Any())
                {
                    return "0:00";
                }

                var totalSeconds = tracks.Sum(t =>
                {
                    var timeOnly = t.Duration;
                    return timeOnly.Hour * 3600 + timeOnly.Minute * 60 + timeOnly.Second;
                });

                var timeSpan = TimeSpan.FromSeconds(totalSeconds);

                if (timeSpan.TotalHours >= 1)
                {
                    return $"{(int)timeSpan.TotalHours}:{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
                }
                else
                {
                    return $"{timeSpan.Minutes}:{timeSpan.Seconds:D2}";
                }
            }
            catch (Exception)
            {
                return "0:00";
            }
        }
    }
}

