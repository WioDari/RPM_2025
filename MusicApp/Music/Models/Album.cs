using System;
using System.Collections.Generic;

namespace Music.Models;

public partial class Album
{
    public int AlbumId { get; set; }

    public string AlbumName { get; set; } = null!;

    public int ArtistId { get; set; }

    public int ReleaseYear { get; set; }

    public int TotalDuration { get; set; }

    public string CoverPath { get; set; } = null!;

    public virtual Artist Artist { get; set; } = null!;

    public virtual ICollection<Track> Tracks { get; set; } = new List<Track>();

    public virtual ICollection<Genre> Genres { get; set; } = new List<Genre>();
}
