using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spotify_wpf.Context;
using System.Windows.Input;
using Spotify_wpf.Models;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;

namespace Spotify_wpf.ViewModels
{
    public class AddTrackViewModel : BaseViewModel
    {
        public int trackId;
        public ObservableCollection<string> album { get; set; }

        private ObservableCollection<Genre> _allAlbum;


        public ObservableCollection<string> artists { get; set; }
        private ObservableCollection<Artist> _allArtists;


        public Track selectedTrack { get; set; }

        /*private int _id { get; set; }
        public int id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }*/
        private string _nametrack { get; set; }
        public string nameTrack
        {
            get => _nametrack;
            set
            {
                _nametrack = value;
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


        private string _selectedAlbum { get; set; }
        public string selectedAlbum
        {
            get => _selectedAlbum;
            set
            {
                _selectedAlbum = value;
                SwitchImage(_selectedAlbum);
                OnPropertyChanged();
            }
        }

        private DateOnly _releaseyear;
        public DateOnly releaseYear
        {
            get => _releaseyear;
            set
            {
                _releaseyear = value;
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
                OnPropertyChanged();
            }
        }

        private int _bitrait;
        public int bitrait
        {
            get => _bitrait;
            set
            {
                _bitrait = value;
                OnPropertyChanged();
            }
        }

        private double _rating;
        public double rating
        {
            get => _rating;
            set
            {
                _rating = value;
                OnPropertyChanged();
            }
        }

        private string _duration;
        public string duration
        {
            get => _duration;
            set
            {
                _duration = value;
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
        public ICommand DeleteTrackCommand { get; }
        public ICommand SaveTrackCommand { get; }

        public ICommand AddImageCommand { get; }

        public ICommand AddArtist { get; }

        private ObservableCollection<string> chooseArtist { get; set; } = new ObservableCollection<string>();
        private string _chooseArtist { get; set; }
        public string chooseArtists
        {
            get => _chooseArtist;
            set
            {
                _chooseArtist = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<string> chooseAlbum { get; set; } = new ObservableCollection<string>();
        private string _chooseAlbum { get; set; }
        public string chooseAlbums
        {
            get => _chooseAlbum;
            set
            {
                _chooseAlbum = value;
                OnPropertyChanged();
            }
        }

        public AddTrackViewModel()
        {
            //trId = id;
            var context = new MusicContext();
            var artistList = context.Artists
                .Select(s => s.ArtistName)
                .Distinct()
                .OrderBy(s => s)
                .ToList();
            artists = new ObservableCollection<string>(artistList);

            var albumList = context.Albums
                .Select(g => g.AlbumName)
                .Distinct()
                .OrderBy(s => s)
                .ToList();
            album = new ObservableCollection<string>(albumList);

            //LoadData(id);
            AddArtist = new RelayCommand(AddArtists);
            SaveTrackCommand = new RelayCommand(SaveTrack);
            AddImageCommand = new RelayCommand(AddImage);

        }


        public void LoadData(int id)
        {
            var context = new MusicContext();
            artists = new ObservableCollection<string>(context.Artists.Select(g => g.ArtistName));
            album = new ObservableCollection<string>(context.Albums.Select(g => g.AlbumName));

        }

        public void AddArtists()
        {
            chooseArtist.Add(_chooseArtist);
            _chooseArtist += selectedArtist + ", ";

            OnPropertyChanged(nameof(chooseArtists));

        }

       

        private void SaveTrack()
        {
            MusicContext context = new MusicContext();
            Track track = new Track
            {
                TrackId = context.Tracks.OrderBy(s => s.TrackId).LastOrDefault().TrackId + 1,
                TrackName = nameTrack,
                Bitrate = bitrait,
                Rating = (decimal)rating,
                Duration = duration.Contains(":") ? TimeOnly.Parse(duration) : TimeOnly.FromTimeSpan(TimeSpan.FromSeconds(Convert.ToDouble(duration))),
                ReleaseDate = releaseYear,
                FilePath = imagePath,
                //ArtistId = context.Artists.Where(a => a.ArtistName == selectedArtist).FirstOrDefault().ArtistId,
                AlbumId = context.Albums.Where(a => a.AlbumName == selectedAlbum).First().AlbumId,

            };
            context.Tracks.Add(track);
            context.SaveChanges();
           
            int trackIds = track.TrackId;
            //var albumIds = context.Albums.Where(a => chooseAlbum.Contains(a.AlbumName)).Select(a => a.AlbumId).ToList();
            var artistsIds = context.Artists.Where(g => chooseArtists.Contains(g.ArtistName)).Select(g => g.ArtistId).ToList();

            foreach (var artistId in artistsIds)
            {
                context.TrackArtists.Add(new TrackArtist
                {
                    TrackArtistId = context.TrackArtists.OrderBy(o => o.TrackArtistId).LastOrDefault().TrackArtistId + 1,
                    TrackId = trackIds,
                    ArtistId = artistId,

                });
                context.SaveChanges();


            }




            context.AlbumTracks.Add(new AlbumTrack
            {
                AlbumTrackId = context.AlbumTracks.OrderBy(o => o.AlbumTrackId).LastOrDefault().AlbumTrackId + 1,
                TrackId = trackIds,
                AlbumId = context.Albums.Where(a => a.AlbumName == selectedAlbum).First().AlbumId,

            });



            context.SaveChanges();

            MessageBox.Show("Успех", "Вы успешно добавили трек");
            foreach (Window w in Application.Current.Windows)
            {
                if (w is Views.AddTrack)
                {
                    w.Close();
                }
            }


        }

        public void SwitchImage(string albName)
        {
            var context = new MusicContext();
            imagePath = context.Albums.Where(a => a.AlbumName == albName).FirstOrDefault().CoverPath;
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
