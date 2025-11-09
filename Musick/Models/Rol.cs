using System;
using System.Collections.Generic;

namespace Musick.Models;

public partial class Rol
{
    public int RoleId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
