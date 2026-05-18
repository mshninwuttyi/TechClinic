using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TechClinic.Database.AppDbContextModels;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TblInvoice> TblInvoices { get; set; }

    public virtual DbSet<TblServiceHistory> TblServiceHistories { get; set; }

    public virtual DbSet<TblSparePart> TblSpareParts { get; set; }

    public virtual DbSet<TblTechnician> TblTechnicians { get; set; }

    public virtual DbSet<TblUser> TblUsers { get; set; }

    public virtual DbSet<TblWorkOrder> TblWorkOrders { get; set; }

    public virtual DbSet<TblWorkOrderAssignment> TblWorkOrderAssignments { get; set; }

    public virtual DbSet<TblWorkOrderPart> TblWorkOrderParts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblInvoice>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tbl_Invo__3214EC077453BAF5");

            entity.ToTable("Tbl_Invoice");

            entity.Property(e => e.Id).HasMaxLength(26);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.CreatedBy).HasMaxLength(26);
            entity.Property(e => e.GrandTotal)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.InvoiceCode).HasMaxLength(50);
            entity.Property(e => e.LaborFee)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ModifiedBy).HasMaxLength(26);
            entity.Property(e => e.PartsTotal)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PaymentStatus).HasMaxLength(50);
            entity.Property(e => e.WorkOrderId).HasMaxLength(26);
        });

        modelBuilder.Entity<TblServiceHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tbl_Serv__3214EC075EAFD8C2");

            entity.ToTable("Tbl_ServiceHistory");

            entity.Property(e => e.Id).HasMaxLength(26);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.CreatedBy).HasMaxLength(26);
            entity.Property(e => e.ModifiedBy).HasMaxLength(26);
            entity.Property(e => e.ServiceHistoryCode).HasMaxLength(50);
            entity.Property(e => e.TechnicianId).HasMaxLength(26);
            entity.Property(e => e.WorkOrderId).HasMaxLength(26);
        });

        modelBuilder.Entity<TblSparePart>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tbl_Spar__3214EC07D5DE2504");

            entity.ToTable("Tbl_SparePart");

            entity.Property(e => e.Id).HasMaxLength(26);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.CreatedBy).HasMaxLength(26);
            entity.Property(e => e.ModifiedBy).HasMaxLength(26);
            entity.Property(e => e.PartCode).HasMaxLength(50);
            entity.Property(e => e.PartName).HasMaxLength(200);
            entity.Property(e => e.UnitPrice)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<TblTechnician>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tbl_Tech__3214EC079F2ED85D");

            entity.ToTable("Tbl_Technician");

            entity.Property(e => e.Id).HasMaxLength(26);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.CreatedBy).HasMaxLength(26);
            entity.Property(e => e.ModifiedBy).HasMaxLength(26);
            entity.Property(e => e.PhoneNo).HasMaxLength(50);
            entity.Property(e => e.Skill).HasMaxLength(200);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.TechnicianCode).HasMaxLength(50);
            entity.Property(e => e.UserId).HasMaxLength(26);
        });

        modelBuilder.Entity<TblUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tbl_User__3214EC07A14DC678");

            entity.ToTable("Tbl_User");

            entity.Property(e => e.Id).HasMaxLength(26);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.CreatedBy).HasMaxLength(26);
            entity.Property(e => e.Email).HasMaxLength(200);
            entity.Property(e => e.FullName).HasMaxLength(150);
            entity.Property(e => e.ModifiedBy).HasMaxLength(26);
            entity.Property(e => e.RoleName).HasMaxLength(50);
            entity.Property(e => e.UserCode).HasMaxLength(50);
            entity.Property(e => e.UserName).HasMaxLength(100);
        });

        modelBuilder.Entity<TblWorkOrder>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tbl_Work__3214EC0799CD80E5");

            entity.ToTable("Tbl_WorkOrder");

            entity.Property(e => e.Id).HasMaxLength(26);
            entity.Property(e => e.Brand).HasMaxLength(100);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.CreatedBy).HasMaxLength(26);
            entity.Property(e => e.DeviceType).HasMaxLength(100);
            entity.Property(e => e.EstimatedCost)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Model).HasMaxLength(100);
            entity.Property(e => e.ModifiedBy).HasMaxLength(26);
            entity.Property(e => e.SerialNumber).HasMaxLength(100);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.WorkOrderCode).HasMaxLength(50);
        });

        modelBuilder.Entity<TblWorkOrderAssignment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tbl_Work__3214EC07D64889E2");

            entity.ToTable("Tbl_WorkOrderAssignment");

            entity.Property(e => e.Id).HasMaxLength(26);
            entity.Property(e => e.AssignmentCode).HasMaxLength(50);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.CreatedBy).HasMaxLength(26);
            entity.Property(e => e.ModifiedBy).HasMaxLength(26);
            entity.Property(e => e.TechnicianId).HasMaxLength(26);
            entity.Property(e => e.WorkOrderId).HasMaxLength(26);
        });

        modelBuilder.Entity<TblWorkOrderPart>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tbl_Work__3214EC07B942BCCC");

            entity.ToTable("Tbl_WorkOrderPart");

            entity.Property(e => e.Id).HasMaxLength(26);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.CreatedBy).HasMaxLength(26);
            entity.Property(e => e.ModifiedBy).HasMaxLength(26);
            entity.Property(e => e.SparePartId).HasMaxLength(26);
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.WorkOrderId).HasMaxLength(26);
            entity.Property(e => e.WorkOrderPartCode).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
