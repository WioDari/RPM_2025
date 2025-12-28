using MusicWpf.Context;
using MusicWpf.Models;
using MusicWpf.Views;
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
    public class AddTrackViewModel : BaseViewModel
    {

        private string _titleTrack;
        public string TitleTrack
        {
            get => _titleTrack;

            set
            {
                _titleTrack = value;
                OnPropertyChanged();
            }

        }

        private int _prodolTrack;
        public int ProdolTrack
        {
            get => _prodolTrack;
            set
            {
                _prodolTrack = value;
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



        private string _bitret;
        public string Bitret
        {
            get => _bitret;
            set
            {
                _bitret = value;
                OnPropertyChanged();
            }
        }
        private int _reiting;
        public int Reiting
        {
            get => _reiting;
            set
            {
                _reiting = value;
                OnPropertyChanged();
            }
        }


        private DateTime _releaseDate = DateTime.Now;
        public DateTime ReleaseDate
        {
            get => _releaseDate;
            set 
            { 
                _releaseDate = value; 
                OnPropertyChanged(); 
            }
        }

        private ObservableCollection<Models.Album> _albums;
        public ObservableCollection<Models.Album> Albums
        {
            get => _albums;
            set
            {
                _albums = value;
                OnPropertyChanged();
            }
        }


        private Models.Album _selectedAlbum;
        public Models.Album SelectedAlbum
        {
            get => _selectedAlbum;
            set
            {
                _selectedAlbum = value;
                OnPropertyChanged();
                if (_selectedAlbum != null && !string.IsNullOrEmpty(_selectedAlbum.CoverPath))
                {
                    CoverPath = _selectedAlbum.CoverPath;
                    // ReleaseDate = new DateTime(_selectedAlbum.ReleaseYear, 1, 1);
                }
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


        public ObservableCollection<Artist> SelectedArtists { get; }

        private Artist _selectedArtist;
        public Artist SelectedArtist
        {
            get => _selectedArtist;
            set
            {
                _selectedArtist = value;
                OnPropertyChanged();

                if (value != null && !SelectedArtists.Contains(value))
                {
                    SelectedArtists.Add(value);
                    UpdateSelectedArtistText();
                    _selectedArtist = null; // Сбрасываем выбранный элемент
                    OnPropertyChanged(nameof(SelectedArtist));
                }
            }
        }




        private string _selectedArtistText;
        public string SelectedArtistText
        {
            get => _selectedArtistText;
            set
            {
                _selectedArtistText = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddTrackCommand { get; set; }
        public ICommand ClearArtistsCommand { get; set; }


        public AddTrackViewModel()
        {
         
            SelectedArtists = new ObservableCollection<Artist>();
            AddTrackCommand = new RelayCommand(AddTrack);
            UpdateSelectedArtistText();
            LoadData();
        }

        public void LoadData()
        {
            try
            {
                using var db = new MusicContext();
                var artistsList = db.Artists.ToList();
                Artists = new ObservableCollection<Artist>(db.Artists.ToList());
                var albumsList = db.Albums.ToList();
                Albums = new ObservableCollection<Models.Album>(db.Albums.ToList());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}");
            }
        }

        private void UpdateSelectedArtistText()
        {
            if (SelectedArtists != null && SelectedArtists.Any())
            {
                SelectedArtistText = string.Join(", ", SelectedArtists.Select(a => a.ArtistName));
            }
            else
            {
                SelectedArtistText = "Артисты не выбраны";
            }

        }



        private void AddTrack()
        {

            if (string.IsNullOrWhiteSpace(TitleTrack))
            {
                MessageBox.Show("Введите название трека");
                return;
            }

            if (ProdolTrack <= 0)
            {
                MessageBox.Show("Введите продолжительность трека в секундах");
                return;
            }

            if (string.IsNullOrWhiteSpace(Bitret) || !int.TryParse(Bitret, out int bitrate) || bitrate <= 0)
            {
                MessageBox.Show("Введите корректный битрейт");
                return;
            }

            if (Reiting < 0 || Reiting > 10)
            {
                MessageBox.Show("Рейтинг должен быть от 0 до 10");
                return;
            }

            if (!SelectedArtists.Any())
            {
                MessageBox.Show("Выберите хотя бы одного артиста");
                return;
            }

            try
            {
                using var db = new MusicContext();

                var track = new Track
                {
                    TrackName = TitleTrack,
                    Duration = TimeOnly.FromTimeSpan(TimeSpan.FromSeconds(ProdolTrack)),
                    ReleaseDate = DateOnly.FromDateTime(ReleaseDate),
                    Bitrate = int.Parse(Bitret),
                    Rating = Reiting,
                    AlbumCoverPath = CoverPath,
                    AlbumCoverBinary = "", // Заполните если нужно
                    FilePath = "",
                };

                db.Tracks.Add(track);
                db.SaveChanges();

                if (SelectedAlbum != null)
                {
                    db.AlbumTracks.Add(new AlbumTrack
                    {
                        TracksId = track.TracksId,
                        AlbumId = SelectedAlbum.AlbumId
                    });
                }

                foreach (var artist in SelectedArtists)
                {
                    db.ArtistTracks.Add(new ArtistTrack
                    {
                        TrackId = track.TracksId,
                        ArtistId = artist.ArtistId
                    });
                }

                db.SaveChanges();

                MessageBox.Show($"Трек '{TitleTrack}' успешно добавлен!");

                TitleTrack = "";
                ProdolTrack = 0;
                Bitret = "";
                Reiting = 0;
                CoverPath = "";
                SelectedAlbum = null;
                SelectedArtists.Clear();
                ReleaseDate = DateTime.Now;
                UpdateSelectedArtistText();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Ошибка при добавлении трека: {ex.Message}");
            }
        }

    }
}
