using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MusicPlusPlus.Context;
using MusicPlusPlus.Models;
using System;
using System.Data.Entity;
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
        

        using (var context = new SpotifyContext())
        {
            album = context.Albums.Include(x => x.Artists).Include(x => x.Tracks).FirstOrDefault(x => x.Albumid == album.Albumid);

            string artistname = string.Join(", ", album.Artists.Select(x => x.Artistname)) ?? "Неизвестен";

            string zxc = "";
            foreach (var artist in album.Artists.Select(x => x.Artistname).ToList())
            {
                zxc = zxc + artist + " | " ?? "Неизвестен";
            }
            
            

            int trackcount = album.Tracks.Count;

            /*
            if (!string.IsNullOrEmpty(album.Coverpath))
            {
                if(Uri.IsWellFormedUriString(album.Coverpath, UriKind.Absolute))
                {
                    imagepath = album.Coverpath;
                }
            }
            */

            //string imagepath = $"Resources/covers/{album.Albumname}.jpg" ?? "Resources/placeholder_cover.png";
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
                ArtistName = zxc,
                TrackCount = $"{trackcount}",
                ImagePath = imagepath
            };
        }
    }
}