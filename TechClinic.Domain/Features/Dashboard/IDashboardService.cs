using TechClinic.Domain.Features.Dashboard.Models;
using TechClinic.Shared.Models;

namespace TechClinic.Domain.Features.Dashboard;

public interface IDashboardService
{
    Task<BaseResponse<DashboardSummaryModel>> GetSummaryAsync();
}
