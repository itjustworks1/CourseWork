using System;
using System.Collections.Generic;

namespace API.NewDB;

public partial class OrderStructure
{
    public int Id { get; set; }

    public int Quantity { get; set; }

    public decimal Value { get; set; }

    public int ProductId { get; set; }

    public int OrderId { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
