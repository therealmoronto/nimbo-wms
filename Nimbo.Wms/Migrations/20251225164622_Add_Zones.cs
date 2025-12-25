using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nimbo.Wms.Migrations
{
    /// <inheritdoc />
    public partial class Add_Zones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "zones",
                schema: "nimbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WarehouseId = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    MaxWeightKg = table.Column<decimal>(type: "numeric", nullable: true),
                    MaxVolumeM3 = table.Column<decimal>(type: "numeric", nullable: true),
                    IsQuarantine = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    IsDamagedArea = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_zones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_zones_warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalSchema: "nimbo",
                        principalTable: "warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_zones_WarehouseId_Code",
                schema: "nimbo",
                table: "zones",
                columns: new[] { "WarehouseId", "Code" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "zones",
                schema: "nimbo");
        }
    }
}
