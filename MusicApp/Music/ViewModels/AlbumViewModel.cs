using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Microsoft.EntityFrameworkCore;
using Music.Context;
using Music.Models;
using Music.Views.Windows;

namespace Music.ViewModels
{
    public class AlbumViewModel : BaseViewModel
    {
        public class AlbumView()
        {
            public string id { get; set; }
            public string? coverPath { get; set; }
            public string name { get; set; }
            public string artistName { get; set; }
            public string numberOfTrack { get; set; }
            public List<Genre> genres{ get; set; }
           
        }
       
        public ObservableCollection<AlbumView> data { get; set; }
        private AlbumView _SelectedAlbum;
        public AlbumView SelectedAlbum
        {
            get => _SelectedAlbum;
            set
            {
                _SelectedAlbum = value;
                if (SelectedAlbum != null)
                    showAlbumInfo(value);
                    OnPropertyChanged();
            }
        }
        private void showAlbumInfo(AlbumView album) {
            var w = new AlbumInfoWindow();
            w.DataContext = new AlbumInfoViewModel(album);
            w.ShowDialog();
        }
        public AlbumViewModel()
        {
            loadAlbum();
        }
        private void loadAlbum()
        {
            var context = new MusicDbContext();
            data = new ObservableCollection<AlbumView>(context.Albums
                   .Include(x => x.Artist)
                   .Include(x => x.Tracks)
                   .Select(x => new AlbumView
                   {    
                       id = x.AlbumId.ToString(),
                       name = x.AlbumName,
                       artistName = x.Artist.ArtistName,
                       numberOfTrack = x.Tracks.Count.ToString(),
                       coverPath = File.Exists($"C:\\Users\\glagol\\source\\repos\\Music\\Music\\Resources\\covers\\{x.AlbumName}.jpg")
                       ? $"/Resources/covers/{x.AlbumName}.jpg"
                       : "/Resources/covers/placeholder_cover.png",
                       genres = x.Genres.ToList()

                   }
                   ).OrderBy(x => x.id).ToList());
            foreach (AlbumView a in data) {
                if (a.name == "Cinematic Soundscape")
                { a.coverPath = "/Resources/covers/placeholder_cover.png";  }    
            }
            
        }
    }
}
