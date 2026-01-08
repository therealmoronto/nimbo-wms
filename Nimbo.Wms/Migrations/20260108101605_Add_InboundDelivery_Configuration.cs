using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nimbo.Wms.Migrations
{
    /// <inheritdoc />
    public partial class Add_InboundDelivery_Configuration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "inbound_deliveries",
                schema: "nimbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SupplierId = table.Column<Guid>(type: "uuid", nullable: false),
                    WarehouseId = table.Column<Guid>(type: "uuid", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ReceivedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    ExternalReference = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inbound_deliveries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "inbound_delivery_lines",
                schema: "nimbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    InboundDeliveryId = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    ExpectedQuantity = table.Column<decimal>(type: "numeric(8,0)", nullable: false),
                    Uom = table.Column<int>(type: "integer", maxLength: 16, nullable: false),
                    ReceivedQuantity = table.Column<decimal>(type: "numeric(8,0)", nullable: true),
                    BatchNumber = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    ExpiryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inbound_delivery_lines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_inbound_delivery_lines_inbound_deliveries_InboundDeliveryId",
                        column: x => x.InboundDeliveryId,
                        principalSchema: "nimbo",
                        principalTable: "inbound_deliveries",
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
                name: "IX_inbound_delivery_lines_InboundDeliveryId",
                schema: "nimbo",
                table: "inbound_delivery_lines",
                column: "InboundDeliveryId");

            migrationBuilder.CreateIndex(
                name: "IX_inbound_delivery_lines_ItemId",
                schema: "nimbo",
                table: "inbound_delivery_lines",
                column: "ItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "inbound_delivery_lines",
                schema: "nimbo");

            migrationBuilder.DropTable(
                name: "inbound_deliveries",
                schema: "nimbo");
        }
    }
}
