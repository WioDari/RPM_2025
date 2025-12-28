using Microsoft.Win32;
using MusicWpf.Context;
using MusicWpf.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

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

         private ObservableCollection<Genre> _genres;
         public ObservableCollection<Genre> Genres
         {
             get => _genres;
             set
             {
                 _genres = value;
                 OnPropertyChanged();
             }
         }

        public ObservableCollection<Genre> SelectedGenres { get; }

        private Genre _selectedGenre;
        public Genre SelectedGenre
        {
            get => _selectedGenre;
            set
            {
                _selectedGenre = value;
                OnPropertyChanged();

                AddGenreToSelection(value);

                _selectedGenre = null;
                OnPropertyChanged(nameof(SelectedGenre));
            }
        }




        private string _selectedGenresText;
        public string SelectedGenresText
        {
            get => _selectedGenresText;
            set
            {
                _selectedGenresText = value;
                OnPropertyChanged();
            }
        }


        public ICommand AlbumCommand { get; private set; }
        public ICommand AddImageCommand { get; private set; }
        public ICommand ClearGenresCommand { get; private set; }

        public AddAlbumViewModel()
        {
            /*using var dbMusic = new MusicContext();
            Artists = new ObservableCollection<Artist>(dbMusic.Artists.ToList());*/

            LoadData();

            AlbumCommand = new RelayCommand(AlbumCreat);
            AddImageCommand = new RelayCommand(AddImage);
            ClearGenresCommand = new RelayCommand(ClearGenres);

            SelectedGenres = new ObservableCollection<Genre>();
            UpdateSelectedGenresText();
        }

        private void LoadData()
        {
            using var dbMusic = new MusicContext();
            Artists = new ObservableCollection<Artist>(dbMusic.Artists.ToList());
            Genres = new ObservableCollection<Genre>(dbMusic.Genres.ToList());
        }

        private void UpdateSelectedGenresText()
        {
            if (SelectedGenres != null && SelectedGenres.Any())
            {
                SelectedGenresText = string.Join(", ", SelectedGenres.Select(g => g.GenreName));
            }
            else
            {
                SelectedGenresText = "Жанры не выбраны";
            }
        }
        public void AlbumCreat()
        {
            if (string.IsNullOrWhiteSpace(TitleAlbum))
            {
                MessageBox.Show("Введите название альбома");
                return;
            }

            if (SelectedArtist == null)
            {
                MessageBox.Show("Выберите артиста");
                return;
            }

            if (ReleaseYear < 1900 || ReleaseYear > DateTime.Now.Year + 1)
            {
                MessageBox.Show("Введите корректный год выпуска");
                return;
            }

            try
            {
                using var db = new MusicContext();
                var album = new Album
                {
                    AlbumTitle = TitleAlbum,
                    ReleaseYear = ReleaseYear,
                    ArtistId = SelectedArtist.ArtistId,
                    CoverPath = CoverPath ?? "",
                    TotalDuration = 0
                };

                db.Albums.Add(album);
                db.SaveChanges();

                foreach (var genre in SelectedGenres)
                {
                    var albumGenre = new AlbumGenre
                    {
                        AlbumId = album.AlbumId,
                        GenreId = genre.GenreId
                    };
                    db.AlbumGenres.Add(albumGenre);
                }
               

                db.SaveChanges();

                MessageBox.Show("Альбом успешно создан!");

                TitleAlbum = "";
                //ReleaseDate = "";
                SelectedArtist = null;
                SelectedGenres.Clear();
                UpdateSelectedGenresText();
                CoverPath = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании альбома: {ex.Message}");
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
                CoverPath = openFileDialog.FileName;
                MessageBox.Show("Изображение выбрано: " + System.IO.Path.GetFileName(CoverPath));
            }
        }

        public void ClearGenres()
        {
            SelectedGenres.Clear();
            UpdateSelectedGenresText();
        }



        private void AddGenreToSelection(Genre genre)
        {
            if (genre == null)
                return;

            if (!SelectedGenres.Contains(genre))
            {
                SelectedGenres.Add(genre);
                UpdateSelectedGenresText();
            }
        }

    }
}
