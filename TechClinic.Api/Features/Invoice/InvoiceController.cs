using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechClinic.Domain.Features.Invoice;
using TechClinic.Domain.Features.Invoice.Models;
using TechClinic.Shared.Enums;

namespace TechClinic.Api.Features.Invoice;

[ApiController]
[Route("api/invoices")]
[Authorize]
public class InvoiceController : ControllerBase
{
    private readonly IInvoiceService _invoiceService;

    public InvoiceController(IInvoiceService invoiceService)
        => _invoiceService = invoiceService;

    [HttpGet]
    public async Task<IActionResult> GetList()
    {
        var result = await _invoiceService.GetListAsync();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var result = await _invoiceService.GetByIdAsync(id);
        return result.Success ? Ok(result) : NotFound(result);
    }

    [HttpGet("work-order/{workOrderId}")]
    public async Task<IActionResult> GetByWorkOrder(string workOrderId)
    {
        var result = await _invoiceService.GetByWorkOrderAsync(workOrderId);
        return result.Success ? Ok(result) : NotFound(result);
    }

    [HttpPost("generate")]
    public async Task<IActionResult> Generate([FromBody] GenerateInvoiceModel model)
    {
        var result = await _invoiceService.GenerateAsync(model, CurrentUserId);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPatch("{id}/mark-paid")]
    public async Task<IActionResult> MarkPaid(string id)
    {
        var result = await _invoiceService.UpdatePaymentStatusAsync(id, PaymentStatus.Paid, CurrentUserId);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPatch("{id}/mark-unpaid")]
    public async Task<IActionResult> MarkUnpaid(string id)
    {
        var result = await _invoiceService.UpdatePaymentStatusAsync(id, PaymentStatus.Unpaid, CurrentUserId);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    private string CurrentUserId =>
        User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "system";
}
