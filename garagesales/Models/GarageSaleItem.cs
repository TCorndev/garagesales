using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace garagesales.Models;

public partial class GarageSaleItem
{
    public int Id { get; set; }

    public int GarageSaleId { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public decimal Price { get; set; }

    [JsonIgnore]
    public virtual GarageSale GarageSale { get; set; } = null!;
}
