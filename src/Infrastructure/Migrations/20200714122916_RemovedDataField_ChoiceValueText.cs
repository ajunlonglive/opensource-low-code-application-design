using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class RemovedDataField_ChoiceValueText : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChoiceDisplayText",
                table: "DAppDataChoiceItems");

            migrationBuilder.DropColumn(
                name: "ChoiceValue",
                table: "DAppDataChoiceItems");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ChoiceDisplayText",
                table: "DAppDataChoiceItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ChoiceValue",
                table: "DAppDataChoiceItems",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
