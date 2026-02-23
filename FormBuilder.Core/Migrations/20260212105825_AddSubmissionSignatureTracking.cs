using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FormBuilder.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddSubmissionSignatureTracking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DocuSignEnvelopeId",
                table: "FORM_SUBMISSIONS",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SignatureStatus",
                table: "FORM_SUBMISSIONS",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "not_required");

            migrationBuilder.AddColumn<DateTime>(
                name: "SignedAt",
                table: "FORM_SUBMISSIONS",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DocuSignEnvelopeId",
                table: "FORM_SUBMISSIONS");

            migrationBuilder.DropColumn(
                name: "SignatureStatus",
                table: "FORM_SUBMISSIONS");

            migrationBuilder.DropColumn(
                name: "SignedAt",
                table: "FORM_SUBMISSIONS");
        }
    }
}
