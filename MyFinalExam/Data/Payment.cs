using System;
using System.Collections.Generic;

namespace MyFinalExam.Data;

public partial class Payment
{
    public int Id { get; set; }

    public DateOnly? PaymentDate { get; set; }

    public string? PaymentMethod { get; set; }

    public int? PaymentStatus { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
