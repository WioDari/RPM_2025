using Microsoft.EntityFrameworkCore;
using SpotApp_wpf.Context;
using SpotApp_wpf.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotApp_wpf.ViewModels
{
    internal class AlbumViewModel : BaseViewModel
    {
        public AlbumViewModel()
        {
            LoadAlbums();
        }

        private AlbTemplate _selectedAlbum {  get; set; }
        public AlbTemplate selectedAlbum
        {
            get => _selectedAlbum;
            set
            {
                _selectedAlbum = value;
                ShowDetails(int.Parse(selectedAlbum.id));
                OnPropertyChanged();
            }
        }

        public class AlbTemplate
        {
            public string id { get; set; }
            public string title { get; set; }
            public string imgPath { get; set; }
            public string author { get; set; }
            public string tracksCount { get; set; }
        };
        public ObservableCollection<AlbTemplate> albums { get; set; }

        public void ShowDetails(int id)
        {
            var win = new Views.AlbumDetails();
            if(selectedAlbum != null)
            {
                win.DataContext = new AlbumDetailViewModel(id);
            }
            win.Show();
        }

        private void LoadAlbums()
        {
            var context = new SpotifyContext();
            albums = new ObservableCollection<AlbTemplate>(context.Albums
                .Include(a => a.Artist)
                .Include(a => a.TracksInAlbums)
                .Select(a => new AlbTemplate
                {
                    id = a.AlbumId.ToString(),
                    title = a.AlbumTitle,
                    imgPath = a.CoverPath == null ? "/Resources/placeholder_cover.png" : a.CoverPath,
                    author = a.Artist.ArtistName,
                    tracksCount = a.TracksInAlbums.Where(t => t.AlbumId == a.AlbumId).Count().ToString()
                })
                .OrderBy(a => a.id)
                .ToList());
        }
    }
}
