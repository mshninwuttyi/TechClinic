using Microsoft.EntityFrameworkCore;
using TechClinic.Database.AppDbContextModels;
using TechClinic.Domain.Features.Assignment.Models;
using TechClinic.Shared.Models;

namespace TechClinic.Domain.Features.Assignment;

public class AssignmentService : IAssignmentService
{
    private readonly AppDbContext _db;

    public AssignmentService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<AssignmentModel>> GetByWorkOrderAsync(string workOrderId)
    {
        var item = await (
            from a  in _db.TblWorkOrderAssignments
            join t  in _db.TblTechnicians on a.TechnicianId equals t.Id
            join u  in _db.TblUsers       on t.UserId        equals u.Id
            join wo in _db.TblWorkOrders  on a.WorkOrderId   equals wo.Id
            where a.WorkOrderId == workOrderId && !a.DeleteFlag
            select new AssignmentModel
            {
                Id             = a.Id,
                AssignmentCode = a.AssignmentCode,
                WorkOrderId    = a.WorkOrderId,
                WorkOrderCode  = wo.WorkOrderCode,
                TechnicianId   = a.TechnicianId,
                TechnicianName = u.FullName,
                AssignedDate   = a.AssignedDate
            }
        ).FirstOrDefaultAsync();

        if (item is null) return BaseResponse<AssignmentModel>.Fail("No assignment found for this work order.");
        return BaseResponse<AssignmentModel>.Ok(item);
    }

    public async Task<BaseResponse<string>> AssignAsync(SaveAssignmentModel model, string currentUserId)
    {
        var existing = await _db.TblWorkOrderAssignments
            .FirstOrDefaultAsync(a => a.WorkOrderId == model.WorkOrderId && !a.DeleteFlag);

        if (existing is not null)
            return BaseResponse<string>.Fail("This work order is already assigned. Use Reassign instead.");

        var assignment = new TblWorkOrderAssignment
        {
            Id             = NewId(),
            AssignmentCode = "ASN-" + DateTime.Now.ToString("yyMMddHHmmss"),
            WorkOrderId    = model.WorkOrderId,
            TechnicianId   = model.TechnicianId,
            AssignedDate   = DateTime.Now,
            CreatedAt      = DateTime.Now,
            CreatedBy      = currentUserId
        };

        _db.TblWorkOrderAssignments.Add(assignment);
        await _db.SaveChangesAsync();
        return BaseResponse<string>.Ok("Assigned successfully.");
    }

    public async Task<BaseResponse<string>> ReassignAsync(string workOrderId, string technicianId, string currentUserId)
    {
        var existing = await _db.TblWorkOrderAssignments
            .FirstOrDefaultAsync(a => a.WorkOrderId == workOrderId && !a.DeleteFlag);

        if (existing is null)
            return BaseResponse<string>.Fail("No active assignment found. Use Assign instead.");

        existing.TechnicianId = technicianId;
        existing.AssignedDate = DateTime.Now;
        existing.ModifiedAt   = DateTime.Now;
        existing.ModifiedBy   = currentUserId;

        await _db.SaveChangesAsync();
        return BaseResponse<string>.Ok("Reassigned successfully.");
    }

    private static string NewId() => Guid.NewGuid().ToString("N")[..26];
}
