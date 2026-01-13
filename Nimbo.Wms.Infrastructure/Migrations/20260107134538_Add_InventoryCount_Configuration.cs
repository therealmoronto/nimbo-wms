using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nimbo.Wms.Migrations
{
    /// <inheritdoc />
    public partial class Add_InventoryCount_Configuration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "inventory_counts",
                schema: "nimbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WarehouseId = table.Column<Guid>(type: "uuid", nullable: false),
                    ZoneId = table.Column<Guid>(type: "uuid", nullable: true),
                    StartedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ClosedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    location_scope = table.Column<Guid[]>(type: "uuid[]", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    ExternalReference = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inventory_counts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "inventory_count_lines",
                schema: "nimbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    InventoryCountId = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    LocationId = table.Column<Guid>(type: "uuid", nullable: false),
                    system_qty_amount = table.Column<decimal>(type: "numeric(18,3)", nullable: false),
                    system_qty_uom = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    counted_qty_amount = table.Column<decimal>(type: "numeric(18,3)", nullable: true),
                    counted_qty_uom = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inventory_count_lines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_inventory_count_lines_inventory_counts_InventoryCountId",
                        column: x => x.InventoryCountId,
                        principalSchema: "nimbo",
                        principalTable: "inventory_counts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_inventory_count_lines_InventoryCountId",
                schema: "nimbo",
                table: "inventory_count_lines",
                column: "InventoryCountId");

            migrationBuilder.CreateIndex(
                name: "IX_inventory_count_lines_ItemId",
                schema: "nimbo",
                table: "inventory_count_lines",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_inventory_count_lines_LocationId",
                schema: "nimbo",
                table: "inventory_count_lines",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_inventory_counts_CreatedAt",
                schema: "nimbo",
                table: "inventory_counts",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_inventory_counts_Status",
                schema: "nimbo",
                table: "inventory_counts",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_inventory_counts_WarehouseId",
                schema: "nimbo",
                table: "inventory_counts",
                column: "WarehouseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "inventory_count_lines",
                schema: "nimbo");

            migrationBuilder.DropTable(
                name: "inventory_counts",
                schema: "nimbo");
        }
    }
}
