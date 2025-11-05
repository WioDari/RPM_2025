using System;
using System.Collections.Generic;

namespace SpotApp_wpf.Models;

public partial class TracksInAlbum
{
    public int TiAId { get; set; }

    public int? TrackId { get; set; }

    public int? AlbumId { get; set; }

    public virtual Album? Album { get; set; }

    public virtual Track? Track { get; set; }
}
