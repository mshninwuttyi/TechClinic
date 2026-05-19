namespace TechClinic.Domain.Features.Technician.Models;

public class TechnicianModel
{
    public string Id { get; set; } = string.Empty;
    public string TechnicianCode { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string? PhoneNo { get; set; }
    public string? Skill { get; set; }
    public string? Status { get; set; }
}
