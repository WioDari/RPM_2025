using System;
using System.Collections.Generic;

namespace BelokonevPRM.Models;

public partial class Playlisttrack
{
    public int Id { get; set; }

    public int Playlist { get; set; }

    public int Track { get; set; }

    public virtual Playlist PlaylistNavigation { get; set; } = null!;

    public virtual Track TrackNavigation { get; set; } = null!;
}
