using System;
using System.Collections.Generic;

namespace MusicWpf.Models;

public partial class Playlist
{
    public int PlaylistId { get; set; }

    public string PlaylistName { get; set; } = null!;

    public int Likes { get; set; }

    public string Description { get; set; } = null!;

    public DateOnly DateCreated { get; set; }

    public int? UserId { get; set; }

    public virtual ICollection<PlaylistTag> PlaylistTags { get; set; } = new List<PlaylistTag>();

    public virtual ICollection<PlaylistsUser> PlaylistsUsers { get; set; } = new List<PlaylistsUser>();

    public virtual ICollection<TracksPlaylist> TracksPlaylists { get; set; } = new List<TracksPlaylist>();

    public virtual User? User { get; set; }
}
