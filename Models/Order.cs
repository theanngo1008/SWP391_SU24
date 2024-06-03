using System;
using System.Collections.Generic;

namespace BE.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public string? OrderName { get; set; }

    public DateOnly? OrderDate { get; set; }

    public string? OrderStatus { get; set; }

    public bool? Status { get; set; }

    public int? AccId { get; set; }

    public int? ShippingId { get; set; }

    public virtual Account? Acc { get; set; }

    public virtual Message? Message { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual Shipping? Shipping { get; set; }
}
