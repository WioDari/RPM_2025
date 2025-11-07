using System;
using System.Collections.Generic;

namespace MusicWpf.Models;

public partial class Tag
{
    public int TagsId { get; set; }

    public string TagsName { get; set; } = null!;

    public virtual ICollection<PlaylistTag> PlaylistTags { get; set; } = new List<PlaylistTag>();
}
