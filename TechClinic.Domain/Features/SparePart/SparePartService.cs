using Microsoft.EntityFrameworkCore;
using TechClinic.Database.AppDbContextModels;
using TechClinic.Domain.Features.SparePart.Models;
using TechClinic.Shared.Models;

namespace TechClinic.Domain.Features.SparePart;

public class SparePartService : ISparePartService
{
    private readonly AppDbContext _db;

    public SparePartService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<List<SparePartModel>>> GetListAsync()
    {
        var list = await _db.TblSpareParts
            .Where(s => !s.DeleteFlag)
            .OrderBy(s => s.PartName)
            .Select(s => new SparePartModel
            {
                Id        = s.Id,
                PartCode  = s.PartCode,
                PartName  = s.PartName,
                Quantity  = s.Quantity,
                UnitPrice = s.UnitPrice
            }).ToListAsync();

        return BaseResponse<List<SparePartModel>>.Ok(list);
    }

    public async Task<BaseResponse<SparePartModel>> GetByIdAsync(string id)
    {
        var item = await _db.TblSpareParts
            .Where(s => s.Id == id && !s.DeleteFlag)
            .Select(s => new SparePartModel
            {
                Id        = s.Id,
                PartCode  = s.PartCode,
                PartName  = s.PartName,
                Quantity  = s.Quantity,
                UnitPrice = s.UnitPrice
            }).FirstOrDefaultAsync();

        if (item is null) return BaseResponse<SparePartModel>.Fail("Spare part not found.");
        return BaseResponse<SparePartModel>.Ok(item);
    }

    public async Task<BaseResponse<string>> SaveAsync(SaveSparePartModel model, string currentUserId)
    {
        if (string.IsNullOrEmpty(model.Id))
        {
            var part = new TblSparePart
            {
                Id        = NewId(),
                PartCode  = "PRT-" + DateTime.Now.ToString("yyMMddHHmmss"),
                PartName  = model.PartName,
                Quantity  = model.Quantity,
                UnitPrice = model.UnitPrice,
                CreatedAt = DateTime.Now,
                CreatedBy = currentUserId
            };
            _db.TblSpareParts.Add(part);
        }
        else
        {
            var part = await _db.TblSpareParts.FirstOrDefaultAsync(s => s.Id == model.Id && !s.DeleteFlag);
            if (part is null) return BaseResponse<string>.Fail("Spare part not found.");

            part.PartName   = model.PartName;
            part.Quantity   = model.Quantity;
            part.UnitPrice  = model.UnitPrice;
            part.ModifiedAt = DateTime.Now;
            part.ModifiedBy = currentUserId;
        }

        await _db.SaveChangesAsync();
        return BaseResponse<string>.Ok("Saved successfully.");
    }

    public async Task<BaseResponse<string>> UpdateStockAsync(string id, int quantity, string currentUserId)
    {
        var part = await _db.TblSpareParts.FirstOrDefaultAsync(s => s.Id == id && !s.DeleteFlag);
        if (part is null) return BaseResponse<string>.Fail("Spare part not found.");

        part.Quantity   = quantity;
        part.ModifiedAt = DateTime.Now;
        part.ModifiedBy = currentUserId;

        await _db.SaveChangesAsync();
        return BaseResponse<string>.Ok("Stock updated.");
    }

    private static string NewId() => Guid.NewGuid().ToString("N")[..26];
}
