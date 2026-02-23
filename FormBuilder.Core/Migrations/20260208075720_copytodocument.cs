using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FormBuilder.Core.Migrations
{
    /// <inheritdoc />
    public partial class copytodocument : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParentDocumentId",
                table: "FORM_SUBMISSIONS",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FORM_SUBMISSIONS_ParentDocumentId",
                table: "FORM_SUBMISSIONS",
                column: "ParentDocumentId");

            migrationBuilder.AddForeignKey(
                name: "FK_FORM_SUBMISSIONS_FORM_SUBMISSIONS_ParentDocumentId",
                table: "FORM_SUBMISSIONS",
                column: "ParentDocumentId",
                principalTable: "FORM_SUBMISSIONS",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FORM_SUBMISSIONS_FORM_SUBMISSIONS_ParentDocumentId",
                table: "FORM_SUBMISSIONS");

            migrationBuilder.DropIndex(
                name: "IX_FORM_SUBMISSIONS_ParentDocumentId",
                table: "FORM_SUBMISSIONS");

            migrationBuilder.DropColumn(
                name: "ParentDocumentId",
                table: "FORM_SUBMISSIONS");
        }
    }
}
