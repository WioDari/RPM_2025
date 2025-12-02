using System;
using System.Collections.Generic;

namespace musicc.Models;

public partial class Album
{
    public int Id { get; set; }

    public string AlbumTitle { get; set; } = null!;

    public int Realease { get; set; }

    public string? Coverpath { get; set; }

    public int? Duration { get; set; }

    public virtual ICollection<Artist> Artists { get; set; } = new List<Artist>();

    public virtual ICollection<Gener> Geners { get; set; } = new List<Gener>();

    public virtual ICollection<Track> Tracks { get; set; } = new List<Track>();
}
