using System.ComponentModel.DataAnnotations;

namespace TechClinic.MVC.Features.SparePart;

public class SparePartListViewModel
{
    public string Id { get; set; } = string.Empty;
    public string PartCode { get; set; } = string.Empty;
    public string PartName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal? UnitPrice { get; set; }
}

public class SaveSparePartViewModel
{
    public string? Id { get; set; }

    [Required, Display(Name = "Part Name")]
    public string PartName { get; set; } = string.Empty;

    [Required, Range(0, int.MaxValue)]
    public int Quantity { get; set; }

    [Display(Name = "Unit Price")]
    public decimal? UnitPrice { get; set; }

    public bool IsEdit => !string.IsNullOrEmpty(Id);
}

public class UpdateStockViewModel
{
    [Required]
    public string Id { get; set; } = string.Empty;

    public string PartName { get; set; } = string.Empty;

    [Required, Range(0, int.MaxValue, ErrorMessage = "Quantity must be 0 or more.")]
    public int Quantity { get; set; }
}
