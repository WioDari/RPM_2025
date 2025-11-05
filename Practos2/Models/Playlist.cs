using System;
using System.Collections.Generic;

namespace Practos2.Models;

public partial class Playlist
{
    public int Playlistid { get; set; }

    public string? Playlistname { get; set; }

    public int? Userid { get; set; }

    public DateOnly? Datecreated { get; set; }

    public int? Likes { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<PlaylistTrack> PlaylistTracks { get; set; } = new List<PlaylistTrack>();

    public virtual ICollection<TagPlaylist> TagPlaylists { get; set; } = new List<TagPlaylist>();

    public virtual User? User { get; set; }
}
