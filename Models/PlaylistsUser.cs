using System;
using System.Collections.Generic;

namespace MusicWpf.Models;

public partial class PlaylistsUser
{
    public int PlaylistsUserId { get; set; }

    public int PlaylistsId { get; set; }

    public int UserId { get; set; }

    public virtual Playlist Playlists { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
