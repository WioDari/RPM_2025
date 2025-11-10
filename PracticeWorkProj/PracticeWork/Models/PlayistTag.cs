using System;
using System.Collections.Generic;

namespace PracticeWork.Models;

public partial class PlayistTag
{
    public int? Tag { get; set; }

    public int? PlayList { get; set; }

    public virtual PlayList? PlayListNavigation { get; set; }

    public virtual Tag? TagNavigation { get; set; }
}
