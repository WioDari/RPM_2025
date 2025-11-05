using System;
using System.Collections.Generic;

namespace PraktikaMusicF.Models;

public partial class Subscription
{
    public int SubId { get; set; }

    public string? SubName { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
