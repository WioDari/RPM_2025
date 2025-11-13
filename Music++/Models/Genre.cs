using System;
using System.Collections.Generic;

namespace Music__.Models;

public partial class Genre
{
    public int Genreid { get; set; }

    public string Genrename { get; set; } = null!;

    public virtual ICollection<Album> Albums { get; set; } = new List<Album>();

    public virtual ICollection<Artist> Artists { get; set; } = new List<Artist>();

    public virtual ICollection<Track> Tracks { get; set; } = new List<Track>();
}
