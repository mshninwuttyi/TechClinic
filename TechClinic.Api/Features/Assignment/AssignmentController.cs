using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechClinic.Domain.Features.Assignment;
using TechClinic.Domain.Features.Assignment.Models;

namespace TechClinic.Api.Features.Assignment;

[ApiController]
[Route("api/assignments")]
[Authorize]
public class AssignmentController : ControllerBase
{
    private readonly IAssignmentService _assignmentService;

    public AssignmentController(IAssignmentService assignmentService)
        => _assignmentService = assignmentService;

    [HttpGet("work-order/{workOrderId}")]
    public async Task<IActionResult> GetByWorkOrder(string workOrderId)
    {
        var result = await _assignmentService.GetByWorkOrderAsync(workOrderId);
        return result.Success ? Ok(result) : NotFound(result);
    }

    [HttpPost]
    public async Task<IActionResult> Assign([FromBody] SaveAssignmentModel model)
    {
        var result = await _assignmentService.AssignAsync(model, CurrentUserId);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPut("reassign/{workOrderId}")]
    public async Task<IActionResult> Reassign(string workOrderId, [FromQuery] string technicianId)
    {
        var result = await _assignmentService.ReassignAsync(workOrderId, technicianId, CurrentUserId);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    private string CurrentUserId =>
        User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "system";
}
