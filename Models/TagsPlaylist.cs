using System;
using System.Collections.Generic;

namespace Spotify_wpf.Models;

public partial class TagsPlaylist
{
    public int TagsPlaylistId { get; set; }

    public int TagsId { get; set; }

    public int PlaylistId { get; set; }

    public virtual Playlist Playlist { get; set; } = null!;

    public virtual Tag Tags { get; set; } = null!;
}
