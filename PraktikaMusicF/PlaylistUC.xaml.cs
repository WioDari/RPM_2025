using PraktikaMusicF.Context;
using PraktikaMusicF.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PraktikaMusicF
{
    /// <summary>
    /// Логика взаимодействия для PlaylistUC.xaml
    /// </summary>
    public partial class PlaylistUC : UserControl, INotifyPropertyChanged
    {
        public ObservableCollection<Playlist> _playlist;
        public ObservableCollection<Playlist> playlist
        {
            get => _playlist;
            set
            {
                _playlist = value;
                OnPropertyChanged(nameof(playlist));
            }
        }
        public ObservableCollection<Playlist> _allPlaylist;

        
        public PlaylistUC()
        {
            InitializeComponent();
        }

        public class PlaylistV
        {
            public string NameAuthor { get; set; }
            public string Likes { get; set; }
            public int Subscribers { get; set; }
            public string Tracks { get; set; }
            public string Created { get; set; }
            public string Duration { get; set; }
        }

        public void LoadPlaylist(Playlist pl)
        {
            using MusicBdFContext mbdf = new();

            var playlistInfo = mbdf.Playlists
                .Where(p => p.PlId == pl.PlId)
                .Select(p => new
                {
                    PlaylistName = p.PlName,
                    CreatorNames = p.PlaylistCreators
                                    .Where(pc => pc.Usr != null)
                                    .Select(pc => pc.Usr!.UsrFName)
                                    .ToList(),
                    SubscriberNames = p.Users
                                    .Select(u => u.UsrFName!)
                                    .ToList(),
                    TotalDuration = p.TotalDuration

                })
                .FirstOrDefault();

            if (playlistInfo == null)
                return;

            DataContext = new PlaylistV
            {
                NameAuthor = $"{playlistInfo.PlaylistName} | {string.Join(", ", playlistInfo.CreatorNames)}",
                Likes = pl.PlLikes?.ToString() ?? "0",
                Subscribers = playlistInfo.SubscriberNames.Count,
                Tracks = pl.PlTracksQuantity?.ToString() ?? "0",
                Created = pl.PlDateCreate?.ToString("dd.MM.yyyy") ?? "Не указана",
                Duration = playlistInfo.TotalDuration.HasValue
                   ? playlistInfo.TotalDuration.Value.ToString("HH:mm:ss")
                   : "00:00:00"
            };

        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
