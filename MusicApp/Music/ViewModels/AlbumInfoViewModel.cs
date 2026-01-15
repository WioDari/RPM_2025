using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Music.Context;
using Music.Models;
using Music.Views.Items;
using static Music.ViewModels.AlbumViewModel;
using static Music.ViewModels.TrackViewModel;

namespace Music.ViewModels
{
    public class AlbumInfoViewModel:BaseViewModel
    {
        private string _albumName ;
        public string albumName
        {
            get => _albumName;
            set
            {
                _albumName = value;
            }
        }
        private string _albumArtist;
        public string albumArtist
        {
            get => _albumArtist;
            set
            {
                _albumArtist = value;
            }
        }
        private string _albumGenres;
        public string albumGenres
        {
            get => _albumGenres;
            set
            {
                _albumGenres = value;
            }
        }
        private string _albumCoverPath;
        public string albumCoverPath
        {
            get => _albumCoverPath;
            set
            {
                _albumCoverPath = value;
            }
        }
        private Page _currentPage;
        public Page currentPage
        {
            get => _currentPage;
            set
            {
                _currentPage = value;
                OnPropertyChanged();
            }
        }
     

        private string _id;
        public AlbumInfoViewModel(AlbumView album)
        {
            currentPage =new AllTrackPage();
            currentPage.DataContext = new TrackViewModel(int.Parse(album.id));
            _id = album.id;
            albumName = album.name;
            albumCoverPath = album.coverPath;
            albumArtist = album.artistName;
            foreach (var g in album.genres) {
                albumGenres += g.GenreName + ";";
            }
        }
   
    }
}
