using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FormBuilder.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddFormBuilderSapExecutionMode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SapExecutionMode",
                table: "FORM_BUILDER",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                defaultValue: "OnSubmit");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SapExecutionMode",
                table: "FORM_BUILDER");
        }
    }
}
