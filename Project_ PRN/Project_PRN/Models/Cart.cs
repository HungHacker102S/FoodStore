using System;
using System.Collections.Generic;

namespace Project_PRN.Models;

public partial class Cart
{
    public int CartId { get; set; }

    public int ProductId { get; set; }

    public int? CartQuantity { get; set; }

    public virtual Product Product { get; set; } = null!;
}
