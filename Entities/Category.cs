using System;
using System.Collections.Generic;

namespace BE.Entities;

public partial class Category
{
    public string CateId { get; set; } = null!;

    public string? CateName { get; set; }

    public bool? Status { get; set; }

    public virtual ICollection<SubCategory> SubCategories { get; set; } = new List<SubCategory>();
}
