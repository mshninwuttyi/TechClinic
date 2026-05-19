namespace TechClinic.Domain.Features.SparePart.Models;

public class SparePartModel
{
    public string Id { get; set; } = string.Empty;
    public string PartCode { get; set; } = string.Empty;
    public string PartName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal? UnitPrice { get; set; }
}
