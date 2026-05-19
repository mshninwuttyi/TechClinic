namespace TechClinic.Domain.Features.Dashboard.Models;

public class DashboardSummaryModel
{
    public int TotalOpenWorkOrders { get; set; }
    public int TotalCompletedWorkOrders { get; set; }
    public int WaitingPartsCount { get; set; }
    public decimal RevenueSummary { get; set; }
}
