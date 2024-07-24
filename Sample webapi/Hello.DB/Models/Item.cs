﻿using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Hello.DB.Models;

public partial class Item
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int? Cost { get; set; }
    [JsonIgnore]
    public virtual Itemcost? CostNavigation { get; set; }
}
