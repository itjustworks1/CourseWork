using System;
using System.Collections.Generic;

namespace Magaz_Stroitelya.NewDB;

public partial class Order
{
    public int Id { get; set; }

    public DateTime Date { get; set; }

    public bool Status { get; set; }

    public int UserId { get; set; }

    public virtual ICollection<OrderStructure> OrderStructures { get; set; } = new List<OrderStructure>();

    public virtual User User { get; set; } = null!;
}
