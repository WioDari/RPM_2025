using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Spotify_wpf.Context;
using Spotify_wpf.Models;

namespace Spotify_wpf.ViewModels
{
    class AddAlbumViewModel :BaseViewModel
    {
        public ObservableCollection<string> genres { get; set; } 

        private ObservableCollection<Genre> _allGenres;

        public ObservableCollection<string> artists { get; set; }
        private ObservableCollection<Artist> _allArtists;

       

        public Album selectedAlbum { get; set; }
        private int _id {  get; set; }
        public int id
        {
            get => id;
            set
            {
                _id = value;
                selectedAlbum.AlbumId = value;
                OnPropertyChanged();
            }
        }
        private string _namealbum {  get; set; }
        public string nameAlbum
        {
            get => _namealbum;
            set
            {
                _namealbum = value;
                //selectedAlbum.AlbumName = value;
                OnPropertyChanged();
            }
        }

        private string _selectedartist { get; set; }
        public string selectedArtist
        {
            get => _selectedartist;
            set
            {
                _selectedartist = value;
                OnPropertyChanged();
            }
        }

        private string _selectedGenre {  get; set; } 
        public string selectedGenre
        {
            get => _selectedGenre;
            set
            {
                _selectedGenre = value;
                OnPropertyChanged();
            }
        }

        private DateTime _releaseyear  = DateTime.UtcNow;
        public DateTime releaseYear
        {
            get => _releaseyear;
            set
            {
                _releaseyear = value;
                //selectedAlbum.ReleaseYear = value;
                OnPropertyChanged();
            }
        }

        private int _releaseyears;
        public int releaseYears
        {
            get => _releaseyears;
            set
            {
                _releaseyears = value;
                //selectedAlbum.ReleaseYear = value;
                OnPropertyChanged();
            }
        }

        private TimeOnly _totaldur { get; set; }
        public TimeOnly totaldur
        {
            get => _totaldur;
            set
            {
                _totaldur = value;
                //selectedAlbum.TotalDur = value;
                OnPropertyChanged();
            }
        }

        private string _imagePath { get; set; }
        public string imagePath
        {
            get => _imagePath;
            set
            {
                _imagePath = value;
                OnPropertyChanged();
            }
        }

        public ICommand SelectImageCommand { get; }
        public ICommand DeleteAlbumCommand { get; }
        public ICommand SaveAlbumCommand { get; }

        public ICommand AddGenre { get; }

        private ObservableCollection<string> chooseGenres { get; set; } = new ObservableCollection<string>();
        private string _chooseGenres {  get; set; }
        public string chooseGenre
        {
            get => _chooseGenres;
            set
            {
                _chooseGenres = value;
                OnPropertyChanged();
            }
        }
        

        public void LoadData()
        {
            var context = new MusicContext();
            genres = new ObservableCollection<string>(context.Genres.Select(g => g.GenreName));
            artists = new ObservableCollection<string>(context.Artists.Select(g => g.ArtistName));

        }

        public void AddGenres()
        {
            chooseGenres.Add(_chooseGenres);
            _chooseGenres += selectedGenre + ", "; 
            
            OnPropertyChanged(nameof(chooseGenre));

        }

        public AddAlbumViewModel()
        {
            var context = new MusicContext();
            var genresList = context.Genres
                .Select(g => g.GenreName)
                .Distinct()
                .OrderBy(s => s)
                .ToList();
            genres = new ObservableCollection<string>(genresList);

            var artistList = context.Artists
                .Select(s => s.ArtistName)
                .Distinct()
                .OrderBy(s => s)
                .ToList();

            LoadData();
            AddGenre = new RelayCommand(AddGenres);
            SaveAlbumCommand = new RelayCommand(SaveAlbum);

        }

        private void SaveAlbum()
        {
            MusicContext context = new MusicContext();
            Album album = new Album
            {
                AlbumId = context.Albums.OrderBy(s => s.AlbumId).LastOrDefault().AlbumId + 1,
                AlbumName = nameAlbum,
                //ReleaseYear = Convert.ToInt32(releaseYear.ToString("yyyy")),
                ReleaseYear = releaseYears,
                TotalDur = TimeOnly.FromTimeSpan(TimeSpan.Zero),
                CoverPath = "/nety",
                ArtistId = context.Artists.Where(a => a.ArtistName == selectedArtist).FirstOrDefault().ArtistId,

            };
            context.Albums.Add(album);
            context.SaveChanges();
            int albumIds = album.AlbumId;

            var genreIds = context.Genres.Where(g => chooseGenre.Contains(g.GenreName)).Select(g => g.GenreId).ToList();

           /* for (int i = 0; i < genres.Count; i++)
            {
                AlbumGenre albgenre = new AlbumGenre
                {
                    AlbumGenreId = context.AlbumGenres.OrderBy(o => o.AlbumGenreId).LastOrDefault().AlbumGenreId + 1,
                    AlbumId = context.Albums.OrderBy(a => a.AlbumId).FirstOrDefault().AlbumId,
                    GenreId = context.Genres.Where(g => g.GenreName == genres[i]).Select(g => g.GenreId).FirstOrDefault(),

                };
                context.AlbumGenres.Add(albgenre);
                context.SaveChanges();
            }*/

            foreach (var genreId in genreIds)
            {
                context.AlbumGenres.Add(new AlbumGenre
                {
                    AlbumGenreId = context.AlbumGenres.OrderBy(o => o.AlbumGenreId).LastOrDefault().AlbumGenreId + 1,
                    AlbumId = albumIds,
                    GenreId = genreId,

                });
               
                context.SaveChanges();

            }
            MessageBox.Show("Успех", "Вы успешно добавили альбом");
        }





    }
}
