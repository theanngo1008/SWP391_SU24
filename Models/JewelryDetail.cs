using System;
using System.Collections.Generic;

namespace BE.Models;

public partial class JewelryDetail
{
    public int JewelryDetailId { get; set; }

    public string? JewelryDetailName { get; set; }

    public decimal? SpotPrice { get; set; }

    public bool? Status { get; set; }

    public int? JewelryId { get; set; }

    public virtual Jewelry? Jewelry { get; set; }
}
