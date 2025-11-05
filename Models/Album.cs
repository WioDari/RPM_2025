using System;
using System.Collections.Generic;

namespace Spotify.Models;

public partial class Album
{
    public int AlbumId { get; set; }

    public string AlbumTitle { get; set; } = null!;

    public int ArtistId { get; set; }

    public DateOnly ReleaseYear { get; set; }

    public string CoverPath { get; set; } = null!;

    public int TotalDuration { get; set; }

    public virtual Artist Artist { get; set; } = null!;

    public virtual ICollection<Track> Tracks { get; set; } = new List<Track>();
}
