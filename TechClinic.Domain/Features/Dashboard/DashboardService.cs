using Microsoft.EntityFrameworkCore;
using TechClinic.Database.AppDbContextModels;
using TechClinic.Domain.Features.Dashboard.Models;
using TechClinic.Shared.Enums;
using TechClinic.Shared.Models;

namespace TechClinic.Domain.Features.Dashboard;

public class DashboardService : IDashboardService
{
    private readonly AppDbContext _db;

    public DashboardService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<DashboardSummaryModel>> GetSummaryAsync()
    {
        var openCount = await _db.TblWorkOrders
            .CountAsync(w => w.Status == WorkOrderStatus.Open && !w.DeleteFlag);

        var completedCount = await _db.TblWorkOrders
            .CountAsync(w => w.Status == WorkOrderStatus.Completed && !w.DeleteFlag);

        var waitingCount = await _db.TblWorkOrders
            .CountAsync(w => w.Status == WorkOrderStatus.WaitingParts && !w.DeleteFlag);

        var revenue = await _db.TblInvoices
            .Where(i => i.PaymentStatus == PaymentStatus.Paid && !i.DeleteFlag)
            .SumAsync(i => i.GrandTotal ?? 0m);

        return BaseResponse<DashboardSummaryModel>.Ok(new DashboardSummaryModel
        {
            TotalOpenWorkOrders      = openCount,
            TotalCompletedWorkOrders = completedCount,
            WaitingPartsCount        = waitingCount,
            RevenueSummary           = revenue
        });
    }
}
