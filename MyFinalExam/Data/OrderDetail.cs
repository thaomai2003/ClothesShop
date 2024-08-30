﻿using System;
using System.Collections.Generic;

namespace MyFinalExam.Data;

public partial class OrderDetail
{
    public int Id { get; set; }

    public int? Quantity { get; set; }

    public decimal? UnitPrice { get; set; }

    public int OrderId { get; set; }

    public int ProductId { get; set; }

    public virtual Order? Order { get; set; }
}
