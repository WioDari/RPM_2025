using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SpotApp_wpf.Context;
using SpotApp_wpf.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SpotApp_wpf.ViewModels
{
    public class AlbumsAddditionViewModel : BaseViewModel
    {
        private string _windowTitle { get; set; }
        public string windowTitle
        {
            get => _windowTitle;
            set
            {
                _windowTitle = value;
                OnPropertyChanged();
            }
        }
        private string _btnTitle { get; set; }
        public string btnTitle
        {
            get => _btnTitle;
            set
            {
                _btnTitle = value;
                OnPropertyChanged();
            }
        }
        private string _coverPath { get; set; }
        public string coverPath
        {
            get => _coverPath;
            set
            {
                _coverPath = value;
                OnPropertyChanged();
            }
        }
        private int? _albumID { get; set; }
        public int? albumID
        {
            get => _albumID;
            set
            {
                _albumID = value;
                OnPropertyChanged();
            }
        }
        private string _title { get; set; }
        public string title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }
        private string _dateCreate { get; set; }
        public string dateCreate
        {
            get => _dateCreate;
            set
            {
                _dateCreate = value;
                OnPropertyChanged();
            }
        }
        private string _author { get; set; }
        public string author
        {
            get => _author;
            set
            {
                _author = value;
                OnPropertyChanged();
            }
        }
        private string _selectedGenre { get; set; }
        public string selectedGenre
        {
            get => _selectedGenre;
            set
            {
                _selectedGenre = value;
                OnPropertyChanged();
            }
        }
        private DateTime _selectedDate = DateTime.UtcNow;
        public DateTime selectedDate
        {
            get => _selectedDate;
            set
            {
                _selectedDate = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<string> authors { get; set; }
        public ObservableCollection<string> genres { get; set; }
        public List<string> choosenGenres { get; set; } = new List<string>();
        private string _choosenGenre {  get; set; }
        public string choosenGenre
        {
            get => _choosenGenre;
            set
            {
                _choosenGenre = value;
                OnPropertyChanged();
            }
        }

        public ICommand GenreAdditionCommand { get; set; }
        public ICommand GenresClearCommand { get; set; }
        public ICommand AlbumAdditionCommand { get; set; }
        private AlbumViewModel _AlbumViewModel;
        public AlbumsAddditionViewModel(int? id = null, AlbumViewModel albumViewModel = null)
        {
            LoadData(id);
            GenreAdditionCommand = new RelayCommand(AddGenre);
            GenresClearCommand = new RelayCommand(ClearGenre);
            AlbumAdditionCommand = new RelayCommand(AddAlbum);
            albumID = id;
            _AlbumViewModel = albumViewModel;
        }

        public void LoadData(int? id = null)
        {
            windowTitle = "Добавление Альбома";
            btnTitle = "Добавить";
            var context = new SpotifyContext();
            genres = new ObservableCollection<string>(context.Genres.Select(g => g.GenreTittle));
            authors = new ObservableCollection<string>(context.Artists.Select(a => a.ArtistName));

            if (id != null)
            {
                windowTitle = "Редактирование Альбома";
                btnTitle = "Сохранить";
                string path = context.Albums.Where(a => a.AlbumId == id).FirstOrDefault().CoverPath;
                coverPath = path == null ? "/Resources/placeholder_cover.png" : path;
                author = context.Albums.Where(a => a.AlbumId == id).Select(a => a.Artist.ArtistName).FirstOrDefault();
                title = context.Albums.Where(a => a.AlbumId == id).FirstOrDefault().AlbumTitle;
                selectedDate = DateTime.Parse($"01.01.{context.Albums.Where(a => a.AlbumId == id).FirstOrDefault().ReleaseYear}");
                choosenGenres = context.GenresInAlbums.Include(g => g.Genre).Where(g => g.AlbumId == id).Select(g => g.Genre.GenreTittle).ToList();
                if (choosenGenres.Count > 0)
                {
                    choosenGenre = choosenGenres[0];
                    for (int i = 1; i < choosenGenres.Count; i++)
                    {
                        choosenGenre += ", " + choosenGenres[i];
                    }
                }
                else
                {
                    choosenGenre = "";
                }
            }
        }
        public void UpdateGenres()
        {
            if (choosenGenres.Count > 0)
            {
                choosenGenre = choosenGenres[0];
                for (int i = 1; i < choosenGenres.Count; i++)
                {
                    choosenGenre += ", " + choosenGenres[i];
                }
            }
            else
            {
                choosenGenre = string.Empty;
            }
        }

        public void AddGenre()
        {
            if (string.IsNullOrWhiteSpace(selectedGenre))
            {
                MessageBox.Show("Выберите жанр", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                foreach (var g in choosenGenres)
                {
                    if (g == selectedGenre)
                    {
                        MessageBox.Show("Жанр повторяется", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
                choosenGenres.Add(selectedGenre);
            }
            UpdateGenres();
        }
        public void ClearGenre()
        {
            choosenGenres.Clear();
            UpdateGenres();

        }

        public void AddAlbum()
        {
            using var context = new SpotifyContext();
            GenresInAlbum gia = new GenresInAlbum();

            Album album;

            if (albumID != null)
            {
                album = context.Albums.FirstOrDefault(a => a.AlbumId == albumID);

                album.AlbumTitle = title;
                album.ReleaseYear = Convert.ToInt32(selectedDate.ToString("yyyy"));
                album.CoverPath = coverPath == null ? "/Resources/placeholder_cover.png" : coverPath;
                album.ArtistId = context.Artists.FirstOrDefault(a => a.ArtistName == author).ArtistId;
                List<GenresInAlbum> oldGenres = context.GenresInAlbums.Where(g => g.AlbumId == albumID).ToList();
                context.GenresInAlbums.RemoveRange(oldGenres);
                context.SaveChanges();
                MessageBox.Show("Альбом обновлён", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                album = new Album
                {
                    AlbumTitle = title,
                    ReleaseYear = Convert.ToInt32(selectedDate.ToString("yyyy")),
                    CoverPath = coverPath == null ? "/Resources/placeholder_cover.png" : coverPath,
                    ArtistId = context.Artists.FirstOrDefault(a => a.ArtistName == author).ArtistId,
                    TotalDuration = TimeOnly.FromTimeSpan(TimeSpan.Zero)
                };
                context.Albums.Add(album);
                MessageBox.Show("Альбом добавлен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            context.SaveChanges();
            if (albumID == null)
            {
                albumID = context.Albums.OrderBy(a => a.AlbumId).LastOrDefault().AlbumId;
            }
            
            for (int i = 0; i < choosenGenres.Count; i++)
            {
                gia = new GenresInAlbum
                {
                    GiAlId = context.GenresInAlbums.OrderBy(g => g.GiAlId).LastOrDefault().GiAlId + 1,
                    GenreId = context.Genres.Where(g => g.GenreTittle == choosenGenres[i]).Select(g => g.GenreId).FirstOrDefault(),
                    AlbumId = (int)albumID
                };
                context.GenresInAlbums.Add(gia);
                context.SaveChanges();
            }

            _AlbumViewModel.LoadAlbums();
            foreach (Window w in Application.Current.Windows)
            {
                if (w is Views.AlbumsAddition)
                {
                    w.Close();
                }
            }
        }
    }
}
