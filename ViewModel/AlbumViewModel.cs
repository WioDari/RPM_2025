using Microsoft.EntityFrameworkCore;
using MusicWpf.Context;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static MusicWpf.ViewModel.PlaylisyViewModel;

namespace MusicWpf.ViewModel
{

    public class AlbumViewModel : BaseViewModel
    {

        private ObservableCollection<AlbumVM> _albums;

       public ObservableCollection<AlbumVM> albums
        {
            get => _albums;

            set
            {
                _albums = value;
                OnPropertyChanged();
            }
        }



        public ICommand ViewAlbumCommand { get; set; }

        private AlbumVM _selectedAlbim { get; set; }
        public AlbumVM selectedAlbim
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
            var w = new Views.Album();
            if (selectedAlbim != null)
            {
                w.DataContext = new TracksViewModel(id);
            }
            w.Show();


        }
        public AlbumViewModel()
        {
            LoadAlbums();
        }
        public class AlbumVM
        {
            public string id { get; set; }
            public string title { get; set; }
            public string author { get; set; }
            public string tracksCount { get; set; }
            public string image { get; set; }   
        };

        private void LoadAlbums()
        {
            var context = new MusicContext();
            _albums = new ObservableCollection<AlbumVM>(context.Albums
                .Include(a => a.Artist)
                .Include(a => a.AlbumTracks)
                .Select(a => new AlbumVM
                {
                    id = a.AlbumId.ToString(),
                    title = a.AlbumTitle,
                    author = a.Artist.ArtistName,
                    tracksCount = a.AlbumTracks.Where(at => at.AlbumId == a.AlbumId).Count().ToString(),
                    image = a.CoverPath
                })
                .OrderBy(a => a.id)
                .ToList());
        }
    }
}
