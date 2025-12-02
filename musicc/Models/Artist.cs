using System;
using System.Collections.Generic;

namespace musicc.Models;

public partial class Artist
{
    public int ArtistId { get; set; }

    public string? ArtName { get; set; }

    public string? Country { get; set; }

    public int? AlbumId { get; set; }

    public string? YearActive { get; set; }

    public string? Description { get; set; }

    public string? Photo { get; set; }

    public virtual ICollection<Album> Albums { get; set; } = new List<Album>();

    public virtual ICollection<Track> Tracks { get; set; } = new List<Track>();
}
