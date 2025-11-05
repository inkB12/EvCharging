USE EVCharging;
GO
ALTER TABLE dbo.ChargingSession
    ALTER COLUMN StartTime DATETIME2(0) NULL;
GO
