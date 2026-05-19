namespace TechClinic.Domain.Features.SparePart.Models;

public class SaveSparePartModel
{
    public string? Id { get; set; }
    public string PartName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal? UnitPrice { get; set; }
}
