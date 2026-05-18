using System;
using System.Collections.Generic;

namespace TechClinic.Database.AppDbContextModels;

public partial class TblInvoice
{
    public string Id { get; set; } = null!;

    public string InvoiceCode { get; set; } = null!;

    public string WorkOrderId { get; set; } = null!;

    public decimal? LaborFee { get; set; }

    public decimal? PartsTotal { get; set; }

    public decimal? GrandTotal { get; set; }

    public string? PaymentStatus { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public string? ModifiedBy { get; set; }

    public bool DeleteFlag { get; set; }
}
