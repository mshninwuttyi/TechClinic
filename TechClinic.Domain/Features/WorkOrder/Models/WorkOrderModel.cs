namespace TechClinic.Domain.Features.WorkOrder.Models;

public class WorkOrderModel
{
    public string Id { get; set; } = string.Empty;
    public string WorkOrderCode { get; set; } = string.Empty;
    public string? DeviceType { get; set; }
    public string? Brand { get; set; }
    public string? Model { get; set; }
    public string? SerialNumber { get; set; }
    public string? ProblemDescription { get; set; }
    public DateTime IntakeDate { get; set; }
    public DateTime? EstimatedReturnDate { get; set; }
    public decimal? EstimatedCost { get; set; }
    public string? Status { get; set; }
}
