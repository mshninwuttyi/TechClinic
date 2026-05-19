namespace TechClinic.Domain.Features.WorkOrder.Models;

public class SaveWorkOrderModel
{
    public string? Id { get; set; }
    public string? DeviceType { get; set; }
    public string? Brand { get; set; }
    public string? Model { get; set; }
    public string? SerialNumber { get; set; }
    public string? ProblemDescription { get; set; }
    public DateTime IntakeDate { get; set; }
    public DateTime? EstimatedReturnDate { get; set; }
    public decimal? EstimatedCost { get; set; }
}
