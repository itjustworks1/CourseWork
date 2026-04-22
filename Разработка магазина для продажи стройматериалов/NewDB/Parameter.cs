using System;
using System.Collections.Generic;

namespace Magaz_Stroitelya.NewDB;

public partial class Parameter
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<ProductParameter> ProductParameters { get; set; } = new List<ProductParameter>();

    public virtual ICollection<ProductType> ProductTypes { get; set; } = new List<ProductType>();
}
