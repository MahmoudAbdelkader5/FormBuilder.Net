using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FormBuilder.Core.Migrations
{
    /// <inheritdoc />
    public partial class DocumentSeriesEngine : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GenerateOn",
                table: "DOCUMENT_SERIES",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "Submit");

            migrationBuilder.AddColumn<string>(
                name: "ResetPolicy",
                table: "DOCUMENT_SERIES",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "None");

            migrationBuilder.AddColumn<int>(
                name: "SequencePadding",
                table: "DOCUMENT_SERIES",
                type: "int",
                nullable: false,
                defaultValue: 3);

            migrationBuilder.AddColumn<int>(
                name: "SequenceStart",
                table: "DOCUMENT_SERIES",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<string>(
                name: "SeriesName",
                table: "DOCUMENT_SERIES",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Template",
                table: "DOCUMENT_SERIES",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.Sql(@"
UPDATE DOCUMENT_SERIES
SET
    SeriesName = CASE WHEN LTRIM(RTRIM(ISNULL(SeriesName, ''))) = '' THEN SeriesCode ELSE SeriesName END,
    SequenceStart = CASE
        WHEN SequenceStart <= 0 THEN CASE WHEN NextNumber > 0 THEN NextNumber ELSE 1 END
        ELSE SequenceStart
    END,
    SequencePadding = CASE WHEN SequencePadding <= 0 THEN 3 ELSE SequencePadding END,
    ResetPolicy = CASE WHEN LTRIM(RTRIM(ISNULL(ResetPolicy, ''))) = '' THEN 'None' ELSE ResetPolicy END,
    GenerateOn = CASE WHEN LTRIM(RTRIM(ISNULL(GenerateOn, ''))) = '' THEN 'Submit' ELSE GenerateOn END,
    Template = CASE WHEN LTRIM(RTRIM(ISNULL(Template, ''))) = '' THEN CONCAT(SeriesCode, '-{SEQ}') ELSE Template END;
");

            migrationBuilder.CreateTable(
                name: "DOCUMENT_NUMBER_AUDIT",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FormSubmissionId = table.Column<int>(type: "int", nullable: false),
                    SeriesId = table.Column<int>(type: "int", nullable: false),
                    GeneratedNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TemplateUsed = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    GeneratedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GeneratedOn = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    GeneratedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DOCUMENT_NUMBER_AUDIT", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DOCUMENT_SERIES_COUNTERS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeriesId = table.Column<int>(type: "int", nullable: false),
                    PeriodKey = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CurrentNumber = table.Column<int>(type: "int", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DOCUMENT_SERIES_COUNTERS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DOCUMENT_SERIES_COUNTERS_DOCUMENT_SERIES_SeriesId",
                        column: x => x.SeriesId,
                        principalTable: "DOCUMENT_SERIES",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DOCUMENT_SERIES_COUNTERS_SeriesId_PeriodKey",
                table: "DOCUMENT_SERIES_COUNTERS",
                columns: new[] { "SeriesId", "PeriodKey" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DOCUMENT_NUMBER_AUDIT");

            migrationBuilder.DropTable(
                name: "DOCUMENT_SERIES_COUNTERS");

            migrationBuilder.DropColumn(
                name: "GenerateOn",
                table: "DOCUMENT_SERIES");

            migrationBuilder.DropColumn(
                name: "ResetPolicy",
                table: "DOCUMENT_SERIES");

            migrationBuilder.DropColumn(
                name: "SequencePadding",
                table: "DOCUMENT_SERIES");

            migrationBuilder.DropColumn(
                name: "SequenceStart",
                table: "DOCUMENT_SERIES");

            migrationBuilder.DropColumn(
                name: "SeriesName",
                table: "DOCUMENT_SERIES");

            migrationBuilder.DropColumn(
                name: "Template",
                table: "DOCUMENT_SERIES");
        }
    }
}
