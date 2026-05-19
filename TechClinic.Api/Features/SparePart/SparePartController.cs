using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechClinic.Domain.Features.SparePart;
using TechClinic.Domain.Features.SparePart.Models;

namespace TechClinic.Api.Features.SparePart;

[ApiController]
[Route("api/spare-parts")]
[Authorize]
public class SparePartController : ControllerBase
{
    private readonly ISparePartService _sparePartService;

    public SparePartController(ISparePartService sparePartService)
        => _sparePartService = sparePartService;

    [HttpGet]
    public async Task<IActionResult> GetList()
    {
        var result = await _sparePartService.GetListAsync();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var result = await _sparePartService.GetByIdAsync(id);
        return result.Success ? Ok(result) : NotFound(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] SaveSparePartModel model)
    {
        model.Id = null;
        var result = await _sparePartService.SaveAsync(model, CurrentUserId);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] SaveSparePartModel model)
    {
        model.Id = id;
        var result = await _sparePartService.SaveAsync(model, CurrentUserId);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPatch("{id}/stock")]
    public async Task<IActionResult> UpdateStock(string id, [FromQuery] int quantity)
    {
        var result = await _sparePartService.UpdateStockAsync(id, quantity, CurrentUserId);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    private string CurrentUserId =>
        User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "system";
}
