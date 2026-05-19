IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
CREATE TABLE [Tbl_Invoice] (
    [Id] nvarchar(26) NOT NULL,
    [InvoiceCode] nvarchar(50) NOT NULL,
    [WorkOrderId] nvarchar(26) NOT NULL,
    [LaborFee] decimal(18,2) NULL DEFAULT 0.0,
    [PartsTotal] decimal(18,2) NULL DEFAULT 0.0,
    [GrandTotal] decimal(18,2) NULL DEFAULT 0.0,
    [PaymentStatus] nvarchar(50) NULL,
    [CreatedAt] datetime2 NOT NULL DEFAULT ((getdate())),
    [CreatedBy] nvarchar(26) NULL,
    [ModifiedAt] datetime2 NULL,
    [ModifiedBy] nvarchar(26) NULL,
    [DeleteFlag] bit NOT NULL,
    CONSTRAINT [PK__Tbl_Invo__3214EC077453BAF5] PRIMARY KEY ([Id])
);

CREATE TABLE [Tbl_ServiceHistory] (
    [Id] nvarchar(26) NOT NULL,
    [ServiceHistoryCode] nvarchar(50) NOT NULL,
    [WorkOrderId] nvarchar(26) NOT NULL,
    [TechnicianId] nvarchar(26) NULL,
    [ActionDescription] nvarchar(max) NULL,
    [ActionDate] datetime2 NOT NULL,
    [CreatedAt] datetime2 NOT NULL DEFAULT ((getdate())),
    [CreatedBy] nvarchar(26) NULL,
    [ModifiedAt] datetime2 NULL,
    [ModifiedBy] nvarchar(26) NULL,
    [DeleteFlag] bit NOT NULL,
    CONSTRAINT [PK__Tbl_Serv__3214EC075EAFD8C2] PRIMARY KEY ([Id])
);

CREATE TABLE [Tbl_SparePart] (
    [Id] nvarchar(26) NOT NULL,
    [PartCode] nvarchar(50) NOT NULL,
    [PartName] nvarchar(200) NOT NULL,
    [Quantity] int NOT NULL,
    [UnitPrice] decimal(18,2) NULL DEFAULT 0.0,
    [CreatedAt] datetime2 NOT NULL DEFAULT ((getdate())),
    [CreatedBy] nvarchar(26) NULL,
    [ModifiedAt] datetime2 NULL,
    [ModifiedBy] nvarchar(26) NULL,
    [DeleteFlag] bit NOT NULL,
    CONSTRAINT [PK__Tbl_Spar__3214EC07D5DE2504] PRIMARY KEY ([Id])
);

CREATE TABLE [Tbl_Technician] (
    [Id] nvarchar(26) NOT NULL,
    [TechnicianCode] nvarchar(50) NOT NULL,
    [UserId] nvarchar(26) NOT NULL,
    [PhoneNo] nvarchar(50) NULL,
    [Skill] nvarchar(200) NULL,
    [Status] nvarchar(50) NULL,
    [CreatedAt] datetime2 NOT NULL DEFAULT ((getdate())),
    [CreatedBy] nvarchar(26) NULL,
    [ModifiedAt] datetime2 NULL,
    [ModifiedBy] nvarchar(26) NULL,
    [DeleteFlag] bit NOT NULL,
    CONSTRAINT [PK__Tbl_Tech__3214EC079F2ED85D] PRIMARY KEY ([Id])
);

CREATE TABLE [Tbl_User] (
    [Id] nvarchar(26) NOT NULL,
    [UserCode] nvarchar(50) NOT NULL,
    [UserName] nvarchar(100) NOT NULL,
    [PasswordHash] nvarchar(max) NOT NULL,
    [FullName] nvarchar(150) NOT NULL,
    [Email] nvarchar(200) NULL,
    [RoleName] nvarchar(50) NOT NULL,
    [CreatedAt] datetime2 NOT NULL DEFAULT ((getdate())),
    [CreatedBy] nvarchar(26) NULL,
    [ModifiedAt] datetime2 NULL,
    [ModifiedBy] nvarchar(26) NULL,
    [DeleteFlag] bit NOT NULL,
    CONSTRAINT [PK__Tbl_User__3214EC07A14DC678] PRIMARY KEY ([Id])
);

CREATE TABLE [Tbl_WorkOrder] (
    [Id] nvarchar(26) NOT NULL,
    [WorkOrderCode] nvarchar(50) NOT NULL,
    [DeviceType] nvarchar(100) NULL,
    [Brand] nvarchar(100) NULL,
    [Model] nvarchar(100) NULL,
    [SerialNumber] nvarchar(100) NULL,
    [ProblemDescription] nvarchar(max) NULL,
    [IntakeDate] datetime2 NOT NULL,
    [EstimatedReturnDate] datetime2 NULL,
    [EstimatedCost] decimal(18,2) NULL DEFAULT 0.0,
    [Status] nvarchar(50) NULL,
    [CreatedAt] datetime2 NOT NULL DEFAULT ((getdate())),
    [CreatedBy] nvarchar(26) NULL,
    [ModifiedAt] datetime2 NULL,
    [ModifiedBy] nvarchar(26) NULL,
    [DeleteFlag] bit NOT NULL,
    CONSTRAINT [PK__Tbl_Work__3214EC0799CD80E5] PRIMARY KEY ([Id])
);

CREATE TABLE [Tbl_WorkOrderAssignment] (
    [Id] nvarchar(26) NOT NULL,
    [AssignmentCode] nvarchar(50) NOT NULL,
    [WorkOrderId] nvarchar(26) NOT NULL,
    [TechnicianId] nvarchar(26) NOT NULL,
    [AssignedDate] datetime2 NOT NULL,
    [CreatedAt] datetime2 NOT NULL DEFAULT ((getdate())),
    [CreatedBy] nvarchar(26) NULL,
    [ModifiedAt] datetime2 NULL,
    [ModifiedBy] nvarchar(26) NULL,
    [DeleteFlag] bit NOT NULL,
    CONSTRAINT [PK__Tbl_Work__3214EC07D64889E2] PRIMARY KEY ([Id])
);

CREATE TABLE [Tbl_WorkOrderPart] (
    [Id] nvarchar(26) NOT NULL,
    [WorkOrderPartCode] nvarchar(50) NOT NULL,
    [WorkOrderId] nvarchar(26) NOT NULL,
    [SparePartId] nvarchar(26) NOT NULL,
    [Quantity] int NOT NULL,
    [UnitPrice] decimal(18,2) NOT NULL,
    [CreatedAt] datetime2 NOT NULL DEFAULT ((getdate())),
    [CreatedBy] nvarchar(26) NULL,
    [ModifiedAt] datetime2 NULL,
    [ModifiedBy] nvarchar(26) NULL,
    [DeleteFlag] bit NOT NULL,
    CONSTRAINT [PK__Tbl_Work__3214EC07B942BCCC] PRIMARY KEY ([Id])
);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260519155224_InitialCreate', N'9.0.9');

COMMIT;
GO

