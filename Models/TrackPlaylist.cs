using System;
using System.Collections.Generic;

namespace PracticeWork.Models;

public partial class TrackPlaylist
{
    public int? Track { get; set; }

    public int? Playlist { get; set; }

    public virtual PlayList? PlaylistNavigation { get; set; }

    public virtual Track? TrackNavigation { get; set; }
}
