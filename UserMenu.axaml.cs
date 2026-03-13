using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Microsoft.EntityFrameworkCore;
using Spotify.Context;
using Spotify.Models;

namespace Spotify;

public partial class UserMenu : Window
{
    private HttpClient httpClient = new HttpClient();
    private User currentUser;

    private string albumUrl;
    private string trackUrl;

    public UserMenu()
    {
        InitializeComponent();
        LoadData();
    }

    private void LoadData()
    {
        using var db = new PostgresContext();
    
        var albums = db.Albums.OrderBy(a => a.AlbumTitle).ToList();
        var artists = db.Artists.OrderBy(a => a.ArtistName).ToList();
        var genres = db.Genres.OrderBy(g => g.GenreName).ToList();
    
        var albumCombo = this.FindControl<ComboBox>("TrackAlbumCombo");
        if (albumCombo != null)
            albumCombo.ItemsSource = albums;
        var artistCombo = this.FindControl<ComboBox>("AlbumArtistCombo");
        if (artistCombo != null)
            artistCombo.ItemsSource = artists;
        var artistsList = this.FindControl<ListBox>("TrackArtistsList");
        if (artistsList != null)
            artistsList.ItemsSource = artists;
        var genresList = this.FindControl<ListBox>("AlbumGenresList");
        if (genresList != null)
            genresList.ItemsSource = genres;
    }
    
