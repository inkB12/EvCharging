/* =========================================
   0) CREATE & USE DATABASE
   ========================================= */
IF DB_ID(N'EVCharging') IS NULL
BEGIN
    CREATE DATABASE EVCharging;
END
GO
USE EVCharging;
GO

/* =========================================
   1) LOOKUP / MASTER
   ========================================= */
IF OBJECT_ID('dbo.ServicePlan','U') IS NOT NULL DROP TABLE dbo.ServicePlan;
CREATE TABLE dbo.ServicePlan
(
    Id            INT IDENTITY(1,1) PRIMARY KEY,
    [Name]        NVARCHAR(120) NOT NULL,
    [Description] NVARCHAR(500) NULL,
    Price         DECIMAL(18,2) NOT NULL CONSTRAINT DF_ServicePlan_Price DEFAULT(0),
    IsDeleted     BIT NOT NULL CONSTRAINT DF_ServicePlan_IsDeleted DEFAULT(0)
);
GO

/* =========================================
   2) CORE ENTITIES
   ========================================= */
IF OBJECT_ID('dbo.ChargingStation','U') IS NOT NULL DROP TABLE dbo.ChargingStation;
CREATE TABLE dbo.ChargingStation
(
    Id            INT IDENTITY(1,1) PRIMARY KEY,
    [Name]        NVARCHAR(150) NOT NULL,
    [Description] NVARCHAR(500) NULL,
    [Location]    NVARCHAR(300) NULL,
    [Station]     NVARCHAR(50)  NULL,  -- mã/slug trạm (theo ERD: "station")
    [Status]      NVARCHAR(20)  NOT NULL CONSTRAINT DF_Station_Status DEFAULT(N'empty'),
    CONSTRAINT CK_Station_Status CHECK ([Status] IN (N'empty', N'inuse'))
);
GO

IF OBJECT_ID('dbo.[User]','U') IS NOT NULL DROP TABLE dbo.[User];
CREATE TABLE dbo.[User]
(
    Id            INT IDENTITY(1,1) PRIMARY KEY,
    Email         NVARCHAR(150) NOT NULL,
    Phone         NVARCHAR(20)  NULL,
    FullName      NVARCHAR(150) NULL,
    [Password]    NVARCHAR(200) NOT NULL,  -- lưu SHA256 (base64) như yêu cầu
    [Role]        NVARCHAR(50)  NULL,
    Vehicle       NVARCHAR(150) NULL,
    IsDeleted     BIT NOT NULL CONSTRAINT DF_User_IsDeleted DEFAULT(0),

    ServicePlanId INT NULL,
    HomeStationId INT NULL
);
GO
CREATE UNIQUE INDEX UX_User_Email ON dbo.[User](Email);

ALTER TABLE dbo.[User]
  ADD CONSTRAINT FK_User_ServicePlan
  FOREIGN KEY (ServicePlanId) REFERENCES dbo.ServicePlan(Id)
  ON DELETE SET NULL;

ALTER TABLE dbo.[User]
  ADD CONSTRAINT FK_User_HomeStation
  FOREIGN KEY (HomeStationId) REFERENCES dbo.ChargingStation(Id)
  ON DELETE SET NULL;
GO

IF OBJECT_ID('dbo.ChargingPoint','U') IS NOT NULL DROP TABLE dbo.ChargingPoint;
CREATE TABLE dbo.ChargingPoint
(
    Id               INT IDENTITY(1,1) PRIMARY KEY,
    StationId        INT NOT NULL,                         -- FK -> ChargingStation
    PowerLevelKW     INT NULL,
    PortType         NVARCHAR(20) NOT NULL CONSTRAINT DF_Point_PortType DEFAULT(N'AC'),
    ChargingSpeedKW  INT NULL,
    Price            DECIMAL(18,2) NOT NULL CONSTRAINT DF_Point_Price DEFAULT(0),
    [Status]         NVARCHAR(20) NOT NULL CONSTRAINT DF_Point_Status DEFAULT(N'online'),
    CONSTRAINT CK_Point_Status CHECK ([Status] IN (N'online', N'offline')),
    CONSTRAINT CK_Point_PortType CHECK (PortType IN (N'CCS', N'CHAdeMO', N'AC'))
);
GO
ALTER TABLE dbo.ChargingPoint
  ADD CONSTRAINT FK_Point_Station
  FOREIGN KEY (StationId) REFERENCES dbo.ChargingStation(Id)
  ON DELETE NO ACTION;
GO
CREATE INDEX IX_Point_Station ON dbo.ChargingPoint(StationId);

