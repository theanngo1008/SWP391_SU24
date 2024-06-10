using System;
using System.Collections.Generic;

namespace BE.Entities;

public partial class Warehouse
{
    public int WarehouseId { get; set; }

    public string? WarehouseName { get; set; }

    public string? Location { get; set; }

    public int? LimitAmount { get; set; }

    public bool? Status { get; set; }

    public virtual ICollection<Jewelry> Jewelries { get; set; } = new List<Jewelry>();
}
