using System;
using System.Collections.Generic;

namespace TechClinic.Database.AppDbContextModels;

public partial class TblTechnician
{
    public string Id { get; set; } = null!;

    public string TechnicianCode { get; set; } = null!;

    public string UserId { get; set; } = null!;

    public string? PhoneNo { get; set; }

    public string? Skill { get; set; }

    public string? Status { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public string? ModifiedBy { get; set; }

    public bool DeleteFlag { get; set; }
}
