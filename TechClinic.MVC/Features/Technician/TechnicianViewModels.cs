using System.ComponentModel.DataAnnotations;

namespace TechClinic.MVC.Features.Technician;

public class TechnicianListViewModel
{
    public string Id { get; set; } = string.Empty;
    public string TechnicianCode { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string? PhoneNo { get; set; }
    public string? Skill { get; set; }
    public string? Status { get; set; }
}

public class SaveTechnicianViewModel
{
    public string? Id { get; set; }

    [Required, Display(Name = "Full Name")]
    public string FullName { get; set; } = string.Empty;

    [Required, Display(Name = "Username")]
    public string UserName { get; set; } = string.Empty;

    [Display(Name = "Password")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [EmailAddress]
    public string? Email { get; set; }

    [Display(Name = "Phone No")]
    public string? PhoneNo { get; set; }

    public string? Skill { get; set; }

    public bool IsEdit => !string.IsNullOrEmpty(Id);
}
