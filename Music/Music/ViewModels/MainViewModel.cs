using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Music.Context;

namespace Music.ViewModels
{
    class MainViewModel : BaseViewModel
    {
        public int trackCount
        {
            get => _trackCount;
            set
            {
                _trackCount = value;
                OnPropertyChanged();
            }
        }
        private int _trackCount { get; set; }

        public int albumCount
        {
            get => _albumCount;
            set
            {
                _albumCount = value;
                OnPropertyChanged();
            }
        }
        private int _albumCount { get; set; }
        public int playlistCount
        {
            get => _playlistCount;
            set
            {
                _playlistCount = value;
                OnPropertyChanged();
            }
        }
        private int _playlistCount { get; set; }
        public MainViewModel() {
            var context = new MusicDbContext();
            _trackCount = context.Tracks.Count();
            _albumCount = context.Albums.Count();
            _playlistCount = context.Playlists.Count();
        }
    }
}
