using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechClinic.Domain.Features.SparePart;
using TechClinic.Domain.Features.SparePart.Models;

namespace TechClinic.MVC.Features.SparePart;

[Authorize]
public class SparePartController : Controller
{
    private readonly ISparePartService _sparePartService;

    public SparePartController(ISparePartService sparePartService)
        => _sparePartService = sparePartService;

    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "Spare Parts";
        var result = await _sparePartService.GetListAsync();
        var list = result.Data!.Select(s => new SparePartListViewModel
        {
            Id        = s.Id,
            PartCode  = s.PartCode,
            PartName  = s.PartName,
            Quantity  = s.Quantity,
            UnitPrice = s.UnitPrice
        }).ToList();

        return View(list);
    }

    [HttpGet]
    public IActionResult Create()
    {
        ViewData["Title"] = "Add Spare Part";
        return View("Save", new SaveSparePartViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(SaveSparePartViewModel model)
    {
        if (!ModelState.IsValid) { ViewData["Title"] = "Add Spare Part"; return View("Save", model); }

        var result = await _sparePartService.SaveAsync(new SaveSparePartModel
        {
            PartName  = model.PartName,
            Quantity  = model.Quantity,
            UnitPrice = model.UnitPrice
        }, CurrentUserId);

        if (!result.Success) { ModelState.AddModelError(string.Empty, result.Message); return View("Save", model); }

        TempData["Success"] = "Spare part added.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(string id)
    {
        ViewData["Title"] = "Edit Spare Part";
        var result = await _sparePartService.GetByIdAsync(id);
        if (!result.Success) return NotFound();

        var s = result.Data!;
        return View("Save", new SaveSparePartViewModel
        {
            Id        = s.Id,
            PartName  = s.PartName,
            Quantity  = s.Quantity,
            UnitPrice = s.UnitPrice
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(SaveSparePartViewModel model)
    {
        if (!ModelState.IsValid) { ViewData["Title"] = "Edit Spare Part"; return View("Save", model); }

        var result = await _sparePartService.SaveAsync(new SaveSparePartModel
        {
            Id        = model.Id,
            PartName  = model.PartName,
            Quantity  = model.Quantity,
            UnitPrice = model.UnitPrice
        }, CurrentUserId);

        if (!result.Success) { ModelState.AddModelError(string.Empty, result.Message); return View("Save", model); }

        TempData["Success"] = "Spare part updated.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> UpdateStock(string id)
    {
        ViewData["Title"] = "Update Stock";
        var result = await _sparePartService.GetByIdAsync(id);
        if (!result.Success) return NotFound();

        return View(new UpdateStockViewModel { Id = id, PartName = result.Data!.PartName, Quantity = result.Data.Quantity });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateStock(UpdateStockViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var result = await _sparePartService.UpdateStockAsync(model.Id, model.Quantity, CurrentUserId);
        if (!result.Success) { ModelState.AddModelError(string.Empty, result.Message); return View(model); }

        TempData["Success"] = "Stock updated.";
        return RedirectToAction(nameof(Index));
    }

    private string CurrentUserId =>
        User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "system";
}
