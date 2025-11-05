using System;
using System.Collections.Generic;

namespace Spotify_wpf.Models;

public partial class Tag
{
    public int TagsId { get; set; }

    public string TagsName { get; set; } = null!;

    public virtual ICollection<TagsPlaylist> TagsPlaylists { get; set; } = new List<TagsPlaylist>();
}
