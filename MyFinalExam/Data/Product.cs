using System;
using System.Collections.Generic;

namespace MyFinalExam.Data;

public partial class Product
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? CategoryId { get; set; }

    public decimal? Price { get; set; }

    public string? ImageUrl { get; set; }

    public int? StockQuantity { get; set; }

    public int? Sale { get; set; }

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual Category? Category { get; set; }
}
