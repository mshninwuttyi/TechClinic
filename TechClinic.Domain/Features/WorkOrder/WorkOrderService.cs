using Microsoft.EntityFrameworkCore;
using TechClinic.Database.AppDbContextModels;
using TechClinic.Domain.Features.WorkOrder.Models;
using TechClinic.Shared.Enums;
using TechClinic.Shared.Models;

namespace TechClinic.Domain.Features.WorkOrder;

public class WorkOrderService : IWorkOrderService
{
    private readonly AppDbContext _db;

    public WorkOrderService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<List<WorkOrderModel>>> GetListAsync(WorkOrderFilterModel filter)
    {
        var query = _db.TblWorkOrders.Where(w => !w.DeleteFlag).AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.Keyword))
            query = query.Where(w =>
                w.WorkOrderCode.Contains(filter.Keyword) ||
                (w.DeviceType != null && w.DeviceType.Contains(filter.Keyword)) ||
                (w.Brand != null && w.Brand.Contains(filter.Keyword)));

        if (!string.IsNullOrWhiteSpace(filter.Status))
            query = query.Where(w => w.Status == filter.Status);

        if (filter.FromDate.HasValue)
            query = query.Where(w => w.IntakeDate >= filter.FromDate.Value);

        if (filter.ToDate.HasValue)
            query = query.Where(w => w.IntakeDate <= filter.ToDate.Value);

        var list = await query
            .OrderByDescending(w => w.CreatedAt)
            .Select(w => new WorkOrderModel
            {
                Id                  = w.Id,
                WorkOrderCode       = w.WorkOrderCode,
                DeviceType          = w.DeviceType,
                Brand               = w.Brand,
                Model               = w.Model,
                SerialNumber        = w.SerialNumber,
                ProblemDescription  = w.ProblemDescription,
                IntakeDate          = w.IntakeDate,
                EstimatedReturnDate = w.EstimatedReturnDate,
                EstimatedCost       = w.EstimatedCost,
                Status              = w.Status
            }).ToListAsync();

        return BaseResponse<List<WorkOrderModel>>.Ok(list);
    }

    public async Task<BaseResponse<WorkOrderModel>> GetByIdAsync(string id)
    {
        var item = await _db.TblWorkOrders
            .Where(w => w.Id == id && !w.DeleteFlag)
            .Select(w => new WorkOrderModel
            {
                Id                  = w.Id,
                WorkOrderCode       = w.WorkOrderCode,
                DeviceType          = w.DeviceType,
                Brand               = w.Brand,
                Model               = w.Model,
                SerialNumber        = w.SerialNumber,
                ProblemDescription  = w.ProblemDescription,
                IntakeDate          = w.IntakeDate,
                EstimatedReturnDate = w.EstimatedReturnDate,
                EstimatedCost       = w.EstimatedCost,
                Status              = w.Status
            }).FirstOrDefaultAsync();

        if (item is null) return BaseResponse<WorkOrderModel>.Fail("Work order not found.");
        return BaseResponse<WorkOrderModel>.Ok(item);
    }

    public async Task<BaseResponse<string>> SaveAsync(SaveWorkOrderModel model, string currentUserId)
    {
        if (string.IsNullOrEmpty(model.Id))
        {
            var wo = new TblWorkOrder
            {
                Id                  = NewId(),
                WorkOrderCode       = "WO-" + DateTime.Now.ToString("yyMMddHHmmss"),
                DeviceType          = model.DeviceType,
                Brand               = model.Brand,
                Model               = model.Model,
                SerialNumber        = model.SerialNumber,
                ProblemDescription  = model.ProblemDescription,
                IntakeDate          = model.IntakeDate,
                EstimatedReturnDate = model.EstimatedReturnDate,
                EstimatedCost       = model.EstimatedCost,
                Status              = WorkOrderStatus.Open,
                CreatedAt           = DateTime.Now,
                CreatedBy           = currentUserId
            };
            _db.TblWorkOrders.Add(wo);
        }
        else
        {
            var wo = await _db.TblWorkOrders.FirstOrDefaultAsync(w => w.Id == model.Id && !w.DeleteFlag);
            if (wo is null) return BaseResponse<string>.Fail("Work order not found.");

            wo.DeviceType          = model.DeviceType;
            wo.Brand               = model.Brand;
            wo.Model               = model.Model;
            wo.SerialNumber        = model.SerialNumber;
            wo.ProblemDescription  = model.ProblemDescription;
            wo.IntakeDate          = model.IntakeDate;
            wo.EstimatedReturnDate = model.EstimatedReturnDate;
            wo.EstimatedCost       = model.EstimatedCost;
            wo.ModifiedAt          = DateTime.Now;
            wo.ModifiedBy          = currentUserId;
        }

        await _db.SaveChangesAsync();
        return BaseResponse<string>.Ok("Saved successfully.");
    }

    public async Task<BaseResponse<string>> UpdateStatusAsync(string id, string status, string currentUserId)
    {
        var wo = await _db.TblWorkOrders.FirstOrDefaultAsync(w => w.Id == id && !w.DeleteFlag);
        if (wo is null) return BaseResponse<string>.Fail("Work order not found.");

        wo.Status     = status;
        wo.ModifiedAt = DateTime.Now;
        wo.ModifiedBy = currentUserId;

        await _db.SaveChangesAsync();
        return BaseResponse<string>.Ok("Status updated.");
    }

    public async Task<BaseResponse<string>> DeleteAsync(string id, string currentUserId)
    {
        var wo = await _db.TblWorkOrders.FirstOrDefaultAsync(w => w.Id == id && !w.DeleteFlag);
        if (wo is null) return BaseResponse<string>.Fail("Work order not found.");

        wo.DeleteFlag = true;
        wo.ModifiedAt = DateTime.Now;
        wo.ModifiedBy = currentUserId;

        await _db.SaveChangesAsync();
        return BaseResponse<string>.Ok("Deleted.");
    }

    private static string NewId() => Guid.NewGuid().ToString("N")[..26];
}
