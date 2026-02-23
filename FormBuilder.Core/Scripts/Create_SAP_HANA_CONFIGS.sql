-- Create table to store SAP HANA connection configuration securely (encrypted at rest in DB)
-- Notes:
-- - ConnectionStringEncrypted is encrypted using ASP.NET Core DataProtection in the API.
-- - Keep only one active row (IsActive = 1) to be used by the application.

IF OBJECT_ID(N'dbo.SAP_HANA_CONFIGS', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.SAP_HANA_CONFIGS
    (
        Id INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_SAP_HANA_CONFIGS PRIMARY KEY,
        Name NVARCHAR(200) NOT NULL,
        ConnectionStringEncrypted NVARCHAR(2000) NOT NULL,

        IsActive BIT NOT NULL CONSTRAINT DF_SAP_HANA_CONFIGS_IsActive DEFAULT(0),
        IsDeleted BIT NOT NULL CONSTRAINT DF_SAP_HANA_CONFIGS_IsDeleted DEFAULT(0),

        CreatedByUserId NVARCHAR(450) NULL,
        CreatedDate DATETIME2 NOT NULL CONSTRAINT DF_SAP_HANA_CONFIGS_CreatedDate DEFAULT(SYSUTCDATETIME()),
        UpdatedDate DATETIME2 NULL,
        DeletedDate DATETIME2 NULL,
        DeletedByUserId NVARCHAR(450) NULL
    );

    CREATE INDEX IX_SAP_HANA_CONFIGS_IsActive ON dbo.SAP_HANA_CONFIGS(IsActive) INCLUDE (IsDeleted);
END
GO


