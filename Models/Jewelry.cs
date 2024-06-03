using System;
using System.Collections.Generic;

namespace BE.Models;

public partial class Jewelry
{
    public int JewelryId { get; set; }

    public string? JewelryName { get; set; }

    public byte[]? Image { get; set; }

    public decimal? Cost { get; set; }

    public int? Quantity { get; set; }

    public bool? Status { get; set; }

    public int? WagesId { get; set; }

    public int? QuotationId { get; set; }

    public int? WarehouseId { get; set; }

    public string? SubCateId { get; set; }

    public virtual ICollection<JewelryDetail> JewelryDetails { get; set; } = new List<JewelryDetail>();

    public virtual ICollection<JewelryGemstone> JewelryGemstones { get; set; } = new List<JewelryGemstone>();

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual Quotation? Quotation { get; set; }

    public virtual SubCategory? SubCate { get; set; }

    public virtual Wage? Wages { get; set; }

    public virtual Warehouse? Warehouse { get; set; }
}
