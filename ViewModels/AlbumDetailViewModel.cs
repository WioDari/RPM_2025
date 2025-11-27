using Microsoft.EntityFrameworkCore;
using NpgsqlTypes;
using SpotApp_wpf.Context;
using SpotApp_wpf.Models;
using SpotApp_wpf.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SpotApp_wpf.ViewModels
{
    public class AlbumDetailViewModel : BaseViewModel
    {
        private string _albumCover { get; set; }
        public string albumCover
        {
            get => _albumCover;
            set
            {
                _albumCover = value;
                OnPropertyChanged();
            }
        }
        private string _albumName {  get; set; }
        public string albumName
        {
            get => _albumName;
            set
            {
                _albumName = value;
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
        private string _genres { get; set; }
        public string genres
        {
            get => _genres;
            set
            {
                _genres = value;
                OnPropertyChanged();
            }
        }

        public class TrackDetails
        {
            public string id { get; set; }
            public string trackName { get; set; }
            public List<string> authors { get; set; }
            public string authorsl {  get; set; }
            public string param { get; set; }
            public string rating { get; set; }
            public string duration { get; set; }

        }

        public ObservableCollection<TrackDetails> albDetails { get; set; }
        public List<string> genresC { get; set; }

        public AlbumDetailViewModel(int? id = null)
        {
            LoadDetails(id);
        }

        public void LoadDetails(int? id = null)
        {
            var context = new SpotifyContext();
            string albCover = context.Albums.Where(a => a.AlbumId == id).FirstOrDefault().CoverPath;
            albumCover = albCover == null ? "/Resources/placeholder_cover.png" : albCover;
            author = context.Artists.Include(a => a.Albums.Where(a => a.AlbumId == id)).FirstOrDefault().ArtistName;
            albumName = context.Albums.Where(a => a.AlbumId == id).FirstOrDefault().AlbumTitle;
            genresC = context.GenresInAlbums.Include(g => g.Genre).Where(g => g.AlbumId == id).Select(g => g.Genre.GenreTittle).ToList();
            if (genresC.Count > 0)
            {
                genres = genresC[0];
                for (int i = 1; i < genresC.Count; i++)
                {
                    genres += ", " + genresC[i];
                }
            }
            else
            {
                genres = "";
            }
            albDetails = new ObservableCollection<TrackDetails>(context.Tracks.Where(t => t.AlbumId == id)
                .Include(t => t.ArtistsInTracks)
                .Select(t => new TrackDetails
                {
                    id = t.TrackId.ToString(),
                    trackName = t.TrackName,
                    authors = t.ArtistsInTracks.Where(at => at.TrackId == t.TrackId).Select(t => t.Artist.ArtistName).ToList(),
                    authorsl = "",
                    param = t.PlayCount.ToString(),
                    rating = t.Rating.ToString(),
                    duration = t.Duration.ToString("mm:ss"),
                })
                .OrderBy(t => t.id)
                .ToList());

            foreach (var track in albDetails)
            {
                /*foreach (string a in track.authors)
                {
                    string b = a;
                    track.authorsl += b + ";  ";
                }*/
                track.authorsl = track.authors[0];
                for (int i = 1; i < track.authors.Count; i++)
                {
                    track.authorsl += ", " + track.authors[i];
                }
            }
        }
    }
}
