using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using musicc.Context;
using musicc.Models;

namespace musicc;

public partial class Window1 : Window
{
    public string Boxname { get; set; } = "none";
    public string Premium { get; set; } = "none";
    public string Role { get; set; } = "none";
    public ObservableCollection<Playlist> Playlists { get; set; } = new ObservableCollection<Playlist>();
    public ObservableCollection<Album> Albums { get; set; } = new ObservableCollection<Album>();
    public Window1()
    {
       
        InitializeComponent();

        
    }

    public Window1(string boxname, string premium, int Inv_role)
    {
        Boxname = boxname;
        Premium = premium?.ToString() ?? "none";
        if (Inv_role == 0)
        {
            Role = "Manager";
        }
        else if (Inv_role == 1)
        {
            Role = "User";
        }
        else if (Inv_role == 4)
        {
            Role = "Administrator";
        }
        InitializeComponent();
        DataContext = this;
        _ = LoadPlay();
        _ = LoadAlbum();
    }

    public async Task LoadAlbum()
    {
        using (var bd = new PostgresContext())
        {
            var albus = await bd.Albums.AsNoTracking().OrderBy(a => a.AlbumId).Select(a => new 
            {
                Name = a.AlbumTitle,
                author = a.Artist.ArtistName,
                photo = a.CoverPath?? @"C:\Users\sekibanki\RiderProjects\musicc\musicc\photo\placeholder_cover.png",
                track = a.Tracks.Count
            }).ToListAsync();
            Albums.Clear();
            foreach (var alb in albus)
                {
                Albums.Add(new Album
                {
                    Name = alb.Name,
                    author = alb.author,
                    photo = await Bis(alb.photo),
                    track =  alb.track
                });
                }
            
        }
      
    }

    private async Task LoadPlay()
    {
        using (var bd = new PostgresContext())
        {
            var plays = await bd.Playlists.AsNoTracking().OrderBy(p => p.PlaylistId).Select(p => new Playlist
            {
                Name = p.PlaylistName,
                author = p.Users.Select(u => u.FullName).FirstOrDefault(),
                like = p.Likes,
                track = p.Tracks.Count,
                data = p.DateCreated,
                subs = p.Users.Count
            }).ToListAsync();
            Playlists.Clear();
            
            foreach (var pl in plays)
                Playlists.Add(pl);
        }
    }
    public class Playlist
    {
        public string Name { get; set; }
        public string author { get; set; }
        public  int like { get; set; }
        public int track { get; set; }
        public System.DateOnly data { get; set; }
        public int subs {get; set;}
    }
    public class Album
    {
        public string Name { get; set; }
        public string author { get; set; }
        public Bitmap photo { get; set; }
        public int track { get; set; }
    }

    private async Task<Bitmap> Bis(string paths)
    {
        using var httpc = new HttpClient();
        try
        {
            var date = await httpc.GetAsync(paths);
            date.EnsureSuccessStatusCode();
            var ste = await date.Content.ReadAsByteArrayAsync();
            return new Bitmap(new MemoryStream(ste));
        }
        catch
        {
            return null;
        }
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        var win = new MainWindow();
        win.Show();
        this.Close();
    }

    private async void Button_OnClick1(object? sender, RoutedEventArgs e)
    {
        var wind = new Window2();
        var result = await wind.ShowDialog<bool?>(this);
        if (result == true)
        {
            await LoadAlbum();
        }
    }

    private void Button_OnClick2(object? sender, RoutedEventArgs e)
    {
        var wind = new Window3();
        wind.ShowDialog(this);
    }
        
}