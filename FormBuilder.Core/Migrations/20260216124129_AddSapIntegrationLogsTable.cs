using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FormBuilder.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddSapIntegrationLogsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                IF OBJECT_ID(N'[dbo].[SAP_INTEGRATION_LOGS]', N'U') IS NULL
                BEGIN
                    CREATE TABLE [dbo].[SAP_INTEGRATION_LOGS](
                        [Id] [int] IDENTITY(1,1) NOT NULL,
                        [FormId] [int] NOT NULL,
                        [SubmissionId] [int] NOT NULL,
                        [SapConfigId] [int] NOT NULL,
                        [Endpoint] [nvarchar](200) NOT NULL,
                        [EventType] [nvarchar](50) NOT NULL,
                        [Status] [nvarchar](20) NOT NULL,
                        [RequestPayloadJson] [nvarchar](max) NULL,
                        [ResponsePayloadJson] [nvarchar](max) NULL,
                        [ErrorMessage] [nvarchar](max) NULL,
                        [TimestampUtc] [datetime2](7) NOT NULL CONSTRAINT [DF_SAP_INTEGRATION_LOGS_TimestampUtc] DEFAULT(SYSUTCDATETIME()),
                        [IsActive] [bit] NOT NULL CONSTRAINT [DF_SAP_INTEGRATION_LOGS_IsActive] DEFAULT(1),
                        [CreatedByUserId] [nvarchar](450) NULL,
                        [CreatedDate] [datetime2](7) NOT NULL CONSTRAINT [DF_SAP_INTEGRATION_LOGS_CreatedDate] DEFAULT(SYSUTCDATETIME()),
                        [UpdatedDate] [datetime2](7) NULL,
                        [IsDeleted] [bit] NOT NULL CONSTRAINT [DF_SAP_INTEGRATION_LOGS_IsDeleted] DEFAULT(0),
                        [DeletedDate] [datetime2](7) NULL,
                        [DeletedByUserId] [nvarchar](450) NULL,
                        CONSTRAINT [PK_SAP_INTEGRATION_LOGS] PRIMARY KEY CLUSTERED ([Id] ASC)
                    );

                    CREATE INDEX [IX_SAP_INTEGRATION_LOGS_FormId] ON [dbo].[SAP_INTEGRATION_LOGS]([FormId]);
                    CREATE INDEX [IX_SAP_INTEGRATION_LOGS_SubmissionId] ON [dbo].[SAP_INTEGRATION_LOGS]([SubmissionId]);
                    CREATE INDEX [IX_SAP_INTEGRATION_LOGS_SapConfigId] ON [dbo].[SAP_INTEGRATION_LOGS]([SapConfigId]);
                    CREATE INDEX [IX_SAP_INTEGRATION_LOGS_TimestampUtc] ON [dbo].[SAP_INTEGRATION_LOGS]([TimestampUtc]);
                END
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                IF OBJECT_ID(N'[dbo].[SAP_INTEGRATION_LOGS]', N'U') IS NOT NULL
                BEGIN
                    DROP TABLE [dbo].[SAP_INTEGRATION_LOGS];
                END
                """);
        }
    }
}
