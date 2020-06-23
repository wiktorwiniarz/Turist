using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace EquipmentRentalCore.Migrations
{
    public partial class DatabaseFixes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Equipments_ListEquipmentTypeViewModel_ListEquipmentTypeViewModelId",
                table: "Equipments");

            migrationBuilder.DropTable(
                name: "ListEquipmentTypeViewModel");

            migrationBuilder.DropIndex(
                name: "IX_Equipments_ListEquipmentTypeViewModelId",
                table: "Equipments");

            migrationBuilder.DropColumn(
                name: "ListEquipmentTypeViewModelId",
                table: "Equipments");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ListEquipmentTypeViewModelId",
                table: "Equipments",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ListEquipmentTypeViewModel",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListEquipmentTypeViewModel", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Equipments_ListEquipmentTypeViewModelId",
                table: "Equipments",
                column: "ListEquipmentTypeViewModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Equipments_ListEquipmentTypeViewModel_ListEquipmentTypeViewModelId",
                table: "Equipments",
                column: "ListEquipmentTypeViewModelId",
                principalTable: "ListEquipmentTypeViewModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
