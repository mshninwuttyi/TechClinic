namespace TechClinic.MVC.Features.Dashboard;

public class DashboardViewModel
{
    public int TotalOpenWorkOrders { get; set; }
    public int TotalCompletedWorkOrders { get; set; }
    public int WaitingPartsCount { get; set; }
    public decimal RevenueSummary { get; set; }
}
