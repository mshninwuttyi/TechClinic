using TechClinic.Domain.Features.Assignment.Models;
using TechClinic.Shared.Models;

namespace TechClinic.Domain.Features.Assignment;

public interface IAssignmentService
{
    Task<BaseResponse<AssignmentModel>> GetByWorkOrderAsync(string workOrderId);
    Task<BaseResponse<string>> AssignAsync(SaveAssignmentModel model, string currentUserId);
    Task<BaseResponse<string>> ReassignAsync(string workOrderId, string technicianId, string currentUserId);
}
