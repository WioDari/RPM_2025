using System;
using System.Collections.Generic;

namespace Spotify_wpf.Models;

public partial class PlaylistUser
{
    public int PlaylistUserId { get; set; }

    public int PlaylistId { get; set; }

    public int UserId { get; set; }

    public virtual Playlist Playlist { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
