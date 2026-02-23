using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FormBuilder.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddSapIntegrationSettingsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                IF OBJECT_ID(N'[dbo].[SAP_INTEGRATION_SETTINGS]', N'U') IS NULL
                BEGIN
                    CREATE TABLE [dbo].[SAP_INTEGRATION_SETTINGS](
                        [Id] [int] IDENTITY(1,1) NOT NULL,
                        [DocumentTypeId] [int] NOT NULL,
                        [SapConfigId] [int] NOT NULL,
                        [TargetEndpoint] [nvarchar](200) NOT NULL,
                        [HttpMethod] [nvarchar](10) NOT NULL CONSTRAINT [DF_SAP_INTEGRATION_SETTINGS_HttpMethod] DEFAULT(N'POST'),
                        [TargetObject] [nvarchar](200) NULL,
                        [ExecutionMode] [nvarchar](50) NOT NULL,
                        [TriggerStageId] [int] NULL,
                        [BlockWorkflowOnError] [bit] NOT NULL CONSTRAINT [DF_SAP_INTEGRATION_SETTINGS_BlockWorkflowOnError] DEFAULT(0),
                        [IsActive] [bit] NOT NULL CONSTRAINT [DF_SAP_INTEGRATION_SETTINGS_IsActive] DEFAULT(1),
                        [CreatedByUserId] [nvarchar](450) NULL,
                        [CreatedDate] [datetime2](7) NOT NULL CONSTRAINT [DF_SAP_INTEGRATION_SETTINGS_CreatedDate] DEFAULT(SYSUTCDATETIME()),
                        [UpdatedDate] [datetime2](7) NULL,
                        [IsDeleted] [bit] NOT NULL CONSTRAINT [DF_SAP_INTEGRATION_SETTINGS_IsDeleted] DEFAULT(0),
                        [DeletedDate] [datetime2](7) NULL,
                        [DeletedByUserId] [nvarchar](450) NULL,
                        CONSTRAINT [PK_SAP_INTEGRATION_SETTINGS] PRIMARY KEY CLUSTERED ([Id] ASC)
                    );

                    CREATE INDEX [IX_SAP_INTEGRATION_SETTINGS_DocumentTypeId]
                        ON [dbo].[SAP_INTEGRATION_SETTINGS]([DocumentTypeId]);

                    CREATE INDEX [IX_SAP_INTEGRATION_SETTINGS_SapConfigId]
                        ON [dbo].[SAP_INTEGRATION_SETTINGS]([SapConfigId]);
                END
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                IF OBJECT_ID(N'[dbo].[SAP_INTEGRATION_SETTINGS]', N'U') IS NOT NULL
                BEGIN
                    DROP TABLE [dbo].[SAP_INTEGRATION_SETTINGS];
                END
                """);
        }
    }
}
