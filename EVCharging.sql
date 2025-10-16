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
   1) MASTER / LOOKUP TABLES
   ========================================= */
-- 1.1 ServicePlan (1 -> N Users)
IF OBJECT_ID('dbo.ServicePlan','U') IS NOT NULL DROP TABLE dbo.ServicePlan;
CREATE TABLE dbo.ServicePlan
(
    Id            INT IDENTITY(1,1) CONSTRAINT PK_ServicePlan PRIMARY KEY,
    [Name]        NVARCHAR(120) NOT NULL,
    [Description] NVARCHAR(500) NULL,
    Price         DECIMAL(18,2) NOT NULL CONSTRAINT DF_ServicePlan_Price DEFAULT(0),
    [Status]      TINYINT NOT NULL CONSTRAINT DF_ServicePlan_Status DEFAULT(1) -- 0=inactive,1=active
);
GO

-- 1.2 ChargingStation (1 -> N Users, 1 -> N ChargingPoints)
IF OBJECT_ID('dbo.ChargingStation','U') IS NOT NULL DROP TABLE dbo.ChargingStation;
CREATE TABLE dbo.ChargingStation
(
    Id            INT IDENTITY(1,1) CONSTRAINT PK_ChargingStation PRIMARY KEY,
    [Name]        NVARCHAR(150) NOT NULL,
    [Description] NVARCHAR(500) NULL,
    [Location]    NVARCHAR(300) NULL,
    [Code]        NVARCHAR(50)  NULL,              -- field "station" trong ảnh: dùng như mã/slug
    [Status]      NVARCHAR(20)  NOT NULL           -- (empty/inuse)
                   CONSTRAINT DF_Station_Status DEFAULT(N'empty'),
    CONSTRAINT CK_Station_Status CHECK ([Status] IN (N'empty', N'inuse'))
);
GO

/* =========================================
   2) CORE ENTITIES
   ========================================= */
-- 2.1 [User] (N thuộc 1 ServicePlan; N thuộc 1 ChargingStation)
IF OBJECT_ID('dbo.[User]','U') IS NOT NULL DROP TABLE dbo.[User];
CREATE TABLE dbo.[User]
(
    Id         INT IDENTITY(1,1) CONSTRAINT PK_User PRIMARY KEY,
    Email      NVARCHAR(150) NOT NULL,
    Phone      NVARCHAR(20)  NULL,
    FullName   NVARCHAR(150) NULL,
    [Password] NVARCHAR(200) NOT NULL,         -- nên lưu hash
    [Role]     NVARCHAR(50)  NULL,             -- admin/user/…
    Vehicle    NVARCHAR(150) NULL,
    [Status]   TINYINT NOT NULL CONSTRAINT DF_User_Status DEFAULT(1),

    ServicePlanId   INT NULL,                  -- 1 -> N (nullable: "One to Zero or Many")
    HomeStationId   INT NULL                   -- 1 -> N (nullable)
);
GO
CREATE UNIQUE INDEX UX_User_Email ON dbo.[User](Email);

ALTER TABLE dbo.[User]
ADD CONSTRAINT FK_User_ServicePlan
    FOREIGN KEY (ServicePlanId)
    REFERENCES dbo.ServicePlan(Id)
    ON DELETE SET NULL;                         -- không cascade xóa user

ALTER TABLE dbo.[User]
ADD CONSTRAINT FK_User_HomeStation
    FOREIGN KEY (HomeStationId)
    REFERENCES dbo.ChargingStation(Id)
    ON DELETE SET NULL;                         -- không cascade xóa user
GO

