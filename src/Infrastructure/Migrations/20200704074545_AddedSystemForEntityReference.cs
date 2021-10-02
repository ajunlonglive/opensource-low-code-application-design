using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class AddedSystemForEntityReference : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSystemBuilt",
                table: "DCAppDataDefinitions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SystemTableName",
                table: "DCAppDataDefinitions",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSystemBuilt",
                table: "DCAppDataDefinitions");

            migrationBuilder.DropColumn(
                name: "SystemTableName",
                table: "DCAppDataDefinitions");
        }
    }
}
