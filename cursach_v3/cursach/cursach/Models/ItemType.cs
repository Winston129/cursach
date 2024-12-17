using System;
using System.Collections.Generic;

namespace cursach.Models;

public partial class ItemType
{
    public int ItemTypeId { get; set; }

    public string? NameType { get; set; }

    public virtual ICollection<Item> Items { get; set; } = new List<Item>();
}
