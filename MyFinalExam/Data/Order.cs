using System;
using System.Collections.Generic;

namespace MyFinalExam.Data;

public partial class Order
{
    public int Id { get; set; }

    public string? CustomerId { get; set; }

    public DateTime OrderDate { get; set; }

    public decimal? TotalAmount { get; set; }

    public string? Status { get; set; }

    public string? ShippingAddress { get; set; }

    public DateOnly ReceiveDay { get; set; }

    public string? ReceiverName { get; set; }

    public string? ReceiverPhone { get; set; }

    public string? Note { get; set; }

    public string? City { get; set; }

    public string? Email { get; set; }

    public string? Pay { get; set; }

    public DateOnly? ShippingDate { get; set; }

    public int? PaymentId { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual Payment? Payment { get; set; }
}
