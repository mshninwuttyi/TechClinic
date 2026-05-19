using TechClinic.Domain.Features.Invoice.Models;
using TechClinic.Shared.Models;

namespace TechClinic.Domain.Features.Invoice;

public interface IInvoiceService
{
    Task<BaseResponse<List<InvoiceModel>>> GetListAsync();
    Task<BaseResponse<InvoiceModel>> GetByIdAsync(string id);
    Task<BaseResponse<InvoiceModel>> GetByWorkOrderAsync(string workOrderId);
    Task<BaseResponse<string>> GenerateAsync(GenerateInvoiceModel model, string currentUserId);
    Task<BaseResponse<string>> UpdatePaymentStatusAsync(string id, string paymentStatus, string currentUserId);
}
