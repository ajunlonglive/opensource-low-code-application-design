using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class BaseDataField_DataValue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BaseDataFieldId",
                table: "DAppDataValues",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DAppDataValues_BaseDataFieldId",
                table: "DAppDataValues",
                column: "BaseDataFieldId");

            migrationBuilder.AddForeignKey(
                name: "FK_DAppDataValues_DAppDataFields_BaseDataFieldId",
                table: "DAppDataValues",
                column: "BaseDataFieldId",
                principalTable: "DAppDataFields",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DAppDataValues_DAppDataFields_BaseDataFieldId",
                table: "DAppDataValues");

            migrationBuilder.DropIndex(
                name: "IX_DAppDataValues_BaseDataFieldId",
                table: "DAppDataValues");

            migrationBuilder.DropColumn(
                name: "BaseDataFieldId",
                table: "DAppDataValues");
        }
    }
}
