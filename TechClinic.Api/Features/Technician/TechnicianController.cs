using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechClinic.Domain.Features.Technician;
using TechClinic.Domain.Features.Technician.Models;

namespace TechClinic.Api.Features.Technician;

[ApiController]
[Route("api/technicians")]
[Authorize]
public class TechnicianController : ControllerBase
{
    private readonly ITechnicianService _technicianService;

    public TechnicianController(ITechnicianService technicianService)
        => _technicianService = technicianService;

    [HttpGet]
    public async Task<IActionResult> GetList()
    {
        var result = await _technicianService.GetListAsync();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var result = await _technicianService.GetByIdAsync(id);
        return result.Success ? Ok(result) : NotFound(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] SaveTechnicianModel model)
    {
        model.Id = null;
        var result = await _technicianService.SaveAsync(model, CurrentUserId);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] SaveTechnicianModel model)
    {
        model.Id = id;
        var result = await _technicianService.SaveAsync(model, CurrentUserId);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPatch("{id}/toggle-status")]
    public async Task<IActionResult> ToggleStatus(string id)
    {
        var result = await _technicianService.ToggleStatusAsync(id, CurrentUserId);
        return result.Success ? Ok(result) : NotFound(result);
    }

    private string CurrentUserId =>
        User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "system";
}
