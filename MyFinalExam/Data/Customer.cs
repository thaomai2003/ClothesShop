using System;
using System.Collections.Generic;

namespace MyFinalExam.Data;

public partial class Customer
{
    public string Id { get; set; } = null!;

    public string? Name { get; set; }

    public int? Age { get; set; }

    public string? Address { get; set; }

    public string? Phone { get; set; }

    public string? Password { get; set; }

    public int? Role { get; set; }

    public string? City { get; set; }

    public string? Email { get; set; }

    public bool? Sex { get; set; }

    public string? Image { get; set; }

    public string? RandomKey { get; set; }

    public bool Effect { get; set; }

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
