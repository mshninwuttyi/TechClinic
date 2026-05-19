using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TechClinic.MVC.Features.WorkOrder;

public class WorkOrderListViewModel
{
    public string Id { get; set; } = string.Empty;
    public string WorkOrderCode { get; set; } = string.Empty;
    public string? DeviceType { get; set; }
    public string? Brand { get; set; }
    public string? Status { get; set; }
    public DateTime IntakeDate { get; set; }
    public decimal? EstimatedCost { get; set; }
}

public class WorkOrderFilterViewModel
{
    public string? Keyword { get; set; }
    public string? Status { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public List<SelectListItem> StatusOptions { get; set; } = new();
}

public class SaveWorkOrderViewModel
{
    public string? Id { get; set; }

    [Display(Name = "Device Type")]
    public string? DeviceType { get; set; }

    public string? Brand { get; set; }

    [Display(Name = "Device Model")]
    public string? DeviceModel { get; set; }

    [Display(Name = "Serial Number")]
    public string? SerialNumber { get; set; }

    [Display(Name = "Problem Description")]
    public string? ProblemDescription { get; set; }

    [Required, Display(Name = "Intake Date")]
    [DataType(DataType.Date)]
    public DateTime IntakeDate { get; set; } = DateTime.Today;

    [Display(Name = "Est. Return Date")]
    [DataType(DataType.Date)]
    public DateTime? EstimatedReturnDate { get; set; }

    [Display(Name = "Est. Cost")]
    public decimal? EstimatedCost { get; set; }

    public bool IsEdit => !string.IsNullOrEmpty(Id);
}

public class WorkOrderDetailsViewModel
{
    public string Id { get; set; } = string.Empty;
    public string WorkOrderCode { get; set; } = string.Empty;
    public string? DeviceType { get; set; }
    public string? Brand { get; set; }
    public string? DeviceModel { get; set; }
    public string? SerialNumber { get; set; }
    public string? ProblemDescription { get; set; }
    public DateTime IntakeDate { get; set; }
    public DateTime? EstimatedReturnDate { get; set; }
    public decimal? EstimatedCost { get; set; }
    public string? Status { get; set; }

    // Assignment
    public string? AssignedTechnicianName { get; set; }
    public DateTime? AssignedDate { get; set; }
    public bool HasAssignment => !string.IsNullOrEmpty(AssignedTechnicianName);

    // Status update
    public List<SelectListItem> StatusOptions { get; set; } = new();
}
