using System;
using System.Collections.Generic;

namespace BE.Entities;

public partial class OrderDetail
{
    public int OrderDetailId { get; set; }

    public int? Quantity { get; set; }

    public DateTime? CreateDate { get; set; }

    public string? DetailStatus { get; set; }

    public int? JewelryId { get; set; }

    public int? OrderId { get; set; }

    public virtual Jewelry? Jewelry { get; set; }

    public virtual Order? Order { get; set; }
}
