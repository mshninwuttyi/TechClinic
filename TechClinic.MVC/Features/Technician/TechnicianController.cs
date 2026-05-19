using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechClinic.Domain.Features.Technician;
using TechClinic.Domain.Features.Technician.Models;

namespace TechClinic.MVC.Features.Technician;

[Authorize]
public class TechnicianController : Controller
{
    private readonly ITechnicianService _technicianService;

    public TechnicianController(ITechnicianService technicianService)
        => _technicianService = technicianService;

    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "Technician Management";
        var result = await _technicianService.GetListAsync();
        var list = result.Data!.Select(t => new TechnicianListViewModel
        {
            Id            = t.Id,
            TechnicianCode = t.TechnicianCode,
            FullName      = t.FullName,
            UserName      = t.UserName,
            PhoneNo       = t.PhoneNo,
            Skill         = t.Skill,
            Status        = t.Status
        }).ToList();

        return View(list);
    }

    [HttpGet]
    public IActionResult Create()
    {
        ViewData["Title"] = "Add Technician";
        return View("Save", new SaveTechnicianViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(SaveTechnicianViewModel model)
    {
        if (!ModelState.IsValid) { ViewData["Title"] = "Add Technician"; return View("Save", model); }

        var result = await _technicianService.SaveAsync(new SaveTechnicianModel
        {
            FullName = model.FullName,
            UserName = model.UserName,
            Password = model.Password,
            Email    = model.Email,
            PhoneNo  = model.PhoneNo,
            Skill    = model.Skill
        }, CurrentUserId);

        if (!result.Success) { ModelState.AddModelError(string.Empty, result.Message); return View("Save", model); }

        TempData["Success"] = "Technician added successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(string id)
    {
        ViewData["Title"] = "Edit Technician";
        var result = await _technicianService.GetByIdAsync(id);
        if (!result.Success) return NotFound();

        var t = result.Data!;
        return View("Save", new SaveTechnicianViewModel
        {
            Id      = t.Id,
            FullName = t.FullName,
            UserName = t.UserName,
            PhoneNo  = t.PhoneNo,
            Skill    = t.Skill
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(SaveTechnicianViewModel model)
    {
        if (!ModelState.IsValid) { ViewData["Title"] = "Edit Technician"; return View("Save", model); }

        var result = await _technicianService.SaveAsync(new SaveTechnicianModel
        {
            Id      = model.Id,
            FullName = model.FullName,
            UserName = model.UserName,
            Password = model.Password,
            Email    = model.Email,
            PhoneNo  = model.PhoneNo,
            Skill    = model.Skill
        }, CurrentUserId);

        if (!result.Success) { ModelState.AddModelError(string.Empty, result.Message); return View("Save", model); }

        TempData["Success"] = "Technician updated successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleStatus(string id)
    {
        await _technicianService.ToggleStatusAsync(id, CurrentUserId);
        return RedirectToAction(nameof(Index));
    }

    private string CurrentUserId =>
        User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "system";
}
