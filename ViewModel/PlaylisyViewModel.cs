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
                ViewPlaylist(int.Parse(selectedPlaylist.id));
                OnPropertyChanged();
            }
        }

        public ICommand ViewPlaylistCommand { get; set; }

        public void ViewPlaylist(int id)
        {
            var w = new Views.Playlist();
           if (selectedPlaylist != null)
            {
                w.DataContext = new PlaylistViewModel2(id);
            }
            w.Show();


        }

        public PlaylisyViewModel()
        {
            LoadPlaylists();
        }

        private void LoadPlaylists()
        {
            var context = new MusicContext();
            playlists = new ObservableCollection<PlaylistVM>(context.Playlists
                .Include(p => p.User)
                .Include(p => p.PlaylistsUsers)
                .Include(t => t.TracksPlaylists)
                .Select(p => new PlaylistVM
                {
                    id = p.PlaylistId.ToString(),
                    title = $"{p.PlaylistName} | {p.User.FullName}",
                    likes = $"Нравиться: {p.Likes.ToString()}",
                    subs = $"Подписчиков: { p.PlaylistsUsers.Where(up => up.PlaylistsId == p.PlaylistId).Count().ToString() }",
                    creationDate = $"Создан: {p.DateCreated.ToString("dd.MM.yyy")}",
                    tracksCount = $"Количество треков: {p.TracksPlaylists.Where(tp => tp.PlaylistId == p.PlaylistId).Count().ToString()}",
                    time = "0:00"
                })
                .OrderBy(p => p.id)
                .ToList());  
        }
    }
}
