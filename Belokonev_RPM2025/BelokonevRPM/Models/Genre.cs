using System;
using System.Collections.Generic;

namespace BelokonevPRM.Models;

public partial class Genre
{
    public int GenreId { get; set; }

    public string? Genre1 { get; set; }

    public virtual ICollection<Albumgenre> Albumgenres { get; set; } = new List<Albumgenre>();

    public virtual ICollection<Artistgenre> Artistgenres { get; set; } = new List<Artistgenre>();
}
