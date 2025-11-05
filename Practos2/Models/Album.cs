using System;
using System.Collections.Generic;

namespace Practos2.Models;

public partial class Album
{
    public int Albumid { get; set; }

    public string? Albumname { get; set; }

    public int? Releaseyear { get; set; }

    public string? Authorid { get; set; }

    public int? Albumcoverpathid { get; set; }

    public int? Totalduration { get; set; }

    public virtual Albumcoverpath? Albumcoverpath { get; set; }

    public virtual ICollection<Track> Tracks { get; set; } = new List<Track>();
}
