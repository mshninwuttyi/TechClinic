using System;
using System.Collections.Generic;

namespace TechClinic.Database.AppDbContextModels;

public partial class TblWorkOrderPart
{
    public string Id { get; set; } = null!;

    public string WorkOrderPartCode { get; set; } = null!;

    public string WorkOrderId { get; set; } = null!;

    public string SparePartId { get; set; } = null!;

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public string? ModifiedBy { get; set; }

    public bool DeleteFlag { get; set; }
}
