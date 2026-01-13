using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nimbo.Wms.Migrations
{
    /// <inheritdoc />
    public partial class Add_Locations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "locations",
                schema: "nimbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WarehouseId = table.Column<Guid>(type: "uuid", nullable: false),
                    ZoneId = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Aisle = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: true),
                    Rack = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: true),
                    Level = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: true),
                    Position = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: true),
                    Type = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    MaxWeightKg = table.Column<decimal>(type: "numeric", nullable: true),
                    MaxVolumeM3 = table.Column<decimal>(type: "numeric", nullable: true),
                    IsSingleItemOnly = table.Column<bool>(type: "boolean", nullable: false),
                    IsPickingLocation = table.Column<bool>(type: "boolean", nullable: false),
                    IsReceivingLocation = table.Column<bool>(type: "boolean", nullable: false),
                    IsShippingLocation = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsBlocked = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_locations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_locations_warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalSchema: "nimbo",
                        principalTable: "warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_locations_zones_ZoneId",
                        column: x => x.ZoneId,
                        principalSchema: "nimbo",
                        principalTable: "zones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_locations_IsActive",
                schema: "nimbo",
                table: "locations",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_locations_WarehouseId_Code",
                schema: "nimbo",
                table: "locations",
                columns: new[] { "WarehouseId", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_locations_ZoneId",
                schema: "nimbo",
                table: "locations",
                column: "ZoneId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "locations",
                schema: "nimbo");
        }
    }
}
