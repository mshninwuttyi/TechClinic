using System;
using System.Collections.Generic;

namespace TechClinic.Database.AppDbContextModels;

public partial class TblWorkOrderAssignment
{
    public string Id { get; set; } = null!;

    public string AssignmentCode { get; set; } = null!;

    public string WorkOrderId { get; set; } = null!;

    public string TechnicianId { get; set; } = null!;

    public DateTime AssignedDate { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public string? ModifiedBy { get; set; }

    public bool DeleteFlag { get; set; }
}
