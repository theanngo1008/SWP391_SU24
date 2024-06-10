using System;
using System.Collections.Generic;

namespace BE.Entities;

public partial class Wage
{
    public int WagesId { get; set; }

    public string? WagesName { get; set; }

    public decimal? Price { get; set; }

    public virtual ICollection<Jewelry> Jewelries { get; set; } = new List<Jewelry>();
}
