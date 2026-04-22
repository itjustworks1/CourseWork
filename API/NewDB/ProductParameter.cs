using System;
using System.Collections.Generic;

namespace API.NewDB;

public partial class ProductParameter
{
    public int Id { get; set; }

    public string Meaning { get; set; } = null!;

    public int ParameterId { get; set; }

    public int ProductId { get; set; }

    public virtual Parameter Parameter { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
