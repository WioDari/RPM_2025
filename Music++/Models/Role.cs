using System;
using System.Collections.Generic;

namespace Music__.Models;

public partial class Role
{
    public short Roleid { get; set; }

    public string Rolename { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
