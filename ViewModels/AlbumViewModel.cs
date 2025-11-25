using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Spotify_wpf.Context;

namespace Spotify_wpf.ViewModels
{
    class AlbumViewModel :BaseViewModel
    {
        private ObservableCollection<AlbumView> _album;

        public ObservableCollection<AlbumView> albums
        {
            get => _album;

            set
            {
                _album = value;
                OnPropertyChanged();
            }
        }
        public ICommand ViewAlbumCommand { get; set; }

        private AlbumView _selectedAlbim {  get; set; }
        public AlbumView selectedAlbim
        {
            get => _selectedAlbim;
            set
            {
                _selectedAlbim = value;
                ViewAlbum(int.Parse(selectedAlbim.id));
                OnPropertyChanged();
            }
        }

        public void ViewAlbum(int id)
        {
            var w = new Views.AlbumList();
            if (selectedAlbim != null)
            {
                w.DataContext = new TrackViewModel(id);
            }
            w.Show();
         
           
        }

        public AlbumViewModel()
        {
            LoadAlbums();
        }



        public class AlbumView
        {

            public string id { get; set; }
            public string nameartist { get; set; }
            public string nametrack { get; set; }

            public string trackcount { get; set; }

            public string imageLink { get; set; }
            

        }


        public void LoadAlbums()
        {
            var context = new MusicContext();
            _album = new ObservableCollection<AlbumView>(context.Albums
                .Include(a => a.AlbumTracks)
                .Include(a => a.Artist)
                .Select(a => new AlbumView
                {
                    id = a.AlbumId.ToString(),
                    nametrack = a.AlbumName,
                    nameartist = a.Artist.ArtistName,
                    trackcount = $"Треков: {a.AlbumTracks.Where(ab => ab.AlbumId == a.AlbumId).Count().ToString()}",
                    imageLink = a.CoverPath == null ? $"/data/icon.png" : a.CoverPath

                })
                .ToList());
                

                
        }
          
    
    }
}
