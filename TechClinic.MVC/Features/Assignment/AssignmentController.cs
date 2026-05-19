using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TechClinic.Domain.Features.Assignment;
using TechClinic.Domain.Features.Assignment.Models;
using TechClinic.Domain.Features.Technician;
using TechClinic.Domain.Features.WorkOrder;

namespace TechClinic.MVC.Features.Assignment;

[Authorize]
public class AssignmentController : Controller
{
    private readonly IAssignmentService _assignmentService;
    private readonly ITechnicianService _technicianService;
    private readonly IWorkOrderService  _workOrderService;

    public AssignmentController(
        IAssignmentService assignmentService,
        ITechnicianService technicianService,
        IWorkOrderService  workOrderService)
    {
        _assignmentService = assignmentService;
        _technicianService = technicianService;
        _workOrderService  = workOrderService;
    }

    [HttpGet]
    public async Task<IActionResult> Assign(string workOrderId)
    {
        var woResult = await _workOrderService.GetByIdAsync(workOrderId);
        if (!woResult.Success) return NotFound();

        ViewData["Title"] = "Assign Technician";
        return View(await BuildAssignViewModel(workOrderId, woResult.Data!.WorkOrderCode, isReassign: false));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Assign(AssignViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await PopulateTechnicians(model);
            return View(model);
        }

        var result = await _assignmentService.AssignAsync(
            new SaveAssignmentModel { WorkOrderId = model.WorkOrderId, TechnicianId = model.TechnicianId },
            CurrentUserId);

        if (!result.Success) { ModelState.AddModelError(string.Empty, result.Message); await PopulateTechnicians(model); return View(model); }

        TempData["Success"] = "Technician assigned.";
        return RedirectToAction("Details", "WorkOrder", new { id = model.WorkOrderId });
    }

    [HttpGet]
    public async Task<IActionResult> Reassign(string workOrderId)
    {
        var woResult = await _workOrderService.GetByIdAsync(workOrderId);
        if (!woResult.Success) return NotFound();

        ViewData["Title"] = "Reassign Technician";
        return View("Assign", await BuildAssignViewModel(workOrderId, woResult.Data!.WorkOrderCode, isReassign: true));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Reassign(AssignViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.IsReassign = true;
            await PopulateTechnicians(model);
            return View("Assign", model);
        }

        var result = await _assignmentService.ReassignAsync(model.WorkOrderId, model.TechnicianId, CurrentUserId);

        if (!result.Success) { ModelState.AddModelError(string.Empty, result.Message); await PopulateTechnicians(model); return View("Assign", model); }

        TempData["Success"] = "Technician reassigned.";
        return RedirectToAction("Details", "WorkOrder", new { id = model.WorkOrderId });
    }

    private async Task<AssignViewModel> BuildAssignViewModel(string workOrderId, string workOrderCode, bool isReassign)
    {
        var vm = new AssignViewModel
        {
            WorkOrderId   = workOrderId,
            WorkOrderCode = workOrderCode,
            IsReassign    = isReassign
        };
        await PopulateTechnicians(vm);
        return vm;
    }

    private async Task PopulateTechnicians(AssignViewModel vm)
    {
        var techs = await _technicianService.GetListAsync();
        vm.Technicians = techs.Data?
            .Where(t => t.Status == TechClinic.Shared.Enums.TechnicianStatus.Active)
            .Select(t => new SelectListItem(t.FullName + " (" + t.TechnicianCode + ")", t.Id))
            .ToList() ?? new();
    }

    private string CurrentUserId =>
        User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "system";
}
