using TechClinic.Domain.Features.SparePart.Models;
using TechClinic.Shared.Models;

namespace TechClinic.Domain.Features.SparePart;

public interface ISparePartService
{
    Task<BaseResponse<List<SparePartModel>>> GetListAsync();
    Task<BaseResponse<SparePartModel>> GetByIdAsync(string id);
    Task<BaseResponse<string>> SaveAsync(SaveSparePartModel model, string currentUserId);
    Task<BaseResponse<string>> UpdateStockAsync(string id, int quantity, string currentUserId);
}
