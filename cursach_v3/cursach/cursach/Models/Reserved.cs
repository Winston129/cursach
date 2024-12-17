using System;
using System.Collections.Generic;

namespace cursach.Models;

public partial class Reserved
{
    public int ReservedId { get; set; }

    public int? ClientId { get; set; }

    public DateOnly? ReservedDate { get; set; }

    public DateOnly? ExpirationDate { get; set; }

    public decimal? InterestRate { get; set; }

    public virtual Client? Client { get; set; }

    public virtual ICollection<Item> Items { get; set; } = new List<Item>();
}