    public UserMenu(User user) : this()
    {
        DataContext = user;
        currentUser = user;
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    // НАВИГАЦИЯ
    private void HideAllPages()
    {
        var home = this.FindControl<StackPanel>("HomeContent");
        var albums = this.FindControl<StackPanel>("AlbumsContent");
        var playlists = this.FindControl<StackPanel>("PlaylistsContent");
        var admAlbum = this.FindControl<StackPanel>("AdmAlbumContent");
        var admTrack = this.FindControl<StackPanel>("AdmTrackContent");

        if (home != null) home.IsVisible = false;
        if (albums != null) albums.IsVisible = false;
        if (playlists != null) playlists.IsVisible = false;
        if (admAlbum != null) admAlbum.IsVisible = false;
        if (admTrack != null) admTrack.IsVisible = false;
    }

    private void HomeBtn_Click(object? sender, RoutedEventArgs e)
    {
        HideAllPages();
        var home = this.FindControl<StackPanel>("HomeContent");
        if (home != null) home.IsVisible = true;
    }

    private void AlbumsBtn_Click(object? sender, RoutedEventArgs e)
    {
        HideAllPages();
        var albums = this.FindControl<StackPanel>("AlbumsContent");
        if (albums != null) albums.IsVisible = true;

        ShowAlbum();
    }

    private void PlaylistsBtn_Click(object? sender, RoutedEventArgs e)
    {
        HideAllPages();
        var playlists = this.FindControl<StackPanel>("PlaylistsContent");
        if (playlists != null) playlists.IsVisible = true;

        ShowPlaylist();
    }

    private void AddAlbumBtn_Click(object? sender, RoutedEventArgs e)
    {
        HideAllPages();
        var admAlbum = this.FindControl<StackPanel>("AdmAlbumContent");
        if (admAlbum != null) admAlbum.IsVisible = true;
    }

    private void AddTrackBtn_Click(object? sender, RoutedEventArgs e)
    {
        HideAllPages();
        var admTrack = this.FindControl<StackPanel>("AdmTrackContent");
        if (admTrack != null) admTrack.IsVisible = true;
    }
    
    private async Task<Image?> LoadImageFromUrl(string url)
    {
        try
        {
            var data = await httpClient.GetByteArrayAsync(url);
            using (var stream = new System.IO.MemoryStream(data))
            {
                var bitmap = new Bitmap(stream);
                return new Image
                {
                    Source = bitmap,
                    Height = 120,
                    Width = 170,
                    Stretch = Stretch.UniformToFill
                };
            }
        }
        catch
        {
            return null;
        }
    }

    private string FormatSeconds(int totalSeconds)
    {
        if (totalSeconds <= 0) return "0:00";

        int hours = totalSeconds / 3600;
        int minutes = (totalSeconds % 3600) / 60;
        int seconds = totalSeconds % 60;

        if (hours > 0)
            return $"{hours}:{minutes:D2}:{seconds:D2}";
        else
            return $"{minutes}:{seconds:D2}";
    }

    private async void ShowAlbum()
    {
        var wrapPanel = this.FindControl<WrapPanel>("AlbumsWrapPanel");
        if (wrapPanel == null) return;

        wrapPanel.Children.Clear();

        try
        {
            using (var db = new PostgresContext())
            {
                var albums = db.Albums
                    .Include(a => a.Artist)
                    .Include(a => a.Tracks)
                    .OrderByDescending(a => a.AlbumId)
                    .ToList();

                if (albums.Count == 0)
                {
                    wrapPanel.Children.Add(new TextBlock
                    {
                        Text = "Альбомы не найдены",
                        FontSize = 18,
                        Foreground = Brushes.Gray
                    });
                    return;
                }

                foreach (var album in albums)
                {
                    var albumCard = new Border
                    {
                        Width = 200,
                        Height = 280,
                        Background = Brushes.White,
                        CornerRadius = new CornerRadius(10),
                        Margin = new Thickness(10),
                        BorderBrush = Brushes.LightGray,
                        BorderThickness = new Thickness(1),
                        Padding = new Thickness(0),
                        Cursor = new Cursor(StandardCursorType.Hand)
                    };

                    var stack = new StackPanel();

                    var imageContainer = new Border
                    {
                        Height = 140,
                        Width = 200,
                        CornerRadius = new CornerRadius(10, 10, 0, 0),
                        ClipToBounds = true,
                        Background = new SolidColorBrush(Color.Parse("#F0F0F0"))
                    };

                    var placeholder = new Border
                    {
                        Background = new SolidColorBrush(Color.Parse("#9C4DFF")),
                        Height = 140,
                        Width = 200
                    };

                    placeholder.Child = new TextBlock
                    {
                        Text = album.AlbumTitle.Length > 0 ? album.AlbumTitle[0].ToString() : "Undefindable",
                        FontSize = 32,
                        FontWeight = FontWeight.Bold,
                        Foreground = Brushes.White,
                        HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                        VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center
                    };

                    imageContainer.Child = placeholder;

                    if (!string.IsNullOrEmpty(album.CoverPath) &&
                        (album.CoverPath.StartsWith("http://") || album.CoverPath.StartsWith("https://")))
                    {
                        var image = await LoadImageFromUrl(album.CoverPath);
                        if (image != null)
                            imageContainer.Child = image;
                    }

                    var textPanel = new StackPanel
                    {
                        Margin = new Thickness(12),
                        Spacing = 4
                    };

                    var title = new TextBlock
                    {
                        Text = album.AlbumTitle.Length > 25
                            ? album.AlbumTitle.Substring(0, 25) + "..."
                            : album.AlbumTitle,
                        FontWeight = FontWeight.Bold,
                        FontSize = 14,
                        Foreground = Brushes.Black,
                        TextWrapping = TextWrapping.Wrap
                    };

                    var artist = new TextBlock
                    {
                        Text = album.Artist?.ArtistName ?? "Неизвестно",
                        FontSize = 13,
                        Foreground = Brushes.Gray,
                        Margin = new Thickness(0, 2, 0, 0)
                    };

                    var infoPanel = new WrapPanel { Margin = new Thickness(0, 5, 0, 0) };

                    infoPanel.Children.Add(new TextBlock
                    {
                        Text = $"Год: {album.ReleaseYear.Year}",
                        FontSize = 11,
                        Foreground = Brushes.DarkGray
                    });

                    var minutes = album.TotalDuration / 60;
                    infoPanel.Children.Add(new TextBlock
                    {
                        Text = $" Длительность: {minutes} мин",
                        FontSize = 11,
                        Foreground = Brushes.DarkGray
                    });

                    infoPanel.Children.Add(new TextBlock
                    {
                        Text = $"Треков: {album.Tracks}",
                        FontSize = 11,
                        Foreground = Brushes.DarkGray
                    });

                    textPanel.Children.Add(title);
                    textPanel.Children.Add(artist);
                    textPanel.Children.Add(infoPanel);

                    stack.Children.Add(imageContainer);
                    stack.Children.Add(textPanel);

                    albumCard.Child = stack;
                    
                    albumCard.PointerPressed += (sender, e) =>
                    {
                        var window = new AlbumDetails(album);
                        window.Show();
                    };

                    wrapPanel.Children.Add(albumCard);
                }
            }
        }
        catch (Exception ex)
        {
            wrapPanel.Children.Add(new TextBlock
            {
                Text = $"Ошибка: {ex.Message}",
                Foreground = Brushes.Red
            });
        }
    }

    private void ShowPlaylist()
    {
        var listPanel = this.FindControl<StackPanel>("PlaylistsListPanel");
        if (listPanel == null) return;

        listPanel.Children.Clear();

        try
        {
            using (var db = new PostgresContext())
            {
                var playlists = db.Playlists
                    .Include(p => p.User)
                    .ThenInclude(u => u.Subscription)
                    .Include(p => p.Tracks)
                    .Include(p => p.Users)
                    .ToList();

                if (playlists.Count == 0)
                {
                    listPanel.Children.Add(new TextBlock
                    {
                        Text = "Плейлистов нет",
                        FontSize = 16,
                        Foreground = Brushes.Gray,
                        Margin = new Thickness(0, 20, 0, 0)
                    });
                    return;
                }

                listPanel.Children.Add(CreateTableHeader());

                foreach (var playlist in playlists)
                    listPanel.Children.Add(CreateTableRow(playlist));
            }
        }
        catch (Exception ex)
        {
            listPanel.Children.Add(new TextBlock
            {
                Text = $"Ошибка: {ex.Message}",
                Foreground = Brushes.Red
            });
        }
    }

    private Border CreateTableHeader()
    {
        var header = new Border
        {
            Background = new SolidColorBrush(Color.Parse("#F0F0F0")),
            Height = 40,
            Margin = new Thickness(0, 0, 0, 10)
        };

        var grid = new Grid
        {
            ColumnDefinitions = new ColumnDefinitions("2*,*,*,*,*,*")
        };

        var titleHeader = new TextBlock
        {
            Text = "Название плейлиста | Автор",
            FontWeight = FontWeight.Bold,
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
            Margin = new Thickness(15, 0, 0, 0)
        };
        Grid.SetColumn(titleHeader, 0);

        var likesHeader = new TextBlock
        {
            Text = "Нравится",
            FontWeight = FontWeight.Bold,
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center
        };
        Grid.SetColumn(likesHeader, 1);

        var subsHeader = new TextBlock
        {
            Text = "Подписчиков",
            FontWeight = FontWeight.Bold,
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center
        };
        Grid.SetColumn(subsHeader, 2);

        var tracksHeader = new TextBlock
        {
            Text = "Треков",
            FontWeight = FontWeight.Bold,
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center
        };
        Grid.SetColumn(tracksHeader, 3);

        var durationHeader = new TextBlock
        {
            Text = "Длительность",
            FontWeight = FontWeight.Bold,
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center
        };
        Grid.SetColumn(durationHeader, 4);

        var dateHeader = new TextBlock
        {
            Text = "Создан",
            FontWeight = FontWeight.Bold,
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center
        };
        Grid.SetColumn(dateHeader, 5);

        grid.Children.Add(titleHeader);
        grid.Children.Add(likesHeader);
        grid.Children.Add(subsHeader);
        grid.Children.Add(tracksHeader);
        grid.Children.Add(durationHeader);
        grid.Children.Add(dateHeader);

        header.Child = grid;
        return header;
    }

    private Border CreateTableRow(Playlist playlist)
    {
        var row = new Border
        {
            Background = Brushes.White,
            BorderBrush = Brushes.LightGray,
            BorderThickness = new Thickness(0, 0, 0, 1),
            Height = 60,
            Cursor = new Cursor(StandardCursorType.Hand)
        };

        var grid = new Grid
        {
            ColumnDefinitions = new ColumnDefinitions("2*,*,*,*,*,*")
        };

        var titleStack = new StackPanel
        {
            Margin = new Thickness(15, 0, 0, 0),
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center
        };

        var title = new TextBlock
        {
            Text = playlist.PlaylistName,
            FontWeight = FontWeight.Bold,
            FontSize = 14
        };

        int totalSeconds = 0;
        if (playlist.Tracks != null && playlist.Tracks.Any())
            totalSeconds = playlist.Tracks.Sum(t => t.Duration);

        string formattedDuration = FormatSeconds(totalSeconds);

        if (playlist.User?.Subscription?.SubscriptionName?.ToLower() == "premium")
            title.Foreground = new SolidColorBrush(Color.Parse("#ff943d"));

        var author = new TextBlock
        {
            Text = playlist.User?.FullName ?? "Неизвестно",
            FontSize = 12,
            Foreground = Brushes.Gray
        };

        titleStack.Children.Add(title);
        titleStack.Children.Add(author);
        Grid.SetColumn(titleStack, 0);

        var likes = new TextBlock
        {
            Text = playlist.Likes.ToString(),
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
            FontSize = 14
        };
        Grid.SetColumn(likes, 1);

        var subs = new TextBlock
        {
            Text = (playlist.Users?.Count ?? 0).ToString(),
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
            FontSize = 14
        };
        Grid.SetColumn(subs, 2);

        var tracksCount = playlist.Tracks?.Count ?? 0;
        var tracks = new TextBlock
        {
            Text = tracksCount.ToString(),
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
            FontSize = 14,
            FontWeight = FontWeight.Bold
        };
        
        if (tracksCount == 0) tracks.Foreground = Brushes.Gray;
        Grid.SetColumn(tracks, 3);

        var durationPanel = new StackPanel
        {
            Orientation = Avalonia.Layout.Orientation.Horizontal,
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
            Spacing = 4
        };

        durationPanel.Children.Add(new TextBlock
        {
            Text = formattedDuration,
            FontSize = 14,
            Foreground = tracksCount == 0 ? Brushes.Gray : Brushes.Black
        });
        Grid.SetColumn(durationPanel, 4);

        var date = new TextBlock
        {
            Text = playlist.DateCreated.ToString("dd.MM.yyyy"),
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
            FontSize = 14
        };
        Grid.SetColumn(date, 5);

        grid.Children.Add(titleStack);
        grid.Children.Add(likes);
        grid.Children.Add(subs);
        grid.Children.Add(tracks);
        grid.Children.Add(durationPanel);
        grid.Children.Add(date);

        row.Child = grid;
        
        row.PointerPressed += (sender, e) =>
        {
            var window = new PlaylistDetails(playlist);
            window.Show();
        };
        
        return row;
    }

    private async void SaveAlbum_Click(object? sender, RoutedEventArgs e)
    {
        var title = this.FindControl<TextBox>("AlbumTitleBox")?.Text;
        var date = this.FindControl<DatePicker>("AlbumReleaseDatePicker")?.SelectedDate;
        var artist = this.FindControl<ComboBox>("AlbumArtistCombo")?.SelectedItem as Artist;
        var coverUrl = this.FindControl<TextBox>("AlbumCoverUrlBox")?.Text;
        var errorText = this.FindControl<TextBlock>("AlbumCoverErrorText");

        if (errorText != null)
            errorText.Text = "";

        if (string.IsNullOrWhiteSpace(title))
        {
            if (errorText != null) errorText.Text = "Введите название альбома";
            return;
        }

        if (date == null)
        {
            if (errorText != null) errorText.Text = "Выберите дату выпуска";
            return;
        }

        if (artist == null)
        {
            if (errorText != null) errorText.Text = "Выберите артиста";
            return;
        }

        if (string.IsNullOrWhiteSpace(coverUrl))
        {
            if (errorText != null) errorText.Text = "Введите URL обложки";
            return;
        }

        try
        {
            using (var db = new PostgresContext())
            {
                var album = new Album
                {
                    AlbumTitle = title,
                    ArtistId = artist.ArtistId,
                    ReleaseYear = DateOnly.FromDateTime(date.Value.DateTime),
                    CoverPath = coverUrl,
                    TotalDuration = 0
                };

                db.Albums.Add(album);
                await db.SaveChangesAsync();
            }

            LoadData();

            HideAllPages();

            var albums = this.FindControl<StackPanel>("AlbumsContent");
            if (albums != null)
                albums.IsVisible = true;

            ShowAlbum();

            if (errorText != null)
                errorText.Text = "Альбом успешно сохранён";
        }
        catch (Exception ex)
        {
            if (errorText != null)
                errorText.Text = "Ошибка сохранения: " + ex.Message;
        }
    }
    
    private async void SaveTrack_Click(object? sender, RoutedEventArgs e)
{
    var album = this.FindControl<ComboBox>("TrackAlbumCombo")?.SelectedItem as Album;
    var name = this.FindControl<TextBox>("TrackTitleBox")?.Text;
    var date = this.FindControl<DatePicker>("TrackReleaseDatePicker")?.SelectedDate;
    var durationText = this.FindControl<TextBox>("TrackDurationBox")?.Text;
    var bitrateText = this.FindControl<TextBox>("TrackBitrateBox")?.Text;
    var ratingText = this.FindControl<TextBox>("TrackRatingBox")?.Text;
    var coverUrl = this.FindControl<TextBox>("TrackCoverUrlBox")?.Text;
    var artistsList = this.FindControl<ListBox>("TrackArtistsList");
    var errorText = this.FindControl<TextBlock>("TrackCoverErrorText");
    var statusText = this.FindControl<TextBlock>("TrackSaveStatusText");

    if (errorText != null) errorText.Text = "";
    if (statusText != null) statusText.Text = "";

    if (album == null)
    {
        if (errorText != null) errorText.Text = "Выберите альбом";
        return;
    }

    if (string.IsNullOrWhiteSpace(name))
    {
        if (errorText != null) errorText.Text = "Введите название трека";
        return;
    }

    if (date == null)
    {
        if (errorText != null) errorText.Text = "Выберите дату выпуска";
        return;
    }

    if (!int.TryParse(durationText, out int duration))
    {
        if (errorText != null) errorText.Text = "Длительность должна быть числом";
        return;
    }

    if (!int.TryParse(bitrateText, out int bitrate))
    {
        if (errorText != null) errorText.Text = "Битрейт должен быть числом";
        return;
    }

    if (!decimal.TryParse(ratingText, out decimal rating))    
    {
        if (errorText != null) errorText.Text = "Рейтинг должен быть числом";
        return;
    }

    if (string.IsNullOrWhiteSpace(coverUrl))
    {
        if (errorText != null) errorText.Text = "Введите URL";
        return;
    }

    var artist = artistsList?.SelectedItems?.OfType<Artist>().FirstOrDefault();
    if (artist == null)
    {
        if (errorText != null) errorText.Text = "Выберите исполнителя";
        return;
    }

    try
    {
        using (var db = new PostgresContext())
        {
            var track = new Track
            {
                TrackName = name,
                AlbumId = album.AlbumId,
                ArtistId = artist.ArtistId,
                ReleaseDate = DateOnly.FromDateTime(date.Value.DateTime),
                Bitrate = bitrate,
                Rating = rating,
                Duration = duration,
                PlayCount = 0,
                FilePath = coverUrl
            };

            db.Tracks.Add(track);
            await db.SaveChangesAsync();
        }

        if (statusText != null)
            statusText.Text = "Трек успешно сохранён";
    }
    catch (Exception ex)
    {
        if (errorText != null)
            errorText.Text = "Ошибка сохранения: " + ex.Message;
    }
}
}