using System;
using System.Collections.Generic;

namespace MusicPlusPlus.Models;

public partial class Artistalbums
{
    public int Albumid { get; set; }

    public int Artistid { get; set; }

    public virtual Artist Artist { get; set; } = null!;
	public virtual Album Album { get; set; } = null!;
}
