using System;
using System.Collections.Generic;

namespace BE.Entities;

public partial class Shipping
{
    public int ShippingId { get; set; }

    public string? ShippingName { get; set; }

    public bool? Status { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
