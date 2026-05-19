using TechClinic.Domain.Features.WorkOrder.Models;
using TechClinic.Shared.Models;

namespace TechClinic.Domain.Features.WorkOrder;

public interface IWorkOrderService
{
    Task<BaseResponse<List<WorkOrderModel>>> GetListAsync(WorkOrderFilterModel filter);
    Task<BaseResponse<WorkOrderModel>> GetByIdAsync(string id);
    Task<BaseResponse<string>> SaveAsync(SaveWorkOrderModel model, string currentUserId);
    Task<BaseResponse<string>> UpdateStatusAsync(string id, string status, string currentUserId);
    Task<BaseResponse<string>> DeleteAsync(string id, string currentUserId);
}
