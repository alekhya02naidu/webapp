using System;
using System.Collections.Generic;

namespace Hello.DB.Models;

public partial class Itemcost
{
    public int Icost { get; set; }

    public int? Quantity { get; set; }

    public virtual ICollection<Item> Items { get; set; } = new List<Item>();
}
