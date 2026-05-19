using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TechClinic.MVC.Features.Invoice;

public class InvoiceListViewModel
{
    public string Id { get; set; } = string.Empty;
    public string InvoiceCode { get; set; } = string.Empty;
    public string WorkOrderCode { get; set; } = string.Empty;
    public decimal? GrandTotal { get; set; }
    public string? PaymentStatus { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class InvoiceDetailsViewModel
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

public class GenerateInvoiceViewModel
{
    [Required]
    public string WorkOrderId { get; set; } = string.Empty;

    public string WorkOrderCode { get; set; } = string.Empty;

    [Required, Range(0, double.MaxValue, ErrorMessage = "Labor fee must be 0 or more.")]
    [Display(Name = "Labor Fee")]
    public decimal LaborFee { get; set; }
}
