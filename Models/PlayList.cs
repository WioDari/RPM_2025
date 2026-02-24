using System;
using System.Collections.Generic;

namespace PracticeWork.Models;

public partial class PlayList
{
    public int PlaylistId { get; set; }

    public string PlaylistName { get; set; } = null!;

    public int? UserCreator { get; set; }

    public DateOnly CreationDate { get; set; }

    public int? Likes { get; set; }

    public string? PlaylistDescription { get; set; }

    public virtual User? UserCreatorNavigation { get; set; }
}
