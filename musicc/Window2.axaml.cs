using System;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using musicc.Context;
using musicc.Models;

namespace musicc;

public partial class Window2 : Window
{
    public ObservableCollection<string> Arts { get; set; } = new();
    public ObservableCollection<string> Janri { get; set; } = new();
    public Window2()
    {
        InitializeComponent();
        DataContext = this;
        Artist();
        janr();
    }

    private void Artist()
    {
        using (var bd = new PostgresContext())
        {
            var artist = bd.Artists.Select(a => a.ArtistName).ToList();
            
            Arts.Clear();
            foreach (var a in artist)
                {
                Arts.Add(a);
                }
            
        }
    }

    private void janr()
    {
        using (var bd = new PostgresContext())
        {
            var jar = bd.Genres.Select(g => g.GenreName).ToList();
            
            Janri.Clear();
            foreach (var a in jar)
                {
                Janri.Add(a);
                }
            
        }
    }
    

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        using (var bd = new PostgresContext())
        {
            if (box_name == null)
            {
                return;
            }
            if (Dates == null)
            {
                return;
            }
            if (box_hero == null)
            {
                return;
            }

            if (box_url == null)
            {
                box_url.Text = @"C:\Users\sekibanki\RiderProjects\musicc\musicc\photo\placeholder_cover.png";
            }
            var artisty = bd.Artists.FirstOrDefault(a => a.ArtistName == box_hero.SelectionBoxItem as string);
            var album = new Album
            {
                AlbumTitle = box_name.Text,
                ReleaseYear = DateOnly.FromDateTime(Dates.SelectedDate.Value.DateTime),
                ArtistId = artisty.ArtistId,
                CoverPath = box_url.Text
            };
            bd.Albums.Add(album);
            bd.SaveChanges();

            this.Close(true);
        }
    }
}