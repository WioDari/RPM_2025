using System;
using System.Collections.Generic;

namespace spotify.Models;

public partial class UsersHistory
{
    public int UserHistoryId { get; set; }

    public int UserId { get; set; }

    public DateTime History { get; set; }

    public virtual User UserHistory { get; set; } = null!;
}
