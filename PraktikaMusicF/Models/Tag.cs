using System;
using System.Collections.Generic;

namespace PraktikaMusicF.Models;

public partial class Tag
{
    public int TagId { get; set; }

    public string? TagName { get; set; }

    public virtual ICollection<PlaylistsTag> PlaylistsTags { get; set; } = new List<PlaylistsTag>();
}
