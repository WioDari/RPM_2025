using System;
using System.Collections.Generic;

namespace Practos2.Models;

public partial class Albumcoverpath
{
    public int Albumcoverpathid { get; set; }

    public string? Coverpath { get; set; }

    public string? Covarbinary { get; set; }

    public virtual ICollection<Album> Albums { get; set; } = new List<Album>();
}
