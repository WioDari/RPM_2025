using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using PraktikaMusicF.Context;
using PraktikaMusicF.Models;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.EntityFrameworkCore;

namespace PraktikaMusicF
{
    /// <summary>
    /// Логика взаимодействия для ArtistUC.xaml
    /// </summary>
    public partial class ArtistUC : UserControl
    {
        public ArtistUC()
        {
            InitializeComponent();
        }

        public class ArtistView
        {
            public string ArtName { get; set; }
            public string YearActive { get; set; }
            public string ArtDesc { get; set; }
            public string ArtPhotoPath { get; set; }
            public string Genres { get; set; } 
        }

        public void LoadArtist(Artist art)
        {
            using var db = new MusicBdFContext();

            string artName = art.ArtName ?? "Не указано";
            string yearActive = art.YearActive ?? "Не указано";
            string artDesc = art.ArtDesc ?? "Не указано";
            var artGenres = db.ArtistGenres
                  .Where(x => x.ArtId == art.ArtId)
                  .Include(x => x.Genre)
                  .Select(x => x.Genre.GenresName)
                  .ToList();
            string genresString = artGenres.Any()
                                  ? string.Join(", ", artGenres)
                                  : "Жанры не указаны";

            string? finalImg = null;

            if (!string.IsNullOrEmpty(art.ArtPhotoPath))
            {
                if (Uri.IsWellFormedUriString(art.ArtPhotoPath, UriKind.Absolute))
                    finalImg = art.ArtPhotoPath;
            }

            DataContext = new ArtistView
            {
                ArtName = artName,
                YearActive = yearActive,
                ArtDesc = artDesc,
                ArtPhotoPath = finalImg,
                Genres = genresString
            };
        }

    }
}
