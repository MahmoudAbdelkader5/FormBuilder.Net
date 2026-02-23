using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FormBuilder.Core.Migrations
{
    public partial class AddSapIntegrationHttpMethod : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HttpMethod",
                table: "SAP_INTEGRATION_SETTINGS",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "POST");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HttpMethod",
                table: "SAP_INTEGRATION_SETTINGS");
        }
    }
}

