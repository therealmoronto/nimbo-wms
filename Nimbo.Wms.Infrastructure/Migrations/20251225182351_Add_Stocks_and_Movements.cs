using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nimbo.Wms.Migrations
{
    /// <inheritdoc />
    public partial class Add_Stocks_and_Movements : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Type",
                schema: "nimbo",
                table: "zones",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldDefaultValue: 1);

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                schema: "nimbo",
                table: "locations",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldDefaultValue: 1);

            migrationBuilder.CreateTable(
                name: "batches",
                schema: "nimbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    BatchNumber = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    SupplierId = table.Column<Guid>(type: "uuid", nullable: true),
                    ManufacturedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ExpiryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ReceivedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Notes = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_batches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_batches_items_ItemId",
                        column: x => x.ItemId,
                        principalSchema: "nimbo",
                        principalTable: "items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_batches_suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalSchema: "nimbo",
                        principalTable: "suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "internal_transfers",
                schema: "nimbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WarehouseId = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    FromLocationId = table.Column<Guid>(type: "uuid", nullable: false),
                    ToLocationId = table.Column<Guid>(type: "uuid", nullable: false),
                    quantity_amount = table.Column<decimal>(type: "numeric(18,3)", nullable: false),
                    quantity_uom = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    OccurredAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Reason = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_internal_transfers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "inventory_items",
                schema: "nimbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    WarehouseId = table.Column<Guid>(type: "uuid", nullable: false),
                    LocationId = table.Column<Guid>(type: "uuid", nullable: false),
                    quantity_amount = table.Column<decimal>(type: "numeric(18,3)", nullable: false),
                    quantity_uom = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    BatchId = table.Column<Guid>(type: "uuid", nullable: true),
                    SerialNumber = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    Status = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    UnitCost = table.Column<decimal>(type: "numeric(18,4)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inventory_items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_inventory_items_batches_BatchId",
                        column: x => x.BatchId,
                        principalSchema: "nimbo",
                        principalTable: "batches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_inventory_items_items_ItemId",
                        column: x => x.ItemId,
                        principalSchema: "nimbo",
                        principalTable: "items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_inventory_items_locations_LocationId",
                        column: x => x.LocationId,
                        principalSchema: "nimbo",
                        principalTable: "locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_inventory_items_warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalSchema: "nimbo",
                        principalTable: "warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_batches_ExpiryDate",
                schema: "nimbo",
                table: "batches",
                column: "ExpiryDate");

            migrationBuilder.CreateIndex(
                name: "IX_batches_ItemId_BatchNumber",
                schema: "nimbo",
                table: "batches",
                columns: new[] { "ItemId", "BatchNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_batches_SupplierId",
                schema: "nimbo",
                table: "batches",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_inventory_items_BatchId",
                schema: "nimbo",
                table: "inventory_items",
                column: "BatchId");

            migrationBuilder.CreateIndex(
                name: "IX_inventory_items_ItemId_WarehouseId_LocationId",
                schema: "nimbo",
                table: "inventory_items",
                columns: new[] { "ItemId", "WarehouseId", "LocationId" });

            migrationBuilder.CreateIndex(
                name: "IX_inventory_items_LocationId",
                schema: "nimbo",
                table: "inventory_items",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_inventory_items_Status",
                schema: "nimbo",
                table: "inventory_items",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_inventory_items_WarehouseId",
                schema: "nimbo",
                table: "inventory_items",
                column: "WarehouseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "internal_transfers",
                schema: "nimbo");

            migrationBuilder.DropTable(
                name: "inventory_items",
                schema: "nimbo");

            migrationBuilder.DropTable(
                name: "batches",
                schema: "nimbo");

            migrationBuilder.AlterColumn<int>(
                name: "Type",
                schema: "nimbo",
                table: "zones",
                type: "integer",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "Type",
                schema: "nimbo",
                table: "locations",
                type: "integer",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
