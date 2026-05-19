using TechClinic.Domain.Features.Technician.Models;
using TechClinic.Shared.Models;

namespace TechClinic.Domain.Features.Technician;

public interface ITechnicianService
{
    Task<BaseResponse<List<TechnicianModel>>> GetListAsync();
    Task<BaseResponse<TechnicianModel>> GetByIdAsync(string id);
    Task<BaseResponse<string>> SaveAsync(SaveTechnicianModel model, string currentUserId);
    Task<BaseResponse<string>> ToggleStatusAsync(string id, string currentUserId);
}
