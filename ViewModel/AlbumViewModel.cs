using Microsoft.EntityFrameworkCore;
using MusicWpf.Context;
using MusicWpf.Models;
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

        private ObservableCollection<AlbumVM> _album;
        public ObservableCollection<string> genres{  get; set; }

        private ObservableCollection<Genre> _allGenres;

        private ObservableCollection<AlbumVM> _albums;
        public ObservableCollection<AlbumVM> albums
        {
            get => _albums;

            set
            {
                _albums = value;
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


        public void ViewAlbum(int id)
        {
            var w = new Views.Album();
            if (selectedAlbim != null)
            {
                w.DataContext = new AlbumViewModel_track_(id);
            }
            w.Show();


        }

        public ICommand AddAlbumCommand { get; private set; }
        public AlbumViewModel()
        {
            LoadAlbums();
            AddAlbumCommand = new RelayCommand(OpenAddAlbumWindow);

            var context = new MusicContext();
            var generes = context.Genres
                .Select(g => g.GenreName)
                .Distinct()
                .OrderBy(s => s)
                .ToList();
            generes.Insert(0, "Жанры");
            genres = new ObservableCollection<string>(generes);
        }

        private void OpenAddAlbumWindow()
        {
            var window = new Views.AddAlbum();
            window.Show();                     
                                             
        }
        public class AlbumVM
        { 
            public string id { get; set; }
            public string title { get; set; }
            public string author { get; set; }
            public string tracksCount { get; set; }
            public double totalSeconds { get; set; }
            public string totdur {  get; set; } 
            public string image { get; set; }
            public List<string> genreses { get; set; }

        };

        /*       private void LoadAlbums()
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

                           *//* totdur = a.AlbumTracks.Sum(at => at.Tracks.Duration ?? 0),*//*
                           //totdur = a.AlbumTracks.Sum(at => at.Tracks.Duration.ToTimeSpan().TotalSeconds).ToString(),
                           totalSeconds = a.AlbumTracks.Sum(at => at.Tracks.Duration.ToTimeSpan().TotalSeconds),

                           totdur = TimeSpan.FromSeconds(a.AlbumTracks.Sum(at => at.Tracks.Duration.ToTimeSpan().TotalSeconds)).ToString(@"mm\:ss"),

                           tracksCount = a.AlbumTracks.Where(at => at.AlbumId == a.AlbumId).Count().ToString(),
                           image = a.CoverPath,
                           genreses = a.AlbumGenres.Where(b => b.AlbumId == a.AlbumId).Select(a => a.Genre.GenreName).ToList()
                       })
                       .OrderBy(a => a.id)
                       .ToList());
               }*/

        private void LoadAlbums()
        {
            var context = new MusicContext();
            var list = context.Albums
                .Include(a => a.Artist)
                .Include(a => a.AlbumTracks)
                .Select(a => new AlbumVM
                {
                    id = a.AlbumId.ToString(),
                    title = a.AlbumTitle,
                    author = a.Artist.ArtistName,
                    totalSeconds = a.AlbumTracks.Sum(at => at.Tracks.Duration.ToTimeSpan().TotalSeconds),
                    totdur = TimeSpan.FromSeconds(a.AlbumTracks.Sum(at => at.Tracks.Duration.ToTimeSpan().TotalSeconds)).ToString(@"mm\:ss"),
                    tracksCount = a.AlbumTracks.Count(at => at.AlbumId == a.AlbumId).ToString(),
                    image = a.CoverPath,
                    genreses = a.AlbumGenres.Where(b => b.AlbumId == a.AlbumId).Select(g => g.Genre.GenreName).ToList()
                })
                .OrderBy(a => a.id)
                .ToList();
           
            _album = new ObservableCollection<AlbumVM>(list);

    
            albums = new ObservableCollection<AlbumVM>();

         
            ApplyFilters();


        }



        private void ApplyFilters()
        {
            /*var queryAlbum = _albums.AsQueryable();

            {
                queryAlbum = queryAlbum.Where(g => g.genreses.Contains(selectedGenres));

            }

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                string temp = searchText.ToLower();
                queryAlbum = queryAlbum.Where(t =>
                t.author.ToLower().Contains(temp) |
                t.author.ToLower().Contains(temp));
            }

            queryAlbum = selectedSort switch
            {
                "По возврастанию" => queryAlbum.OrderBy(a => a.totdur),
                "По убыванию" => queryAlbum.OrderByDescending(a => a.totdur),
                _ => queryAlbum
            };

            albums.Clear();
            foreach (var a in queryAlbum)
                albums.Add(a);*/

            if (_album == null) return;

            var queryAlbum = _album.AsQueryable();
            if (!string.IsNullOrWhiteSpace(selectedGenres) && selectedGenres != "Жанры")
            {
                string sel = selectedGenres.Trim().ToLower();
                queryAlbum = queryAlbum.Where(g => g.genreses.Any(gs => (gs ?? string.Empty).Trim().ToLower() == sel));
            }

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                string temp = searchText.ToLower();
                queryAlbum = queryAlbum.Where(t =>
                    (t.author ?? string.Empty).ToLower().Contains(temp) ||
                    (t.title ?? string.Empty).ToLower().Contains(temp));
            }

            queryAlbum = selectedSort switch
            {
                "По возврастанию" => queryAlbum.OrderBy(a => a.totalSeconds),
                "По убыванию" => queryAlbum.OrderByDescending(a => a.totalSeconds),
                _ => queryAlbum
            };

            albums.Clear();
            foreach (var a in queryAlbum)
                albums.Add(a);
        }

    }

   
}
