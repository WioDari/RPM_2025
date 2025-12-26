using System;
using System.Collections.Generic;

namespace BelokonevPRM.Models;

public partial class Artist
{
    public int ArtistId { get; set; }

    public string ArtistName { get; set; } = null!;

    public int? Country { get; set; }

    public int? Yearbegin { get; set; }

    public int? YearactiveUntill { get; set; }

    public string? Description { get; set; }

    public string? PhotoPath { get; set; }

    public virtual ICollection<Artistalbum> Artistalbums { get; set; } = new List<Artistalbum>();

    public virtual ICollection<Artistgenre> Artistgenres { get; set; } = new List<Artistgenre>();

    public virtual ICollection<Trackscreator> Trackscreators { get; set; } = new List<Trackscreator>();
}
