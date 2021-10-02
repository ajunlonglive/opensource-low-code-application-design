using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class DMReferenceInDV : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_DCAppDataDefinitions_RefDataModelId",
                table: "DCAppDataDefinitions",
                column: "RefDataModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_DCAppDataDefinitions_DAppDataModels_RefDataModelId",
                table: "DCAppDataDefinitions",
                column: "RefDataModelId",
                principalTable: "DAppDataModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DCAppDataDefinitions_DAppDataModels_RefDataModelId",
                table: "DCAppDataDefinitions");

            migrationBuilder.DropIndex(
                name: "IX_DCAppDataDefinitions_RefDataModelId",
                table: "DCAppDataDefinitions");
        }
    }
}
