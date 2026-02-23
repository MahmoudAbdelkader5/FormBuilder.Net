using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FormBuilder.Core.Migrations
{
    /// <inheritdoc />
    public partial class RemoveDocumentTypeFromDocumentSeries : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DOCUMENT_SERIES_DOCUMENT_TYPES_DocumentTypeId",
                table: "DOCUMENT_SERIES");

            migrationBuilder.DropIndex(
                name: "IX_DOCUMENT_SERIES_DocumentTypeId",
                table: "DOCUMENT_SERIES");

            migrationBuilder.DropColumn(
                name: "DocumentTypeId",
                table: "DOCUMENT_SERIES");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DocumentTypeId",
                table: "DOCUMENT_SERIES",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_DOCUMENT_SERIES_DocumentTypeId",
                table: "DOCUMENT_SERIES",
                column: "DocumentTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_DOCUMENT_SERIES_DOCUMENT_TYPES_DocumentTypeId",
                table: "DOCUMENT_SERIES",
                column: "DocumentTypeId",
                principalTable: "DOCUMENT_TYPES",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
