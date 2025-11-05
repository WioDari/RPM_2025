using System;
using System.Collections.Generic;

namespace Practos2.Models;

public partial class ArtistAlbum
{
    public int Artistalbumid { get; set; }

    public int? Artistid { get; set; }

    public int? Albumid { get; set; }

    public virtual Artist? Artist { get; set; }
}