-- 2.2 ChargingPoint (N thuộc 1 ChargingStation)
IF OBJECT_ID('dbo.ChargingPoint','U') IS NOT NULL DROP TABLE dbo.ChargingPoint;
CREATE TABLE dbo.ChargingPoint
(
    Id             INT IDENTITY(1,1) CONSTRAINT PK_ChargingPoint PRIMARY KEY,
    StationId      INT NOT NULL,                           -- FK -> ChargingStation
    PowerLevelKW   INT NULL,                               -- power level (kW)
    PortType       NVARCHAR(20) NOT NULL                   -- CCS / CHAdeMO / AC
                      CONSTRAINT DF_Point_PortType DEFAULT(N'AC'),
    ChargingSpeedKW INT NULL,                              -- tốc độ sạc
    Price          DECIMAL(18,2) NOT NULL CONSTRAINT DF_Point_Price DEFAULT(0),
    [Status]       NVARCHAR(20) NOT NULL
                      CONSTRAINT DF_Point_Status DEFAULT(N'online'),  -- online/offline
    CONSTRAINT CK_Point_Status CHECK ([Status] IN (N'online', N'offline')),
    CONSTRAINT CK_Point_Port CHECK (PortType IN (N'CCS', N'CHAdeMO', N'AC'))
);
GO
ALTER TABLE dbo.ChargingPoint
ADD CONSTRAINT FK_Point_Station
    FOREIGN KEY (StationId)
    REFERENCES dbo.ChargingStation(Id) 
    ON DELETE NO ACTION;                       -- tránh multiple cascade paths
GO
CREATE INDEX IX_Point_Station ON dbo.ChargingPoint(StationId);

-- 2.3 ChargingSession (N thuộc 1 User; N thuộc 1 ChargingPoint)
IF OBJECT_ID('dbo.ChargingSession','U') IS NOT NULL DROP TABLE dbo.ChargingSession;
CREATE TABLE dbo.ChargingSession
(
    Id                 INT IDENTITY(1,1) CONSTRAINT PK_ChargingSession PRIMARY KEY,
    UserId             INT NOT NULL,                      -- FK -> User
    PointId            INT NOT NULL,                      -- FK -> ChargingPoint
    StartTime          DATETIME2(0) NOT NULL,
    EndTime            DATETIME2(0) NULL,
    EnergyConsumedKWh  DECIMAL(18,3) NULL,
    [Status]           NVARCHAR(20) NOT NULL
                         CONSTRAINT DF_Session_Status DEFAULT(N'open')   -- open/closed/cancelled
);
GO
ALTER TABLE dbo.ChargingSession
ADD CONSTRAINT FK_Session_User
    FOREIGN KEY (UserId)
    REFERENCES dbo.[User](Id)
    ON DELETE NO ACTION;                       -- không xóa session khi xóa user

ALTER TABLE dbo.ChargingSession
ADD CONSTRAINT FK_Session_Point
    FOREIGN KEY (PointId)
    REFERENCES dbo.ChargingPoint(Id)
    ON DELETE NO ACTION;                       -- không xóa session khi xóa point
GO
CREATE INDEX IX_Session_User   ON dbo.ChargingSession(UserId);
CREATE INDEX IX_Session_Point  ON dbo.ChargingSession(PointId);
CREATE INDEX IX_Session_Start  ON dbo.ChargingSession(StartTime);

-- 2.4 Transaction (N thuộc 1 ChargingSession)
IF OBJECT_ID('dbo.[Transaction]','U') IS NOT NULL DROP TABLE dbo.[Transaction];
CREATE TABLE dbo.[Transaction]
(
    Id             INT IDENTITY(1,1) CONSTRAINT PK_Transaction PRIMARY KEY,
    SessionId      INT NOT NULL,                   -- FK -> ChargingSession
    [Datetime]     DATETIME2(0) NOT NULL CONSTRAINT DF_Trans_Dt DEFAULT(SYSDATETIME()),
    Total          DECIMAL(18,2) NOT NULL,
    PaymentMethod  NVARCHAR(50) NOT NULL,          -- e.g., CASH, CARD, MOMO
    [Status]       NVARCHAR(20) NOT NULL
                     CONSTRAINT DF_Trans_Status DEFAULT(N'pending')  -- pending/paid/failed/refunded
);
GO
ALTER TABLE dbo.[Transaction]
ADD CONSTRAINT FK_Transaction_Session
    FOREIGN KEY (SessionId)
    REFERENCES dbo.ChargingSession(Id)
    ON DELETE NO ACTION;                           -- không cascade
GO
CREATE INDEX IX_Transaction_Session ON dbo.[Transaction](SessionId);
