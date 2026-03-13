using System;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Microsoft.EntityFrameworkCore;
using Spotify.Context;
using Spotify.Models;

namespace Spotify;

public partial class PlaylistDetails : Window
{
    public PlaylistDetails(Playlist playlist)
    {
        InitializeComponent();
        LoadPlaylist(playlist);
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
    
    private void LoadPlaylist(Playlist playlist)
    {
        try
        {
            using (var db = new PostgresContext())
            {
                var fullPlaylist = db.Playlists
                    .Include(p => p.User)
                    .Include(p => p.Tracks)
                        .ThenInclude(t => t.Artist)
                    .Include(p => p.Tracks)
                        .ThenInclude(t => t.Album)
                    .FirstOrDefault(p => p.PlaylistId == playlist.PlaylistId);

                if (fullPlaylist == null)
                    return;

                var playlistTitleText = this.FindControl<TextBlock>("PlaylistTitleText");
                var createdDateText = this.FindControl<TextBlock>("CreatedDateText");
                var likesText = this.FindControl<TextBlock>("LikesText");
                var durationText = this.FindControl<TextBlock>("DurationText");
                var playlistTracksPanel = this.FindControl<StackPanel>("PlaylistTracksPanel");

                if (playlistTitleText != null)
                    playlistTitleText.Text = fullPlaylist.PlaylistName + " | " + (fullPlaylist.User?.FullName ?? "Неизвестно");

                if (createdDateText != null)
                    createdDateText.Text = "Дата создания: " + fullPlaylist.DateCreated.ToString("dd.MM.yyyy");

                if (likesText != null)
                    likesText.Text = "Лайков: " + fullPlaylist.Likes;

                int totalSeconds = 0;
                if (fullPlaylist.Tracks != null && fullPlaylist.Tracks.Any())
                    totalSeconds = fullPlaylist.Tracks.Sum(t => t.Duration);

                if (durationText != null)
                    durationText.Text = "Продолжительность: " + FormatSeconds(totalSeconds);

                if (playlistTracksPanel != null)
                {
                    playlistTracksPanel.Children.Clear();

                    foreach (var track in fullPlaylist.Tracks)
                    {
                        var rowBorder = new Border
                        {
                            BorderBrush = Avalonia.Media.Brushes.Black,
                            BorderThickness = new Avalonia.Thickness(0, 0, 0, 2),
                            Padding = new Avalonia.Thickness(10)
                        };

                        var rowGrid = new Grid
                        {
                            ColumnDefinitions = new ColumnDefinitions("2*,2*,2*,2*,2*")
                        };

                        var trackNameText = new TextBlock
                        {
                            Text = track.TrackName,
                            FontSize = 18
                        };

                        var artistText = new TextBlock
                        {
                            Text = track.Artist?.ArtistName ?? "Неизвестно",
                            FontSize = 18,
                            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center
                        };

                        var albumText = new TextBlock
                        {
                            Text = track.Album?.AlbumTitle ?? "Без альбома",
                            FontSize = 18,
                            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center
                        };

                        var ratingText = new TextBlock
                        {
                            Text = track.Rating.ToString(),
                            FontSize = 18,
                            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center
                        };

                        var durationTrackText = new TextBlock
                        {
                            Text = FormatSeconds(track.Duration),
                            FontSize = 18,
                            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Right
                        };

                        Grid.SetColumn(trackNameText, 0);
                        Grid.SetColumn(artistText, 1);
                        Grid.SetColumn(albumText, 2);
                        Grid.SetColumn(ratingText, 3);
                        Grid.SetColumn(durationTrackText, 4);

                        rowGrid.Children.Add(trackNameText);
                        rowGrid.Children.Add(artistText);
                        rowGrid.Children.Add(albumText);
                        rowGrid.Children.Add(ratingText);
                        rowGrid.Children.Add(durationTrackText);

                        rowBorder.Child = rowGrid;
                        playlistTracksPanel.Children.Add(rowBorder);
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