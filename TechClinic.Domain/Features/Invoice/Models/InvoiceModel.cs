namespace TechClinic.Domain.Features.Invoice.Models;

public class InvoiceModel
{
    public string Id { get; set; } = string.Empty;
    public string InvoiceCode { get; set; } = string.Empty;
    public string WorkOrderId { get; set; } = string.Empty;
    public string WorkOrderCode { get; set; } = string.Empty;
    public decimal? LaborFee { get; set; }
    public decimal? PartsTotal { get; set; }
    public decimal? GrandTotal { get; set; }
    public string? PaymentStatus { get; set; }
    public DateTime CreatedAt { get; set; }
}
