using System;
using System.Collections.Generic;

namespace BelokonevPRM.Models;

public partial class Playlist
{
    public int PlaylistId { get; set; }

    public string PlaylistTitle { get; set; } = null!;

    public int Creator { get; set; }

    public string? Description { get; set; }

    public int? Likes { get; set; }

    public DateOnly? Datecreated { get; set; }

    public virtual User CreatorNavigation { get; set; } = null!;

    public virtual ICollection<Playlisttrack> Playlisttracks { get; set; } = new List<Playlisttrack>();
}
