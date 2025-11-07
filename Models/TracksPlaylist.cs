using System;
using System.Collections.Generic;

namespace MusicWpf.Models;

public partial class TracksPlaylist
{
    public int TracksPlaylistId { get; set; }

    public int? TracksId { get; set; }

    public int PlaylistId { get; set; }

    public virtual Playlist Playlist { get; set; } = null!;

    public virtual Track? Tracks { get; set; }
}
