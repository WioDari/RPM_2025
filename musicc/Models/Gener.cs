using System;
using System.Collections.Generic;

namespace musicc.Models;

public partial class Gener
{
    public int Id { get; set; }

    public string? Geners { get; set; }

    public virtual ICollection<Album> Albums { get; set; } = new List<Album>();

    public virtual ICollection<Track> Tracks { get; set; } = new List<Track>();
}
