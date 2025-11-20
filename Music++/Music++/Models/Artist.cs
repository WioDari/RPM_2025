using System;
using System.Collections.Generic;

namespace Music__.Models;

public partial class Artist
{
    public int Artistid { get; set; }

    public string Artistname { get; set; } = null!;

    public int Countryid { get; set; }

    public string Activeyears { get; set; } = null!;

    public string Artistdescription { get; set; } = null!;

    public string Photopath { get; set; } = null!;

    public virtual Country Country { get; set; } = null!;

    public virtual ICollection<Album> Albums { get; set; } = new List<Album>();

    public virtual ICollection<Genre> Genres { get; set; } = new List<Genre>();

    public virtual ICollection<Track> Tracks { get; set; } = new List<Track>();
}
