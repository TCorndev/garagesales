using System;
using System.Collections.Generic;

namespace garagesales.Models;

public partial class GarageSaleItem
{
    public int Id { get; set; }

    public int GarageSaleId { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public decimal Price { get; set; }

    public virtual GarageSale GarageSale { get; set; } = null!;
}
