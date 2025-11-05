using System;
using System.Collections.Generic;

namespace spotify.Models;

public partial class Album
{
    public int Id { get; set; }

    public string AlbumTitle { get; set; } = null!;

    public int ArtistId { get; set; }

    public int ReleaseYear { get; set; }

    public string CoverPath { get; set; } = null!;

    public TimeOnly TotalDuration { get; set; }

    public virtual Artist Artist { get; set; } = null!;

    public virtual ICollection<Track> Tracks { get; set; } = new List<Track>();

    public virtual ICollection<Genre> Genres { get; set; } = new List<Genre>();
}
