using PraktikaMusicF.Context;
using PraktikaMusicF.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PraktikaMusicF
{
    /// <summary>
    /// Логика взаимодействия для AlbumsUC.xaml
    /// </summary>
    public partial class AlbumsUC : UserControl
    {
        public AlbumsUC()
        {
            InitializeComponent();
        }

        public class AlbumView
        {
            public string AlbumName { get; set; }
            public string ArtistName { get; set; }
            public string TrackCount { get; set; }
            public string ImagePath { get; set; }
        }

        public void LoadAlbum(Album alb)
        {
            if (alb == null)
                return;

            using var db = new MusicBdFContext();

            string artistName =
                db.Artists
                  .Where(a => a.ArtId == alb.ArtistId)
                  .Select(a => a.ArtName)
                  .FirstOrDefault()
                ?? "Не указан";

            
            int trackCount =
                db.TrackLists.Count(tl => tl.AlbId == alb.AlbId);

            
            string? img = alb.AlbImage;
            string defaultImage = "/Images/defaultAlbum.png";

            string finalImg;

            if (!string.IsNullOrEmpty(img))
            {
                if (Uri.IsWellFormedUriString(img, UriKind.Absolute))
                {

                    finalImg = img;
                }
                else if (File.Exists(img))
                {
                   
                    finalImg = img;
                }
                else
                {
                    finalImg = "/Images/defaultAlbum.png";
                }
            }
            else
            {
                finalImg = "/Images/defaultAlbum.png";
            }


            DataContext = new AlbumView
            {
                AlbumName = alb.AlbName ?? "Без названия",
                ArtistName = artistName,
                TrackCount = $"{trackCount}",
                ImagePath = finalImg
            };
        }
    }
}
