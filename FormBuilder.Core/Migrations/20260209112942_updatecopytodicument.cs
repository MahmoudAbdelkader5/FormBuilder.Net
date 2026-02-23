using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FormBuilder.Core.Migrations
{
    /// <inheritdoc />
    public partial class updatecopytodicument : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_COPY_TO_DOCUMENT_AUDIT_FORM_SUBMISSIONS_SourceSubmissionId",
                table: "COPY_TO_DOCUMENT_AUDIT");

            migrationBuilder.DropIndex(
                name: "IX_COPY_TO_DOCUMENT_AUDIT_SourceSubmissionId",
                table: "COPY_TO_DOCUMENT_AUDIT");

            migrationBuilder.DropColumn(
                name: "SourceSubmissionId",
                table: "COPY_TO_DOCUMENT_AUDIT");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SourceSubmissionId",
                table: "COPY_TO_DOCUMENT_AUDIT",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_COPY_TO_DOCUMENT_AUDIT_SourceSubmissionId",
                table: "COPY_TO_DOCUMENT_AUDIT",
                column: "SourceSubmissionId");

            migrationBuilder.AddForeignKey(
                name: "FK_COPY_TO_DOCUMENT_AUDIT_FORM_SUBMISSIONS_SourceSubmissionId",
                table: "COPY_TO_DOCUMENT_AUDIT",
                column: "SourceSubmissionId",
                principalTable: "FORM_SUBMISSIONS",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
