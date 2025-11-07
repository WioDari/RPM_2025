using System;
using System.Collections.Generic;

namespace SpotApp_wpf.Models;

public partial class TagsInPlaylist
{
    public int TiPId { get; set; }

    public int TagId { get; set; }

    public int? PlaylistId { get; set; }

    public virtual Playlist? Playlist { get; set; }

    public virtual Tag Tag { get; set; } = null!;
}
