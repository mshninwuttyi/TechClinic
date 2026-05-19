using Microsoft.EntityFrameworkCore;
using TechClinic.Database.AppDbContextModels;
using TechClinic.Domain.Features.Invoice.Models;
using TechClinic.Shared.Enums;
using TechClinic.Shared.Models;

namespace TechClinic.Domain.Features.Invoice;

public class InvoiceService : IInvoiceService
{
    private readonly AppDbContext _db;

    public InvoiceService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<List<InvoiceModel>>> GetListAsync()
    {
        var list = await (
            from i  in _db.TblInvoices
            join wo in _db.TblWorkOrders on i.WorkOrderId equals wo.Id
            where !i.DeleteFlag
            orderby i.CreatedAt descending
            select new InvoiceModel
            {
                Id            = i.Id,
                InvoiceCode   = i.InvoiceCode,
                WorkOrderId   = i.WorkOrderId,
                WorkOrderCode = wo.WorkOrderCode,
                LaborFee      = i.LaborFee,
                PartsTotal    = i.PartsTotal,
                GrandTotal    = i.GrandTotal,
                PaymentStatus = i.PaymentStatus,
                CreatedAt     = i.CreatedAt
            }
        ).ToListAsync();

        return BaseResponse<List<InvoiceModel>>.Ok(list);
    }

    public async Task<BaseResponse<InvoiceModel>> GetByIdAsync(string id)
    {
        var item = await (
            from i  in _db.TblInvoices
            join wo in _db.TblWorkOrders on i.WorkOrderId equals wo.Id
            where i.Id == id && !i.DeleteFlag
            select new InvoiceModel
            {
                Id            = i.Id,
                InvoiceCode   = i.InvoiceCode,
                WorkOrderId   = i.WorkOrderId,
                WorkOrderCode = wo.WorkOrderCode,
                LaborFee      = i.LaborFee,
                PartsTotal    = i.PartsTotal,
                GrandTotal    = i.GrandTotal,
                PaymentStatus = i.PaymentStatus,
                CreatedAt     = i.CreatedAt
            }
        ).FirstOrDefaultAsync();

        if (item is null) return BaseResponse<InvoiceModel>.Fail("Invoice not found.");
        return BaseResponse<InvoiceModel>.Ok(item);
    }

    public async Task<BaseResponse<InvoiceModel>> GetByWorkOrderAsync(string workOrderId)
    {
        var item = await (
            from i  in _db.TblInvoices
            join wo in _db.TblWorkOrders on i.WorkOrderId equals wo.Id
            where i.WorkOrderId == workOrderId && !i.DeleteFlag
            select new InvoiceModel
            {
                Id            = i.Id,
                InvoiceCode   = i.InvoiceCode,
                WorkOrderId   = i.WorkOrderId,
                WorkOrderCode = wo.WorkOrderCode,
                LaborFee      = i.LaborFee,
                PartsTotal    = i.PartsTotal,
                GrandTotal    = i.GrandTotal,
                PaymentStatus = i.PaymentStatus,
                CreatedAt     = i.CreatedAt
            }
        ).FirstOrDefaultAsync();

        if (item is null) return BaseResponse<InvoiceModel>.Fail("Invoice not found for this work order.");
        return BaseResponse<InvoiceModel>.Ok(item);
    }

    public async Task<BaseResponse<string>> GenerateAsync(GenerateInvoiceModel model, string currentUserId)
    {
        var existing = await _db.TblInvoices
            .FirstOrDefaultAsync(i => i.WorkOrderId == model.WorkOrderId && !i.DeleteFlag);

        if (existing is not null)
            return BaseResponse<string>.Fail("An invoice already exists for this work order.");

        var partsTotal = await _db.TblWorkOrderParts
            .Where(p => p.WorkOrderId == model.WorkOrderId)
            .SumAsync(p => p.UnitPrice * p.Quantity);

        var grandTotal = model.LaborFee + partsTotal;

        var invoice = new TblInvoice
        {
            Id            = NewId(),
            InvoiceCode   = "INV-" + DateTime.Now.ToString("yyMMddHHmmss"),
            WorkOrderId   = model.WorkOrderId,
            LaborFee      = model.LaborFee,
            PartsTotal    = partsTotal,
            GrandTotal    = grandTotal,
            PaymentStatus = PaymentStatus.Unpaid,
            CreatedAt     = DateTime.Now,
            CreatedBy     = currentUserId
        };

        _db.TblInvoices.Add(invoice);
        await _db.SaveChangesAsync();
        return BaseResponse<string>.Ok("Invoice generated.");
    }

    public async Task<BaseResponse<string>> UpdatePaymentStatusAsync(string id, string paymentStatus, string currentUserId)
    {
        var invoice = await _db.TblInvoices.FirstOrDefaultAsync(i => i.Id == id && !i.DeleteFlag);
        if (invoice is null) return BaseResponse<string>.Fail("Invoice not found.");

        invoice.PaymentStatus = paymentStatus;
        invoice.ModifiedAt    = DateTime.Now;
        invoice.ModifiedBy    = currentUserId;

        await _db.SaveChangesAsync();
        return BaseResponse<string>.Ok($"Marked as {paymentStatus}.");
    }

    private static string NewId() => Guid.NewGuid().ToString("N")[..26];
}
