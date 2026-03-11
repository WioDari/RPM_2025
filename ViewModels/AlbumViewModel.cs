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
using Spotify_wpf.Models;
using Spotify_wpf.Views;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Spotify_wpf.ViewModels
{
    class AlbumViewModel :BaseViewModel
    {
        private ObservableCollection<AlbumView> _album;
        public ObservableCollection<string> genres {  get; set; }

        private ObservableCollection<Genre> _allGenres;

        private ObservableCollection<AlbumView> _allAlbums;
        public ObservableCollection<AlbumView> albums
        {
            get => _album;

            set
            {
                _album = value;
                ApplyFilters();
                OnPropertyChanged();
            }
        }

        public ICommand GoAddAlbum { get; }

        private string _selectedGenres { get; set; } = "Жанры";
        public string selectedGenres
        {
            get => _selectedGenres;
            set
            {
                _selectedGenres = value;
                ApplyFilters();
                OnPropertyChanged();
            }
        }

        private string _selectedSort { get; set; }
        public string selectedSort
        {
            get => _selectedSort;
            set
            {
                _selectedSort = value;
                ApplyFilters();
                OnPropertyChanged();
            }
        }

        private string _searchText { get; set; }
        public string searchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                ApplyFilters();
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

        public void GoAddAlbums()
        {
            Window window = new AddAlbum();
            window.Show();
        }

        public AlbumViewModel()
        {
            LoadAlbums();
            GoAddAlbum = new RelayCommand(GoAddAlbums);
            var context = new MusicContext();
            var genresList = context.Genres 
                .Select(g => g.GenreName)
                .Distinct()
                .OrderBy(s => s)
                .ToList();
            genresList.Insert(0, "Жанры");
            genres = new ObservableCollection<string>(genresList);
        }



        public class AlbumView
        {

            public string id { get; set; }
            public string nameartist { get; set; }
            public string nametrack { get; set; }

            public string trackcount { get; set; }

            public string totdur { get; set; }
            public string imageLink { get; set; }

            public List<string> genreses { get; set; }
            

        }


        public void LoadAlbums()
        {
            var context = new MusicContext();

            _allAlbums = new ObservableCollection<AlbumView>(context.Albums
                .Include(a => a.AlbumTracks)
                .Include(a => a.Artist)
                .Select(a => new AlbumView
                {
                    id = a.AlbumId.ToString(),
                    nametrack = a.AlbumName,
                    nameartist = a.Artist.ArtistName,
                    trackcount = $"Треков: {a.AlbumTracks.Where(ab => ab.AlbumId == a.AlbumId).Count().ToString()}",
                    imageLink = a.CoverPath == null ? $"/data/icon.png" : a.CoverPath,
                    genreses = a.AlbumGenres.Where(b => b.AlbumId == a.AlbumId).Select(a => a.Genre.GenreName).ToList(),
                    totdur = a.TotalDur.ToString(),
                })
                .ToList());
            _album = new ObservableCollection<AlbumView>(_allAlbums);

                
        }

        private void ApplyFilters()
        {
            //var queryGenr = _allGenres.AsQueryable();
            var queryAlbum = _allAlbums.AsQueryable();

            if (selectedGenres != "Жанры")
            {
                queryAlbum = queryAlbum.Where(g => g.genreses.Contains(selectedGenres));

            }

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                string temp = searchText.ToLower();
                queryAlbum = queryAlbum.Where(t =>
                t.nameartist.ToLower().Contains(temp) | 
                t.nametrack.ToLower().Contains(temp));
            }

            queryAlbum = selectedSort switch
            {
                "По возрастанию" => queryAlbum.OrderBy(a => a.totdur),
                "По убыванию" => queryAlbum.OrderByDescending(a => a.totdur),
                _ => queryAlbum
            };

            albums.Clear();
            foreach (var a in queryAlbum)
                albums.Add(a);
        }

        
          
    
    }
}
