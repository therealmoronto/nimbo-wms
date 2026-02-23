using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nimbo.Wms.Migrations
{
    /// <inheritdoc />
    public partial class Documents_DeletedAllDocumentsClassesAndConfigs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "inbound_delivery_lines",
                schema: "nimbo");

            migrationBuilder.DropTable(
                name: "internal_transfers",
                schema: "nimbo");

            migrationBuilder.DropTable(
                name: "inventory_count_lines",
                schema: "nimbo");

            migrationBuilder.DropTable(
                name: "shipment_order_lines",
                schema: "nimbo");

            migrationBuilder.DropTable(
                name: "transfer_order_lines",
                schema: "nimbo");

            migrationBuilder.DropTable(
                name: "inbound_deliveries",
                schema: "nimbo");

            migrationBuilder.DropTable(
                name: "inventory_counts",
                schema: "nimbo");

            migrationBuilder.DropTable(
                name: "shipment_orders",
                schema: "nimbo");

            migrationBuilder.DropTable(
                name: "transfer_orders",
                schema: "nimbo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "inbound_deliveries",
                schema: "nimbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExternalReference = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    Name = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    ReceivedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    StartedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    SupplierId = table.Column<Guid>(type: "uuid", nullable: false),
                    WarehouseId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inbound_deliveries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "internal_transfers",
                schema: "nimbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FromLocationId = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    OccurredAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Reason = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ToLocationId = table.Column<Guid>(type: "uuid", nullable: false),
                    WarehouseId = table.Column<Guid>(type: "uuid", nullable: false),
                    quantity_uom = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    quantity_amount = table.Column<decimal>(type: "numeric(18,3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_internal_transfers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "inventory_counts",
                schema: "nimbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ClosedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Code = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExternalReference = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    Name = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    StartedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    WarehouseId = table.Column<Guid>(type: "uuid", nullable: false),
                    ZoneId = table.Column<Guid>(type: "uuid", nullable: true),
                    location_scope = table.Column<Guid[]>(type: "uuid[]", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inventory_counts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "shipment_orders",
                schema: "nimbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CancelReason = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    CancelledAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Code = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: false),
                    ExternalReference = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    Name = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    ShippedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    WarehouseId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shipment_orders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "transfer_orders",
                schema: "nimbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExternalReference = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    FromWarehouseId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    PickingStartedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ReceivedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ShippedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    ToWarehouseId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transfer_orders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "inbound_delivery_lines",
                schema: "nimbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BatchNumber = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    DocumentId = table.Column<Guid>(type: "uuid", nullable: false),
                    ExpectedQuantity = table.Column<decimal>(type: "numeric(8,0)", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    ReceivedQuantity = table.Column<decimal>(type: "numeric(8,0)", nullable: true),
                    Uom = table.Column<int>(type: "integer", maxLength: 16, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inbound_delivery_lines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_inbound_delivery_lines_inbound_deliveries_DocumentId",
                        column: x => x.DocumentId,
                        principalSchema: "nimbo",
                        principalTable: "inbound_deliveries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "inventory_count_lines",
                schema: "nimbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DocumentId = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    LocationId = table.Column<Guid>(type: "uuid", nullable: false),
                    counted_qty_uom = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: true),
                    counted_qty_amount = table.Column<decimal>(type: "numeric(18,3)", nullable: true),
                    system_qty_uom = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    system_qty_amount = table.Column<decimal>(type: "numeric(18,3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inventory_count_lines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_inventory_count_lines_inventory_counts_DocumentId",
                        column: x => x.DocumentId,
                        principalSchema: "nimbo",
                        principalTable: "inventory_counts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "shipment_order_lines",
                schema: "nimbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DocumentId = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderedQuantity = table.Column<decimal>(type: "numeric(8,0)", nullable: false),
                    PickedQuantity = table.Column<decimal>(type: "numeric(8,0)", nullable: false),
                    ReservedQuantity = table.Column<decimal>(type: "numeric(8,0)", nullable: false),
                    UomCode = table.Column<int>(type: "integer", maxLength: 16, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shipment_order_lines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_shipment_order_lines_shipment_orders_DocumentId",
                        column: x => x.DocumentId,
                        principalSchema: "nimbo",
                        principalTable: "shipment_orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "transfer_order_lines",
                schema: "nimbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DocumentId = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    picked_qty_uom = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    picked_qty_amount = table.Column<decimal>(type: "numeric(18,3)", nullable: false),
                    planned_qty_uom = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    planned_qty_amount = table.Column<decimal>(type: "numeric(18,3)", nullable: false),
                    received_qty_uom = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    received_qty_amount = table.Column<decimal>(type: "numeric(18,3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transfer_order_lines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_transfer_order_lines_transfer_orders_DocumentId",
                        column: x => x.DocumentId,
                        principalSchema: "nimbo",
                        principalTable: "transfer_orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_inbound_deliveries_CreatedAt",
                schema: "nimbo",
                table: "inbound_deliveries",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_inbound_deliveries_Status",
                schema: "nimbo",
                table: "inbound_deliveries",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_inbound_deliveries_SupplierId",
                schema: "nimbo",
                table: "inbound_deliveries",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_inbound_deliveries_WarehouseId",
                schema: "nimbo",
                table: "inbound_deliveries",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_inbound_delivery_lines_DocumentId",
                schema: "nimbo",
                table: "inbound_delivery_lines",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_inbound_delivery_lines_ItemId",
                schema: "nimbo",
                table: "inbound_delivery_lines",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_inventory_count_lines_DocumentId",
                schema: "nimbo",
                table: "inventory_count_lines",
                column: "DocumentId");

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

            migrationBuilder.CreateIndex(
                name: "IX_shipment_order_lines_DocumentId",
                schema: "nimbo",
                table: "shipment_order_lines",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_shipment_order_lines_ItemId",
                schema: "nimbo",
                table: "shipment_order_lines",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_shipment_orders_CreatedAt",
                schema: "nimbo",
                table: "shipment_orders",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_shipment_orders_CustomerId",
                schema: "nimbo",
                table: "shipment_orders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_shipment_orders_Status",
                schema: "nimbo",
                table: "shipment_orders",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_shipment_orders_WarehouseId",
                schema: "nimbo",
                table: "shipment_orders",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_transfer_order_lines_DocumentId",
                schema: "nimbo",
                table: "transfer_order_lines",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_transfer_order_lines_ItemId",
                schema: "nimbo",
                table: "transfer_order_lines",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_transfer_orders_CreatedAt",
                schema: "nimbo",
                table: "transfer_orders",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_transfer_orders_FromWarehouseId",
                schema: "nimbo",
                table: "transfer_orders",
                column: "FromWarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_transfer_orders_Status",
                schema: "nimbo",
                table: "transfer_orders",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_transfer_orders_ToWarehouseId",
                schema: "nimbo",
                table: "transfer_orders",
                column: "ToWarehouseId");
        }
    }
}
