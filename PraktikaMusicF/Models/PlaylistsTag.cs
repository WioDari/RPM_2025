using System;
using System.Collections.Generic;

namespace PraktikaMusicF.Models;

public partial class PlaylistsTag
{
    public int PlTagId { get; set; }

    public int? PlId { get; set; }

    public int? TagId { get; set; }

    public virtual Playlist? Pl { get; set; }

    public virtual Tag? Tag { get; set; }
}
