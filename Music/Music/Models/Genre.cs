using System;
using System.Collections.Generic;

namespace Music.Models;

public partial class Genre
{
    public int GenreId { get; set; }

    public string GenreName { get; set; } = null!;

    public virtual ICollection<Album> Albums { get; set; } = new List<Album>();

    public virtual ICollection<Artist> Artists { get; set; } = new List<Artist>();

    public virtual ICollection<Track> Tracks { get; set; } = new List<Track>();
}
