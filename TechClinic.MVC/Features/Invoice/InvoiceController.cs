using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechClinic.Domain.Features.Invoice;
using TechClinic.Domain.Features.Invoice.Models;
using TechClinic.Domain.Features.WorkOrder;
using TechClinic.Shared.Enums;

namespace TechClinic.MVC.Features.Invoice;

[Authorize]
public class InvoiceController : Controller
{
    private readonly IInvoiceService   _invoiceService;
    private readonly IWorkOrderService _workOrderService;

    public InvoiceController(IInvoiceService invoiceService, IWorkOrderService workOrderService)
    {
        _invoiceService   = invoiceService;
        _workOrderService = workOrderService;
    }

    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "Invoices";
        var result = await _invoiceService.GetListAsync();
        var list = result.Data!.Select(i => new InvoiceListViewModel
        {
            Id            = i.Id,
            InvoiceCode   = i.InvoiceCode,
            WorkOrderCode = i.WorkOrderCode,
            GrandTotal    = i.GrandTotal,
            PaymentStatus = i.PaymentStatus,
            CreatedAt     = i.CreatedAt
        }).ToList();

        return View(list);
    }

    public async Task<IActionResult> Details(string id)
    {
        ViewData["Title"] = "Invoice Details";
        var result = await _invoiceService.GetByIdAsync(id);
        if (!result.Success) return NotFound();

        var i = result.Data!;
        return View(new InvoiceDetailsViewModel
        {
            Id            = i.Id,
            InvoiceCode   = i.InvoiceCode,
            WorkOrderId   = i.WorkOrderId,
            WorkOrderCode = i.WorkOrderCode,
            LaborFee      = i.LaborFee,
            PartsTotal    = i.PartsTotal,
            GrandTotal    = i.GrandTotal,
            PaymentStatus = i.PaymentStatus,
            CreatedAt     = i.CreatedAt
        });
    }

    [HttpGet]
    public async Task<IActionResult> Generate(string workOrderId)
    {
        ViewData["Title"] = "Generate Invoice";
        var wo = await _workOrderService.GetByIdAsync(workOrderId);
        if (!wo.Success) return NotFound();

        return View(new GenerateInvoiceViewModel
        {
            WorkOrderId   = workOrderId,
            WorkOrderCode = wo.Data!.WorkOrderCode
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Generate(GenerateInvoiceViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var result = await _invoiceService.GenerateAsync(new GenerateInvoiceModel
        {
            WorkOrderId = model.WorkOrderId,
            LaborFee    = model.LaborFee
        }, CurrentUserId);

        if (!result.Success) { ModelState.AddModelError(string.Empty, result.Message); return View(model); }

        TempData["Success"] = "Invoice generated.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MarkPaid(string id)
    {
        await _invoiceService.UpdatePaymentStatusAsync(id, PaymentStatus.Paid, CurrentUserId);
        TempData["Success"] = "Invoice marked as Paid.";
        return RedirectToAction(nameof(Details), new { id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MarkUnpaid(string id)
    {
        await _invoiceService.UpdatePaymentStatusAsync(id, PaymentStatus.Unpaid, CurrentUserId);
        TempData["Success"] = "Invoice marked as Unpaid.";
        return RedirectToAction(nameof(Details), new { id });
    }

    private string CurrentUserId =>
        User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "system";
}
