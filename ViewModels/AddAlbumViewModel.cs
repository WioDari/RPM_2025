using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
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

        private int? _albumId;

        public Album selectedAlbum { get; set; }
        private int _id {  get; set; }
        public int id
        {
            get => _id;
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

        public ICommand AddImageCommand { get; }

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
            chooseGenres.Add(selectedGenre);
            _chooseGenres += selectedGenre + ", "; 
            
            OnPropertyChanged(nameof(chooseGenre));

        }

        public AddAlbumViewModel(int? id = null)
        {
            
            var context = new MusicContext();
            _albumId = id;

            if (id != null)
            {
                selectedAlbum = context.Albums
                    .Include(a => a.Artist)
                    .FirstOrDefault(a => a.AlbumId == id);

                if (selectedAlbum != null)
                {
                    nameAlbum = selectedAlbum.AlbumName;
                    selectedArtist = selectedAlbum.Artist?.ArtistName;
                    releaseYears = selectedAlbum.ReleaseYear;
                    totaldur = selectedAlbum.TotalDur ?? TimeOnly.FromTimeSpan(TimeSpan.Zero);
                    imagePath = selectedAlbum.CoverPath;

                    var albumGenres = context.AlbumGenres
                        .Where(ag => ag.AlbumId == id)
                        .Select(ag => ag.Genre.GenreName)
                        .ToList();

                    foreach (var g in albumGenres)
                    {
                        chooseGenres.Add(g);
                        _chooseGenres += g + ", ";
                    }

                    OnPropertyChanged(nameof(chooseGenre));
                }
            }
            else
            {
                selectedAlbum = new Album(); 
            }
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
            AddImageCommand = new RelayCommand(AddImage);
        }

        private void SaveAlbum()
        {
         
            using var context = new MusicContext();

            Album album;

            if (_albumId != null) 
            {
                album = context.Albums.FirstOrDefault(a => a.AlbumId == _albumId);

                if (album == null) return;

                album.AlbumName = nameAlbum;
                album.ReleaseYear = releaseYears;
                album.TotalDur = totaldur;
                album.ArtistId = context.Artists.Where(a => a.ArtistName == selectedArtist).Select(a => a.ArtistId).FirstOrDefault();
                album.CoverPath = imagePath;
                context.Albums.Update(album);

                var oldGenres = context.AlbumGenres.Where(ag => ag.AlbumId == album.AlbumId);

                context.AlbumGenres.RemoveRange(oldGenres);
                LoadData();
            }
            else 
            {
                album = new Album
                {
                    AlbumId = context.Albums.OrderBy(s => s.AlbumId).LastOrDefault().AlbumId + 1,
                    AlbumName = nameAlbum,
                    ReleaseYear = releaseYears,
                    TotalDur = totaldur,
                    CoverPath = imagePath,
                    ArtistId = context.Artists
                        .Where(a => a.ArtistName == selectedArtist)
                        .Select(a => a.ArtistId)
                        .FirstOrDefault()
                };

                context.Albums.Add(album);
                context.SaveChanges();
                LoadData();
            }

            int albumId = album.AlbumId;

            var genreIds = context.Genres
                .Where(g => chooseGenres.Contains(g.GenreName))
                .Select(g => g.GenreId)
                .ToList();

            foreach (var genreId in genreIds)
            {
                context.AlbumGenres.Add(new AlbumGenre
                {
                    AlbumGenreId = context.AlbumGenres.OrderBy(o => o.AlbumGenreId).LastOrDefault().AlbumGenreId + 1,
                    AlbumId = albumId,
                    GenreId = genreId
                });
                context.SaveChanges();
            }

            context.SaveChanges();

            MessageBox.Show("Альбом сохранён");
            foreach (Window w in Application.Current.Windows)
            {
                if (w is Views.AddAlbum or Views.AlbumList)
                {
                    w.Close();
                }
            }
           

        }

        public void AddImage()
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp",
                Title = "Выберите обложку альбома"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                imagePath = openFileDialog.FileName;
                
            }
        }


    }
}
