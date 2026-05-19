namespace TechClinic.Domain.Features.Assignment.Models;

public class AssignmentModel
{
    public string Id { get; set; } = string.Empty;
    public string AssignmentCode { get; set; } = string.Empty;
    public string WorkOrderId { get; set; } = string.Empty;
    public string WorkOrderCode { get; set; } = string.Empty;
    public string TechnicianId { get; set; } = string.Empty;
    public string TechnicianName { get; set; } = string.Empty;
    public DateTime AssignedDate { get; set; }
}
