using System;
using System.Collections.Generic;

namespace garagesales.Models;

public partial class GarageSale
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string City { get; set; } = null!;

    public string State { get; set; } = null!;

    public string ZipCode { get; set; } = null!;

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<GarageSaleItem> GarageSaleItems { get; set; } = new List<GarageSaleItem>();
}
