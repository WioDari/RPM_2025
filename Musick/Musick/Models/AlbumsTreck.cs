using System;
using System.Collections.Generic;

namespace Musick.Models;

public partial class AlbumsTreck
{
    public int AlbumId { get; set; }

    public int TrackId { get; set; }

    public virtual Album Album { get; set; } = null!;

    public virtual Track Track { get; set; } = null!;
}
