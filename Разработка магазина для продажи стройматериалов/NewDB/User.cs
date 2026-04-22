using System;
using System.Collections.Generic;

namespace Magaz_Stroitelya.NewDB;

public partial class User
{
    public int Id { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public bool IsAdmin { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
