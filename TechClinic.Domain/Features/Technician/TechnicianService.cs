using Microsoft.EntityFrameworkCore;
using TechClinic.Database.AppDbContextModels;
using TechClinic.Domain.Features.Auth;
using TechClinic.Domain.Features.Technician.Models;
using TechClinic.Shared.Constants;
using TechClinic.Shared.Enums;
using TechClinic.Shared.Models;

namespace TechClinic.Domain.Features.Technician;

public class TechnicianService : ITechnicianService
{
    private readonly AppDbContext _db;

    public TechnicianService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<List<TechnicianModel>>> GetListAsync()
    {
        var list = await (
            from t in _db.TblTechnicians
            join u in _db.TblUsers on t.UserId equals u.Id
            where !t.DeleteFlag && !u.DeleteFlag
            orderby t.TechnicianCode
            select new TechnicianModel
            {
                Id            = t.Id,
                TechnicianCode = t.TechnicianCode,
                UserId        = t.UserId,
                FullName      = u.FullName,
                UserName      = u.UserName,
                PhoneNo       = t.PhoneNo,
                Skill         = t.Skill,
                Status        = t.Status
            }
        ).ToListAsync();

        return BaseResponse<List<TechnicianModel>>.Ok(list);
    }

    public async Task<BaseResponse<TechnicianModel>> GetByIdAsync(string id)
    {
        var item = await (
            from t in _db.TblTechnicians
            join u in _db.TblUsers on t.UserId equals u.Id
            where t.Id == id && !t.DeleteFlag
            select new TechnicianModel
            {
                Id            = t.Id,
                TechnicianCode = t.TechnicianCode,
                UserId        = t.UserId,
                FullName      = u.FullName,
                UserName      = u.UserName,
                PhoneNo       = t.PhoneNo,
                Skill         = t.Skill,
                Status        = t.Status
            }
        ).FirstOrDefaultAsync();

        if (item is null) return BaseResponse<TechnicianModel>.Fail("Technician not found.");
        return BaseResponse<TechnicianModel>.Ok(item);
    }

    public async Task<BaseResponse<string>> SaveAsync(SaveTechnicianModel model, string currentUserId)
    {
        if (string.IsNullOrEmpty(model.Id))
        {
            var userId = NewId();
            var techId = NewId();

            var user = new TblUser
            {
                Id           = userId,
                UserCode     = "USR-" + DateTime.Now.ToString("yyMMddHHmmss"),
                UserName     = model.UserName,
                PasswordHash = AuthService.HashPassword(model.Password),
                FullName     = model.FullName,
                Email        = model.Email,
                RoleName     = AppRoles.Technician,
                CreatedAt    = DateTime.Now,
                CreatedBy    = currentUserId
            };

            var tech = new TblTechnician
            {
                Id            = techId,
                TechnicianCode = "TECH-" + DateTime.Now.ToString("yyMMddHHmmss"),
                UserId        = userId,
                PhoneNo       = model.PhoneNo,
                Skill         = model.Skill,
                Status        = TechnicianStatus.Active,
                CreatedAt     = DateTime.Now,
                CreatedBy     = currentUserId
            };

            _db.TblUsers.Add(user);
            _db.TblTechnicians.Add(tech);
        }
        else
        {
            var tech = await _db.TblTechnicians.FirstOrDefaultAsync(t => t.Id == model.Id && !t.DeleteFlag);
            if (tech is null) return BaseResponse<string>.Fail("Technician not found.");

            var user = await _db.TblUsers.FirstOrDefaultAsync(u => u.Id == tech.UserId);
            if (user is not null)
            {
                user.FullName   = model.FullName;
                user.Email      = model.Email;
                user.ModifiedAt = DateTime.Now;
                user.ModifiedBy = currentUserId;
                if (!string.IsNullOrWhiteSpace(model.Password))
                    user.PasswordHash = AuthService.HashPassword(model.Password);
            }

            tech.PhoneNo    = model.PhoneNo;
            tech.Skill      = model.Skill;
            tech.ModifiedAt = DateTime.Now;
            tech.ModifiedBy = currentUserId;
        }

        await _db.SaveChangesAsync();
        return BaseResponse<string>.Ok("Saved successfully.");
    }

    public async Task<BaseResponse<string>> ToggleStatusAsync(string id, string currentUserId)
    {
        var tech = await _db.TblTechnicians.FirstOrDefaultAsync(t => t.Id == id && !t.DeleteFlag);
        if (tech is null) return BaseResponse<string>.Fail("Technician not found.");

        tech.Status     = tech.Status == TechnicianStatus.Active ? TechnicianStatus.Inactive : TechnicianStatus.Active;
        tech.ModifiedAt = DateTime.Now;
        tech.ModifiedBy = currentUserId;

        await _db.SaveChangesAsync();
        return BaseResponse<string>.Ok(tech.Status!);
    }

    private static string NewId() => Guid.NewGuid().ToString("N")[..26];
}
