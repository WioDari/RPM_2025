using System;
using System.Collections.Generic;

namespace SpotApp_wpf.Models;

public partial class Tag
{
    public int TagId { get; set; }

    public string? TagTittle { get; set; }

    public virtual ICollection<TagsInPlaylist> TagsInPlaylists { get; set; } = new List<TagsInPlaylist>();
}