IF OBJECT_ID('dbo.Booking','U') IS NOT NULL DROP TABLE dbo.Booking;
CREATE TABLE dbo.Booking
(
    Id           INT IDENTITY(1,1) PRIMARY KEY,
    UserId       INT NOT NULL,                       -- 1 User -> N Booking
    BookingTime  DATETIME2(0) NOT NULL CONSTRAINT DF_BookingTime DEFAULT(SYSDATETIME()),
    StartTime    DATETIME2(0) NULL,
    EndTime      DATETIME2(0) NULL,
    Price        DECIMAL(18,2) NOT NULL CONSTRAINT DF_Booking_Price DEFAULT(0),
    [Status]     NVARCHAR(20) NOT NULL CONSTRAINT DF_Booking_Status DEFAULT(N'ongoing'),
    CONSTRAINT CK_Booking_Status CHECK ([Status] IN (N'success', N'cancelled', N'ongoing'))
);
GO
ALTER TABLE dbo.Booking
  ADD CONSTRAINT FK_Booking_User
  FOREIGN KEY (UserId) REFERENCES dbo.[User](Id)
  ON DELETE NO ACTION;
GO
CREATE INDEX IX_Booking_User ON dbo.Booking(UserId);

IF OBJECT_ID('dbo.ChargingSession','U') IS NOT NULL DROP TABLE dbo.ChargingSession;
CREATE TABLE dbo.ChargingSession
(
    Id                 INT IDENTITY(1,1) PRIMARY KEY,
    BookingId          INT NOT NULL,                      -- theo ERD: Session gắn với Booking
    PointId            INT NOT NULL,                      -- 1 Point -> N Session
    StartTime          DATETIME2(0) NOT NULL,
    EndTime            DATETIME2(0) NULL,
    EnergyConsumedKWh  DECIMAL(18,3) NULL,
    [Status]           NVARCHAR(20) NOT NULL CONSTRAINT DF_Session_Status DEFAULT(N'coming-soon'),
    CONSTRAINT CK_Session_Status CHECK ([Status] IN (N'coming-soon', N'ongoing', N'success'))
);
GO
ALTER TABLE dbo.ChargingSession
  ADD CONSTRAINT FK_Session_Booking
  FOREIGN KEY (BookingId) REFERENCES dbo.Booking(Id)
  ON DELETE NO ACTION;

ALTER TABLE dbo.ChargingSession
  ADD CONSTRAINT FK_Session_Point
  FOREIGN KEY (PointId) REFERENCES dbo.ChargingPoint(Id)
  ON DELETE NO ACTION;
GO
CREATE INDEX IX_Session_Booking ON dbo.ChargingSession(BookingId);
CREATE INDEX IX_Session_Point   ON dbo.ChargingSession(PointId);
CREATE INDEX IX_Session_Start   ON dbo.ChargingSession(StartTime);

IF OBJECT_ID('dbo.[Transaction]','U') IS NOT NULL DROP TABLE dbo.[Transaction];
CREATE TABLE dbo.[Transaction]
(
    Id            INT IDENTITY(1,1) PRIMARY KEY,
    BookingId     INT NOT NULL,                         -- theo ERD: Transaction gắn với Booking
    [Datetime]    DATETIME2(0) NOT NULL CONSTRAINT DF_Trans_Dt DEFAULT(SYSDATETIME()),
    Total         DECIMAL(18,2) NOT NULL,
    PaymentMethod NVARCHAR(50) NOT NULL,               -- CASH/CARD/MOMO/...
    [Status]      NVARCHAR(20) NOT NULL CONSTRAINT DF_Trans_Status DEFAULT(N'ongoing'),
    CONSTRAINT CK_Trans_Status CHECK ([Status] IN (N'ongoing', N'success', N'failed'))
);
GO
ALTER TABLE dbo.[Transaction]
  ADD CONSTRAINT FK_Transaction_Booking
  FOREIGN KEY (BookingId) REFERENCES dbo.Booking(Id)
  ON DELETE NO ACTION;
GO
CREATE INDEX IX_Transaction_Booking ON dbo.[Transaction](BookingId);

IF OBJECT_ID('dbo.FaultReport','U') IS NOT NULL DROP TABLE dbo.FaultReport;
CREATE TABLE dbo.FaultReport
(
    Id          INT IDENTITY(1,1) PRIMARY KEY,
    UserId      INT NOT NULL,                           -- người báo lỗi
    PointId     INT NOT NULL,                           -- điểm sạc gặp sự cố
    ReportTime  DATETIME2(0) NOT NULL CONSTRAINT DF_Fault_Time DEFAULT(SYSDATETIME()),
    [Title]     NVARCHAR(200) NOT NULL,
    [Description] NVARCHAR(1000) NULL,
    Severity    TINYINT NOT NULL,                       -- 1..5 tùy chọn
    [Status]    NVARCHAR(10) NOT NULL CONSTRAINT DF_Fault_Status DEFAULT(N'open'),
    CONSTRAINT CK_Fault_Status CHECK ([Status] IN (N'open', N'closed'))
);
GO
ALTER TABLE dbo.FaultReport
  ADD CONSTRAINT FK_Fault_User
  FOREIGN KEY (UserId) REFERENCES dbo.[User](Id)
  ON DELETE NO ACTION;

ALTER TABLE dbo.FaultReport
  ADD CONSTRAINT FK_Fault_Point
  FOREIGN KEY (PointId) REFERENCES dbo.ChargingPoint(Id)
  ON DELETE NO ACTION;
GO
CREATE INDEX IX_Fault_User  ON dbo.FaultReport(UserId);
CREATE INDEX IX_Fault_Point ON dbo.FaultReport(PointId);
