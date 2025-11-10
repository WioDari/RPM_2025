using System;
using System.Collections.Generic;

namespace PracticeWork.Models;

public partial class Subscription
{
    public int SubId { get; set; }

    public string SubName { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
