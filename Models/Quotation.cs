using System;
using System.Collections.Generic;

namespace BE.Models;

public partial class Quotation
{
    public int QuotationId { get; set; }

    public string? QuotationName { get; set; }

    public decimal? TotalPrice { get; set; }

    public string? QuotationStatus { get; set; }

    public bool? Status { get; set; }

    public int? AccId { get; set; }

    public virtual Account? Acc { get; set; }

    public virtual ICollection<Jewelry> Jewelries { get; set; } = new List<Jewelry>();
}
