using System;
using System.Collections.Generic;

namespace Music.Models;

public partial class LoginAttempt
{
    public int LoginId { get; set; }

    public int UserId { get; set; }

    public DateTime LoginTime { get; set; }

    public bool IsSuccess { get; set; }

    public virtual User User { get; set; } = null!;
}
