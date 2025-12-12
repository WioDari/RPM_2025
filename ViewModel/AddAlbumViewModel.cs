using MusicWpf.Context;
using MusicWpf.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MusicWpf.ViewModel
{
    class AddAlbumViewModel : BaseViewModel
    {
        private string _titleAlbum;
        public string TitleAlbum
        {
            get => _titleAlbum;

            set
            {
                _titleAlbum = value;
                OnPropertyChanged();
            }
            
        }

        private int _releaseYear;
        public int ReleaseYear
        {
            get => _releaseYear;
            set
            {
                _releaseYear = value;
                OnPropertyChanged();
            }
        }


        private string _coverPath;
        public string CoverPath
        {
            get => _coverPath;
            set
            {
                _coverPath = value;
                OnPropertyChanged();
            }
        }



        private ObservableCollection<Artist> _artists;
        public ObservableCollection<Artist> Artists
        {
            get => _artists;
            set 
            { 
                _artists = value;
                OnPropertyChanged(); 
            }
        }

        private string _genereAlbum;
        public string GenereAlbum
        {
            get => _genereAlbum;
            set
            {
                _genereAlbum = value;
                OnPropertyChanged();
            }
        }


        private Artist _selectedArtist;
        public Artist SelectedArtist
        {
            get => _selectedArtist;
            set 
            { 
                _selectedArtist = value; 
                OnPropertyChanged(); 
            }
        }
        public ICommand AlbumCommand { get;}
        public AddAlbumViewModel()
        {
            using var dbMusic = new MusicContext();
            Artists = new ObservableCollection<Artist>(dbMusic.Artists.ToList());
        }

        public void AlbumCreat(object obj)
        {
            using var db = new MusicContext();
            var album = new Album
            {
                AlbumTitle = TitleAlbum,
                ReleaseYear = ReleaseYear,
                ArtistId = SelectedArtist?.ArtistId,
                CoverPath = CoverPath ?? "",
                TotalDuration = 0
            };

            db.Albums.Add(album);
            db.SaveChanges();


        }
    }
}
