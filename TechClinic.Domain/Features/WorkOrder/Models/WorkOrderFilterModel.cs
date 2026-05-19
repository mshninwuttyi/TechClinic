namespace TechClinic.Domain.Features.WorkOrder.Models;

public class WorkOrderFilterModel
{
    public string? Keyword { get; set; }
    public string? Status { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
}
