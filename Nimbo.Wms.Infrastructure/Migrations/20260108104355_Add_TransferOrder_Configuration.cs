using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nimbo.Wms.Migrations
{
    /// <inheritdoc />
    public partial class Add_TransferOrder_Configuration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "transfer_orders",
                schema: "nimbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FromWarehouseId = table.Column<Guid>(type: "uuid", nullable: false),
                    ToWarehouseId = table.Column<Guid>(type: "uuid", nullable: false),
                    PickingStartedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ShippedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ReceivedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    ExternalReference = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transfer_orders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "transfer_order_lines",
                schema: "nimbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TransferOrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    planned_qty_amount = table.Column<decimal>(type: "numeric(18,3)", nullable: false),
                    planned_qty_uom = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    picked_qty_amount = table.Column<decimal>(type: "numeric(18,3)", nullable: false),
                    picked_qty_uom = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    received_qty_amount = table.Column<decimal>(type: "numeric(18,3)", nullable: false),
                    received_qty_uom = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transfer_order_lines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_transfer_order_lines_transfer_orders_TransferOrderId",
                        column: x => x.TransferOrderId,
                        principalSchema: "nimbo",
                        principalTable: "transfer_orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_transfer_order_lines_ItemId",
                schema: "nimbo",
                table: "transfer_order_lines",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_transfer_order_lines_TransferOrderId",
                schema: "nimbo",
                table: "transfer_order_lines",
                column: "TransferOrderId");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "transfer_order_lines",
                schema: "nimbo");

            migrationBuilder.DropTable(
                name: "transfer_orders",
                schema: "nimbo");
        }
    }
}
