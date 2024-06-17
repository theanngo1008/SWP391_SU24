using System;
using System.Collections.Generic;

namespace BE.Entities;

public partial class Jewelry
{
    public int JewelryId { get; set; }

    public string? JewelryName { get; set; }

    public string? Image { get; set; }

    public decimal? Cost { get; set; }

    public int? Quantity { get; set; }

    public bool? Status { get; set; }

    public int? ChargeId { get; set; }

    public int? QuotationId { get; set; }

    public int? WarehouseId { get; set; }

    public string? SubCateId { get; set; }

    public virtual JewelryMakingCharge? Charge { get; set; }

    public virtual ICollection<Gemstone> Gemstones { get; set; } = new List<Gemstone>();

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual Quotation? Quotation { get; set; }

    public virtual SubCategory? SubCate { get; set; }

    public virtual Warehouse? Warehouse { get; set; }
}
