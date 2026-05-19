using TechClinic.Domain.Features.Auth.Models;
using TechClinic.Shared.Models;

namespace TechClinic.Domain.Features.Auth;

public interface IAuthService
{
    Task<BaseResponse<LoginResponseModel>> LoginAsync(LoginRequestModel request);
}
