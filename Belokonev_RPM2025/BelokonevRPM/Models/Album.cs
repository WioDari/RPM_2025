using System;
using System.Collections.Generic;

namespace BelokonevPRM.Models;

public partial class Album
{
    public int AlbumId { get; set; }

    public string AlbumName { get; set; } = null!;

    public int Realeaseyear { get; set; }

    public string? CoverPath { get; set; }

    public string? CoverBinary { get; set; }

    public int? Totaldurations { get; set; }

    public virtual ICollection<Albumgenre> Albumgenres { get; set; } = new List<Albumgenre>();

    public virtual ICollection<Artistalbum> Artistalbums { get; set; } = new List<Artistalbum>();

    public virtual ICollection<Track> Tracks { get; set; } = new List<Track>();
}
