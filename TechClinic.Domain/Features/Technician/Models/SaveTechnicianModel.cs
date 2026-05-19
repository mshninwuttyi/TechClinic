namespace TechClinic.Domain.Features.Technician.Models;

public class SaveTechnicianModel
{
    public string? Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? PhoneNo { get; set; }
    public string? Skill { get; set; }
}
