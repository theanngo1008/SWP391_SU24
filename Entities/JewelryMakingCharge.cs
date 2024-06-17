using System;
using System.Collections.Generic;

namespace BE.Entities;

public partial class JewelryMakingCharge
{
    public int ChargeId { get; set; }

    public string? ChargeName { get; set; }

    public decimal? Price { get; set; }

    public virtual ICollection<Jewelry> Jewelries { get; set; } = new List<Jewelry>();
}
