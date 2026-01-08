using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nimbo.Wms.Migrations
{
    /// <inheritdoc />
    public partial class Add_ShipmentOrder_Configuration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "shipment_orders",
                schema: "nimbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WarehouseId = table.Column<Guid>(type: "uuid", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: false),
                    ShippedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CancelledAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CancelReason = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    ExternalReference = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shipment_orders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "shipment_order_lines",
                schema: "nimbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ShipmentOrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderedQuantity = table.Column<decimal>(type: "numeric(8,0)", nullable: false),
                    ReservedQuantity = table.Column<decimal>(type: "numeric(8,0)", nullable: false),
                    PickedQuantity = table.Column<decimal>(type: "numeric(8,0)", nullable: false),
                    UomCode = table.Column<int>(type: "integer", maxLength: 16, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shipment_order_lines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_shipment_order_lines_shipment_orders_ShipmentOrderId",
                        column: x => x.ShipmentOrderId,
                        principalSchema: "nimbo",
                        principalTable: "shipment_orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_shipment_order_lines_ItemId",
                schema: "nimbo",
                table: "shipment_order_lines",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_shipment_order_lines_ShipmentOrderId",
                schema: "nimbo",
                table: "shipment_order_lines",
                column: "ShipmentOrderId");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "shipment_order_lines",
                schema: "nimbo");

            migrationBuilder.DropTable(
                name: "shipment_orders",
                schema: "nimbo");
        }
    }
}
