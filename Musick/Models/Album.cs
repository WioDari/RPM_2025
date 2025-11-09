using System;
using System.Collections.Generic;

namespace Musick.Models;

public partial class Album
{
    public int AlbumId { get; set; }

    public string AlbumName { get; set; } = null!;

    public int ArtistId { get; set; }

    public int RealiseDate { get; set; }

    public string AlbumCoverPath { get; set; } = null!;

    public TimeOnly? TotalDuration { get; set; }

    public virtual AlbumsTreck? AlbumsTreck { get; set; }

    public virtual Artist Artist { get; set; } = null!;

    public virtual ICollection<Genre> Genres { get; set; } = new List<Genre>();
}
