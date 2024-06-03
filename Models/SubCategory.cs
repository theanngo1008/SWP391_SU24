using System;
using System.Collections.Generic;

namespace BE.Models;

public partial class SubCategory
{
    public string SubCateId { get; set; } = null!;

    public string? SubCateName { get; set; }

    public bool? Status { get; set; }

    public string? CateId { get; set; }

    public virtual Category? Cate { get; set; }

    public virtual ICollection<Jewelry> Jewelries { get; set; } = new List<Jewelry>();
}
