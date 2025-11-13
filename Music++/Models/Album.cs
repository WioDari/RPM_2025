using System;
using System.Collections.Generic;

namespace Music__.Models;

public partial class Album
{
    public int Albumid { get; set; }

    public string Albumname { get; set; } = null!;

    public string Albumreleaseyear { get; set; } = null!;

    public string Coverpath { get; set; } = null!;

    public string Albumduration { get; set; } = null!;

    public virtual ICollection<Artist> Artists { get; set; } = new List<Artist>();

    public virtual ICollection<Genre> Genres { get; set; } = new List<Genre>();

    public virtual ICollection<Track> Tracks { get; set; } = new List<Track>();
}
