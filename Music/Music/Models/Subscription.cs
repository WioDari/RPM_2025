using System;
using System.Collections.Generic;

namespace Music.Models;

public partial class Subscription
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
