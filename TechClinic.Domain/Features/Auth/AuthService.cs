using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TechClinic.Database.AppDbContextModels;
using TechClinic.Domain.Features.Auth.Models;
using TechClinic.Shared.Models;

namespace TechClinic.Domain.Features.Auth;

public class AuthService : IAuthService
{
    private readonly AppDbContext _db;

    public AuthService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<LoginResponseModel>> LoginAsync(LoginRequestModel request)
    {
        var hash = HashPassword(request.Password);
        var user = await _db.TblUsers
            .FirstOrDefaultAsync(u => u.UserName == request.UserName
                                   && u.PasswordHash == hash
                                   && !u.DeleteFlag);

        if (user is null)
            return BaseResponse<LoginResponseModel>.Fail("Invalid username or password.");

        return BaseResponse<LoginResponseModel>.Ok(new LoginResponseModel
        {
            UserId   = user.Id,
            UserName = user.UserName,
            FullName = user.FullName,
            RoleName = user.RoleName
        });
    }

    // SHA-256 hash — suitable for this learning project only.
    // Replace with BCrypt in a production system.
    public static string HashPassword(string password)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bytes);
    }
}
