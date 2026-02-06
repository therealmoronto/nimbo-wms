using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nimbo.Wms.Migrations
{
    /// <inheritdoc />
    public partial class Warehouse_UpdateRelationsWithZonesAndLocations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_locations_warehouses_WarehouseId",
                schema: "nimbo",
                table: "locations");

            migrationBuilder.DropForeignKey(
                name: "FK_locations_zones_ZoneId",
                schema: "nimbo",
                table: "locations");

            migrationBuilder.DropForeignKey(
                name: "FK_zones_warehouses_WarehouseId",
                schema: "nimbo",
                table: "zones");

            migrationBuilder.AddForeignKey(
                name: "FK_locations_warehouses_WarehouseId",
                schema: "nimbo",
                table: "locations",
                column: "WarehouseId",
                principalSchema: "nimbo",
                principalTable: "warehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_zones_warehouses_WarehouseId",
                schema: "nimbo",
                table: "zones",
                column: "WarehouseId",
                principalSchema: "nimbo",
                principalTable: "warehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_locations_warehouses_WarehouseId",
                schema: "nimbo",
                table: "locations");

            migrationBuilder.DropForeignKey(
                name: "FK_zones_warehouses_WarehouseId",
                schema: "nimbo",
                table: "zones");

            migrationBuilder.AddForeignKey(
                name: "FK_locations_warehouses_WarehouseId",
                schema: "nimbo",
                table: "locations",
                column: "WarehouseId",
                principalSchema: "nimbo",
                principalTable: "warehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_locations_zones_ZoneId",
                schema: "nimbo",
                table: "locations",
                column: "ZoneId",
                principalSchema: "nimbo",
                principalTable: "zones",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_zones_warehouses_WarehouseId",
                schema: "nimbo",
                table: "zones",
                column: "WarehouseId",
                principalSchema: "nimbo",
                principalTable: "warehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
