using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TechClinic.MVC.Features.Assignment;

public class AssignViewModel
{
    [Required]
    public string WorkOrderId { get; set; } = string.Empty;

    public string WorkOrderCode { get; set; } = string.Empty;

    [Required, Display(Name = "Technician")]
    public string TechnicianId { get; set; } = string.Empty;

    public List<SelectListItem> Technicians { get; set; } = new();

    public bool IsReassign { get; set; }
}
