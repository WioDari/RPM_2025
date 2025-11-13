using System;
using System.Collections.Generic;

namespace Music__.Models;

public partial class Playlist
{
    public short Playlistid { get; set; }

    public string Playlistname { get; set; } = null!;

    public int Userid { get; set; }

    public DateOnly Playlistcreationdate { get; set; }

    public int Likes { get; set; }

    public string? Playlistdescription { get; set; }

    public virtual User User { get; set; } = null!;

    public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();

    public virtual ICollection<Track> Tracks { get; set; } = new List<Track>();
}
