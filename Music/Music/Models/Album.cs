using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Windows.Media.Imaging;

namespace Music.Models;

public partial class Album
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public int ArtistId { get; set; }

    public int ReleaseYear { get; set; }

    public string? CoverPath { get; set; }

    public TimeSpan TotalDuration { get; set; }

    public virtual Artist Artist { get; set; } = null!;

    public virtual ObservableCollection<Track> Tracks { get; set; } = [];

    public virtual ObservableCollection<Genre> Genres { get; set; } = [];

    public BitmapImage Image => Img.GetImage(CoverPath);

    public string GenresString => string.Join(", ", Genres.Select(x => x.Name));
}
