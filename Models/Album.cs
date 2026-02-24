using System;
using System.Collections.Generic;

namespace PracticeWork.Models;

public partial class Album
{
    public int AlbumId { get; set; }

    public string AlbumName { get; set; } = null!;

    public int? Artist { get; set; }

    public int ReleaseYear { get; set; }

    public string CoverPath { get; set; } = null!;

    public TimeSpan? TotalDuration { get; set; }

    public virtual Artist? ArtistNavigation { get; set; }
}
