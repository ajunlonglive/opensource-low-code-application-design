using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class DefinedDataViews : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReferenceRowIds",
                table: "DAppDataValues");

            migrationBuilder.AddColumn<Guid>(
                name: "BaseDataModelId",
                table: "DAppDataValues",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MultipleReferenceRowIds",
                table: "DAppDataValues",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SingleReferenceRowId",
                table: "DAppDataValues",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_DAppDataValues_BaseDataModelId",
                table: "DAppDataValues",
                column: "BaseDataModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_DAppDataValues_DAppDataModels_BaseDataModelId",
                table: "DAppDataValues",
                column: "BaseDataModelId",
                principalTable: "DAppDataModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DAppDataValues_DAppDataModels_BaseDataModelId",
                table: "DAppDataValues");

            migrationBuilder.DropIndex(
                name: "IX_DAppDataValues_BaseDataModelId",
                table: "DAppDataValues");

            migrationBuilder.DropColumn(
                name: "BaseDataModelId",
                table: "DAppDataValues");

            migrationBuilder.DropColumn(
                name: "MultipleReferenceRowIds",
                table: "DAppDataValues");

            migrationBuilder.DropColumn(
                name: "SingleReferenceRowId",
                table: "DAppDataValues");

            migrationBuilder.AddColumn<string>(
                name: "ReferenceRowIds",
                table: "DAppDataValues",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
