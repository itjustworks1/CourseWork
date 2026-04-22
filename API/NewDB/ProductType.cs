using System;
using System.Collections.Generic;

namespace Magaz_Stroitelya.NewDB;

public partial class ProductType
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public virtual ICollection<Parameter> Parameters { get; set; } = new List<Parameter>();
}
