using System;
using System.Collections.Generic;

namespace API.NewDB;

public partial class Product
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public decimal Value { get; set; }

    public int Quantity { get; set; }

    public int ProductTypeId { get; set; }

    public virtual ICollection<OrderStructure> OrderStructures { get; set; } = new List<OrderStructure>();

    public virtual ICollection<ProductParameter> ProductParameters { get; set; } = new List<ProductParameter>();

    public virtual ProductType ProductType { get; set; } = null!;
}
