namespace TechClinic.Domain.Features.Invoice.Models;

public class GenerateInvoiceModel
{
    public string WorkOrderId { get; set; } = string.Empty;
    public decimal LaborFee { get; set; }
}
