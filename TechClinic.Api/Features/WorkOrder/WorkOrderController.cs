using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechClinic.Domain.Features.WorkOrder;
using TechClinic.Domain.Features.WorkOrder.Models;

namespace TechClinic.Api.Features.WorkOrder;

[ApiController]
[Route("api/work-orders")]
[Authorize]
public class WorkOrderController : ControllerBase
{
    private readonly IWorkOrderService _workOrderService;

    public WorkOrderController(IWorkOrderService workOrderService)
        => _workOrderService = workOrderService;

    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] WorkOrderFilterModel filter)
    {
        var result = await _workOrderService.GetListAsync(filter);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var result = await _workOrderService.GetByIdAsync(id);
        return result.Success ? Ok(result) : NotFound(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] SaveWorkOrderModel model)
    {
        model.Id = null;
        var result = await _workOrderService.SaveAsync(model, CurrentUserId);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] SaveWorkOrderModel model)
    {
        model.Id = id;
        var result = await _workOrderService.SaveAsync(model, CurrentUserId);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPatch("{id}/status")]
    public async Task<IActionResult> UpdateStatus(string id, [FromQuery] string status)
    {
        var result = await _workOrderService.UpdateStatusAsync(id, status, CurrentUserId);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var result = await _workOrderService.DeleteAsync(id, CurrentUserId);
        return result.Success ? Ok(result) : NotFound(result);
    }

    private string CurrentUserId =>
        User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "system";
}
