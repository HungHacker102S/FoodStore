using System;
using System.Collections.Generic;

namespace Project_PRN.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string? ProductName { get; set; }

    public int CategoryId { get; set; }

    public string? Image { get; set; }

    public float? ProductPrice { get; set; }

    public int? Quantity { get; set; }

    public virtual ICollection<Cart> Carts { get; } = new List<Cart>();

    public virtual Category Category { get; set; } = null!;
}
