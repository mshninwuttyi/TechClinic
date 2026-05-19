using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TechClinic.Domain.Features.Assignment;
using TechClinic.Domain.Features.WorkOrder;
using TechClinic.Domain.Features.WorkOrder.Models;
using TechClinic.Shared.Enums;

namespace TechClinic.MVC.Features.WorkOrder;

[Authorize]
public class WorkOrderController : Controller
{
    private readonly IWorkOrderService  _workOrderService;
    private readonly IAssignmentService _assignmentService;

    public WorkOrderController(IWorkOrderService workOrderService, IAssignmentService assignmentService)
    {
        _workOrderService  = workOrderService;
        _assignmentService = assignmentService;
    }

    public async Task<IActionResult> Index(WorkOrderFilterViewModel filter)
    {
        ViewData["Title"] = "Work Orders";
        filter.StatusOptions = GetStatusOptions(filter.Status);

        var result = await _workOrderService.GetListAsync(new WorkOrderFilterModel
        {
            Keyword  = filter.Keyword,
            Status   = filter.Status,
            FromDate = filter.FromDate,
            ToDate   = filter.ToDate
        });

        ViewBag.Filter = filter;
        var list = result.Data!.Select(w => new WorkOrderListViewModel
        {
            Id            = w.Id,
            WorkOrderCode = w.WorkOrderCode,
            DeviceType    = w.DeviceType,
            Brand         = w.Brand,
            Status        = w.Status,
            IntakeDate    = w.IntakeDate,
            EstimatedCost = w.EstimatedCost
        }).ToList();

        return View(list);
    }

    [HttpGet]
    public IActionResult Create()
    {
        ViewData["Title"] = "Create Work Order";
        return View("Save", new SaveWorkOrderViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(SaveWorkOrderViewModel model)
    {
        if (!ModelState.IsValid) { ViewData["Title"] = "Create Work Order"; return View("Save", model); }

        var result = await _workOrderService.SaveAsync(ToSaveModel(model), CurrentUserId);
        if (!result.Success) { ModelState.AddModelError(string.Empty, result.Message); return View("Save", model); }

        TempData["Success"] = "Work order created.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(string id)
    {
        ViewData["Title"] = "Edit Work Order";
        var result = await _workOrderService.GetByIdAsync(id);
        if (!result.Success) return NotFound();

        var w = result.Data!;
        return View("Save", new SaveWorkOrderViewModel
        {
            Id                  = w.Id,
            DeviceType          = w.DeviceType,
            Brand               = w.Brand,
            DeviceModel         = w.Model,
            SerialNumber        = w.SerialNumber,
            ProblemDescription  = w.ProblemDescription,
            IntakeDate          = w.IntakeDate,
            EstimatedReturnDate = w.EstimatedReturnDate,
            EstimatedCost       = w.EstimatedCost
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(SaveWorkOrderViewModel model)
    {
        if (!ModelState.IsValid) { ViewData["Title"] = "Edit Work Order"; return View("Save", model); }

        var result = await _workOrderService.SaveAsync(ToSaveModel(model), CurrentUserId);
        if (!result.Success) { ModelState.AddModelError(string.Empty, result.Message); return View("Save", model); }

        TempData["Success"] = "Work order updated.";
        return RedirectToAction(nameof(Details), new { id = model.Id });
    }

    public async Task<IActionResult> Details(string id)
    {
        ViewData["Title"] = "Work Order Details";
        var result = await _workOrderService.GetByIdAsync(id);
        if (!result.Success) return NotFound();

        var w = result.Data!;
        var vm = new WorkOrderDetailsViewModel
        {
            Id                  = w.Id,
            WorkOrderCode       = w.WorkOrderCode,
            DeviceType          = w.DeviceType,
            Brand               = w.Brand,
            DeviceModel         = w.Model,
            SerialNumber        = w.SerialNumber,
            ProblemDescription  = w.ProblemDescription,
            IntakeDate          = w.IntakeDate,
            EstimatedReturnDate = w.EstimatedReturnDate,
            EstimatedCost       = w.EstimatedCost,
            Status              = w.Status,
            StatusOptions       = GetStatusOptions(w.Status)
        };

        var assignment = await _assignmentService.GetByWorkOrderAsync(id);
        if (assignment.Success && assignment.Data is not null)
        {
            vm.AssignedTechnicianName = assignment.Data.TechnicianName;
            vm.AssignedDate           = assignment.Data.AssignedDate;
        }

        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateStatus(string id, string status)
    {
        await _workOrderService.UpdateStatusAsync(id, status, CurrentUserId);
        TempData["Success"] = $"Status updated to {status}.";
        return RedirectToAction(nameof(Details), new { id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(string id)
    {
        await _workOrderService.DeleteAsync(id, CurrentUserId);
        TempData["Success"] = "Work order deleted.";
        return RedirectToAction(nameof(Index));
    }

    private static SaveWorkOrderModel ToSaveModel(SaveWorkOrderViewModel vm) => new()
    {
        Id                  = vm.Id,
        DeviceType          = vm.DeviceType,
        Brand               = vm.Brand,
        Model               = vm.DeviceModel,
        SerialNumber        = vm.SerialNumber,
        ProblemDescription  = vm.ProblemDescription,
        IntakeDate          = vm.IntakeDate,
        EstimatedReturnDate = vm.EstimatedReturnDate,
        EstimatedCost       = vm.EstimatedCost
    };

    private static List<SelectListItem> GetStatusOptions(string? selected) => new()
    {
        new SelectListItem("Open",          WorkOrderStatus.Open,         selected == WorkOrderStatus.Open),
        new SelectListItem("In Progress",   WorkOrderStatus.InProgress,   selected == WorkOrderStatus.InProgress),
        new SelectListItem("Waiting Parts", WorkOrderStatus.WaitingParts, selected == WorkOrderStatus.WaitingParts),
        new SelectListItem("Completed",     WorkOrderStatus.Completed,    selected == WorkOrderStatus.Completed),
        new SelectListItem("Cancelled",     WorkOrderStatus.Cancelled,    selected == WorkOrderStatus.Cancelled)
    };

    private string CurrentUserId =>
        User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "system";
}
