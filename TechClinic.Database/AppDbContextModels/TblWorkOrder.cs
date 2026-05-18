using System;
using System.Collections.Generic;

namespace TechClinic.Database.AppDbContextModels;

public partial class TblWorkOrder
{
    public string Id { get; set; } = null!;

    public string WorkOrderCode { get; set; } = null!;

    public string? DeviceType { get; set; }

    public string? Brand { get; set; }

    public string? Model { get; set; }

    public string? SerialNumber { get; set; }

    public string? ProblemDescription { get; set; }

    public DateTime IntakeDate { get; set; }

    public DateTime? EstimatedReturnDate { get; set; }

    public decimal? EstimatedCost { get; set; }

    public string? Status { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public string? ModifiedBy { get; set; }

    public bool DeleteFlag { get; set; }
}
