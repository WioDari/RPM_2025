using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MusicPlusPlus.Context;
using MusicPlusPlus.Models;
using System;
using Microsoft.EntityFrameworkCore;
//using System.Data.Entity;
using System.Linq;
using Avalonia.Media.Imaging;


namespace MusicPlusPlus;

public partial class AlbumUC : UserControl
{
    public AlbumUC()
    {
        InitializeComponent();
    }

    public class AlbumView
    {
        public string ImagePath { get; set; }
        public string AlbumName { get; set; }
        public string ArtistName { get; set; }
        public string TrackCount { get; set; }
    }

    public void LoadAlbum (Album album, string imagepath)
    {



        string artistname = "";
        using(var context = new MusicdbContext())
    {
            var fullAlbum = context.Albums
                .Include(a => a.Artists)
                .AsSplitQuery()
                .FirstOrDefault(a => a.Albumid == album.Albumid);

            artistname = fullAlbum != null && fullAlbum.Artists.Any()
                ? string.Join(", ", fullAlbum.Artists.Select(a => a.Artistname))
                : "Неизвестный исполнитель";
        }


        //подсчёт треков в альбоме
        int trackcount = 0;
            using (var context = new MusicdbContext())
            {
                var tracks = context.Tracks.Where(x => x.Albumid == album.Albumid);
                foreach (var track in tracks)
                {

                    trackcount ++;
                }
            }
            
            

            try
            {
                AlbumCover.Source = new Bitmap(imagepath);
            }
            catch (Exception ex)
            {
                AlbumCover.Source = new Bitmap("Resources/placeholder_cover.png");
            }

            

            DataContext = new AlbumView
            {
                AlbumName = album.Albumname ?? "Без названия",
                ArtistName = artistname,
                TrackCount = trackcount.ToString(),
                ImagePath = imagepath
            };
        
    }
}