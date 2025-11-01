using System;
using System.Collections.Generic;

namespace Music.Models;

public partial class Playlist
{
    public int PlaylistId { get; set; }

    public string PlaylistName { get; set; } = null!;

    public int UserId { get; set; }

    public DateOnly CreationDate { get; set; }

    public int Likes { get; set; }

    public string? Description { get; set; }

    public virtual User User { get; set; } = null!;

    public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();

    public virtual ICollection<Track> Tracks { get; set; } = new List<Track>();
}
