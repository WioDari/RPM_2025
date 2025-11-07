using System;
using System.Collections.Generic;

namespace MusicWpf.Models;

public partial class PlaylistTag
{
    public int PlaylistTagsId { get; set; }

    public int TagsId { get; set; }

    public int PlaylistId { get; set; }

    public virtual Playlist Playlist { get; set; } = null!;

    public virtual Tag Tags { get; set; } = null!;
}
