using Microsoft.EntityFrameworkCore;
using MusicWpf.Context;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;


namespace MusicWpf.ViewModel
{
    public class PlaylisyViewModel : BaseViewModel
    {
        private ObservableCollection<PlaylistVM> _playlists { get; set; }

        public ObservableCollection<PlaylistVM> playlists
        {
            get => _playlists;

            set
            {
                _playlists = value;
                OnPropertyChanged();
            }
        }

        public class PlaylistVM
        {
            public string id { get; set; }
            public string title { get; set; }
            public string likes { get; set; }
            public string subs { get; set; }
            public string tracksCount { get; set; }
            public string creationDate { get; set; }
            public string time { get; set; }
        }

        private PlaylistVM _selectedPlaylist { get; set; }
        public PlaylistVM selectedPlaylist
        {
            get => _selectedPlaylist;
            set
            {
                _selectedPlaylist = value;
                if (value != null)
                {
                    ViewPlaylist(int.Parse(value.id));
                }

                OnPropertyChanged();
            }
        }

        public ICommand ViewPlaylistCommand { get; set; }
        public ICommand AddPlaylistCommand { get; private set; }

        public void ViewPlaylist(int id)
        {
            var w = new Views.Playlist();
            w.DataContext = new PlaylistViewModel2(id);
            w.Show();


        }

        private void OpenAddPlaylistWindow()
        {
            var window = new Views.AddPlaylist();
            window.Show();

        }

        public PlaylisyViewModel()
        {
            AddPlaylistCommand = new RelayCommand(OpenAddPlaylistWindow);
            LoadPlaylists();
        }

        private void LoadPlaylists()
        {
            var context = new MusicContext();

            var playlistsData = context.Playlists
                .Include(p => p.User)
                .Include(p => p.PlaylistsUsers)
                .Include(p => p.TracksPlaylists)
                    .ThenInclude(tp => tp.Tracks)
                .ToList();

            var playlistVMs = playlistsData.Select(p => new PlaylistVM
            {
                id = p.PlaylistId.ToString(),
                title = $"{p.PlaylistName} | {p.User?.FullName ?? "Неизвестный автор"}",
                likes = $"Нравится: {p.Likes}",
                subs = $"Подписчиков: {p.PlaylistsUsers?.Count(up => up.PlaylistsId == p.PlaylistId) ?? 0}",
                creationDate = $"Создан: {p.DateCreated:dd.MM.yyyy}",
                tracksCount = $"Количество треков: {p.TracksPlaylists?.Count(tp => tp.PlaylistId == p.PlaylistId) ?? 0}",
                 time = CalculateDuration(p)
            })
            .OrderBy(p => int.Parse(p.id))
            .ToList();

            playlists = new ObservableCollection<PlaylistVM>(playlistVMs);
        }
        private string CalculateDuration(Models.Playlist playlist)
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

