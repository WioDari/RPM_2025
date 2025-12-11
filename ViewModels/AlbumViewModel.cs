using Microsoft.EntityFrameworkCore;
using SpotApp_wpf.Context;
using SpotApp_wpf.Models;
using SpotApp_wpf.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace SpotApp_wpf.ViewModels
{
    internal class AlbumViewModel : BaseViewModel
    {
        public AlbumViewModel()
        {
            var context = new SpotifyContext();
            LoadAlbums();
            genres = new ObservableCollection<string>(context.Genres.OrderBy(g => g.GenreId).Select(g => g.GenreTittle).Distinct().ToList());
            genres.Insert(0, "Все жанры");
            
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
            public List<string> genres { get; set; }
            public TimeOnly duration { get; set; }
        };
        private ObservableCollection<AlbTemplate> _albums { get; set; }
        public ObservableCollection<AlbTemplate> albums
        {
            get => _albums;
            set
            {
                _albums = value;
                UseFilters();
                OnPropertyChanged();
            }
        }
        private ObservableCollection<string> _genres { get; set; }
        public ObservableCollection<string> genres
        {
            get => _genres;
            set
            {
                _genres = value;
                OnPropertyChanged();
            }
        }
        private string _selectedGenre { get; set; } = "Все жанры";
        public string selectedGenre
        {
            get => _selectedGenre;
            set
            {
                _selectedGenre = value;
                UseFilters();
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
                UseFilters();
                OnPropertyChanged();
            }
        }
        private ObservableCollection<AlbTemplate> _allAlbums { get; set; }
        private string _selectedSort { get; set; }
        public string selectedSort
        {
            get => _selectedSort;
            set
            {
                _selectedSort = value;
                UseFilters();
                OnPropertyChanged();
            }
        }

        public void ShowDetails(int id)
        {
            User user = (User)Application.Current.Properties["CurrentUser"];
            if (user.RoleId != 3 || user != null)
            {
                var win = new Views.AlbumDetails();
                if (selectedAlbum != null)
                {
                    win.DataContext = new AlbumDetailViewModel(id);
                }
                win.Show();
            }
            else
            {
                return;
            }
        }

        private void LoadAlbums()
        {
            var context = new SpotifyContext();
            _allAlbums = new ObservableCollection<AlbTemplate>(context.Albums
                .Include(a => a.Artist)
                .Include(a => a.TracksInAlbums)
                .Select(a => new AlbTemplate
                {
                    id = a.AlbumId.ToString(),
                    title = a.AlbumTitle,
                    imgPath = a.CoverPath == null ? "/Resources/placeholder_cover.png" : a.CoverPath,
                    author = a.Artist.ArtistName,
                    tracksCount = a.TracksInAlbums.Where(t => t.AlbumId == a.AlbumId).Count().ToString(),
                    genres = a.GenresInAlbums.Where(a => a.AlbumId == a.Album.AlbumId).Select(a => a.Genre.GenreTittle).ToList(),
                    duration = a.TotalDuration
                })
                .ToList());
            albums = new ObservableCollection<AlbTemplate>(_allAlbums);
        }

        public void UseFilters()
        {
            var tmp = _allAlbums.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                string str = searchText.ToLower();
                tmp = tmp.Where(t =>
                t.title.ToLower().Contains(str) ||
                t.author.ToLower().Contains(str));
            }

            if (selectedGenre != "Все жанры")
            {
                tmp = tmp.Where(t => t.genres.Contains(selectedGenre));
            }

            tmp = selectedSort switch
            {
                "По возрастанию" => tmp.OrderBy(t => t.duration),
                "По убыванию" => tmp.OrderByDescending(t => t.duration),
                _ => tmp.OrderBy(t => t.duration)
            };

            albums.Clear();
            foreach (var album in tmp)
            {
                albums.Add(album);
            }
        }
    }
}
