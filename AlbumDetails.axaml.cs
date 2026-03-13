using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Microsoft.EntityFrameworkCore;
using Spotify.Context;
using Spotify.Models;

namespace Spotify;

public partial class AlbumDetails : Window
{
    private HttpClient httpClient = new HttpClient();

    public AlbumDetails(Album album)
    {
        InitializeComponent();
        LoadAlbum(album);
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private async void LoadAlbum(Album album)
    {
        try
        {
            using (var db = new PostgresContext())
            {
                var fullAlbum = db.Albums
                    .Include(a => a.Artist)
                    .ThenInclude(ar => ar.Genres)
                    .Include(a => a.Tracks)
                    .ThenInclude(t => t.Artist)
                    .FirstOrDefault(a => a.AlbumId == album.AlbumId);

                if (fullAlbum == null)
                    return;

                var albumTitleText = this.FindControl<TextBlock>("AlbumTitleText");
                var artistText = this.FindControl<TextBlock>("ArtistText");
                var yearText = this.FindControl<TextBlock>("YearText");
                var genresText = this.FindControl<TextBlock>("GenresText");
                var coverImage = this.FindControl<Image>("CoverImage");
                var noCoverText = this.FindControl<TextBlock>("NoCoverText");
                var tracksListPanel = this.FindControl<StackPanel>("TracksListPanel");

                if (albumTitleText != null)
                    albumTitleText.Text = fullAlbum.AlbumTitle;

                if (artistText != null)
                    artistText.Text = "Исполнитель: " + (fullAlbum.Artist?.ArtistName ?? "Неизвестно");

                if (yearText != null)
                    yearText.Text = "Год выпуска: " + fullAlbum.ReleaseYear.Year;

                
                string genresLine = "Жанры: Не указаны";

                if (fullAlbum.Artist != null && fullAlbum.Artist.Genres != null && fullAlbum.Artist.Genres.Any())
                {
                    var genreNames = fullAlbum.Artist.Genres
                        .Select(g => g.GenreName)
                        .ToList();

                    genresLine = "Жанры: " + string.Join(", ", genreNames);
                }
                
                if (genresText != null)
                    genresText.Text = genresLine;

                if (!string.IsNullOrWhiteSpace(fullAlbum.CoverPath))
                {
                    try
                    {
                        var data = await httpClient.GetByteArrayAsync(fullAlbum.CoverPath);
                        using (var ms = new MemoryStream(data))
                        {
                            var bitmap = new Bitmap(ms);
                            if (coverImage != null)
                                coverImage.Source = bitmap;

                            if (noCoverText != null)
                                noCoverText.IsVisible = false;
                        }
                    }
                    catch
                    {
                    }
                }

                if (tracksListPanel != null)
                {
                    tracksListPanel.Children.Clear();

                    var tracks = db.Tracks
                        .Include(t => t.Artist)
                        .Where(t => t.AlbumId == fullAlbum.AlbumId)
                        .ToList();

                    tracksListPanel.Children.Clear();

                    foreach (var track in tracks)
                    {
                        var rowBorder = new Border
                        {
                            BorderBrush = Avalonia.Media.Brushes.Black,
                            BorderThickness = new Avalonia.Thickness(0, 0, 0, 2),
                            Padding = new Avalonia.Thickness(10)
                        };

                        var rowGrid = new Grid
                        {
                            ColumnDefinitions = new ColumnDefinitions("2*,2*,2*,2*")
                        };

                        var trackNameText = new TextBlock
                        {
                            Text = track.TrackName,
                            FontSize = 18
                        };

                        var artistNameText = new TextBlock
                        {
                            Text = track.Artist?.ArtistName ?? "Неизвестно",
                            FontSize = 18,
                            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center
                        };

                        var ratingText = new TextBlock
                        {
                            Text = track.Rating.ToString(),
                            FontSize = 18,
                            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center
                        };

                        var durationText = new TextBlock
                        {
                            Text = FormatSeconds(track.Duration),
                            FontSize = 18,
                            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Right
                        };

                        Grid.SetColumn(trackNameText, 0);
                        Grid.SetColumn(artistNameText, 1);
                        Grid.SetColumn(ratingText, 2);
                        Grid.SetColumn(durationText, 3);

                        rowGrid.Children.Add(trackNameText);
                        rowGrid.Children.Add(artistNameText);
                        rowGrid.Children.Add(ratingText);
                        rowGrid.Children.Add(durationText);

                        rowBorder.Child = rowGrid;
                        tracksListPanel.Children.Add(rowBorder);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private string FormatSeconds(int totalSeconds)
    {
        if (totalSeconds <= 0) return "0:00";

        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;
        return $"{minutes}:{seconds:D2}";
    }
}