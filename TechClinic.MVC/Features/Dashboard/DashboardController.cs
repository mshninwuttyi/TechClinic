using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechClinic.Domain.Features.Dashboard;

namespace TechClinic.MVC.Features.Dashboard;

[Authorize]
public class DashboardController : Controller
{
    private readonly IDashboardService _dashboardService;

    public DashboardController(IDashboardService dashboardService)
        => _dashboardService = dashboardService;

    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "Dashboard";
        var result = await _dashboardService.GetSummaryAsync();
        var summary = result.Data!;

        return View(new DashboardViewModel
        {
            TotalOpenWorkOrders      = summary.TotalOpenWorkOrders,
            TotalCompletedWorkOrders = summary.TotalCompletedWorkOrders,
            WaitingPartsCount        = summary.WaitingPartsCount,
            RevenueSummary           = summary.RevenueSummary
        });
    }
}
