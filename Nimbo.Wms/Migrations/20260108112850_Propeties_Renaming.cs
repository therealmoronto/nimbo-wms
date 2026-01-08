using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nimbo.Wms.Migrations
{
    /// <inheritdoc />
    public partial class Propeties_Renaming : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_inbound_delivery_lines_inbound_deliveries_InboundDeliveryId",
                schema: "nimbo",
                table: "inbound_delivery_lines");

            migrationBuilder.DropForeignKey(
                name: "FK_inventory_count_lines_inventory_counts_InventoryCountId",
                schema: "nimbo",
                table: "inventory_count_lines");

            migrationBuilder.DropForeignKey(
                name: "FK_shipment_order_lines_shipment_orders_ShipmentOrderId",
                schema: "nimbo",
                table: "shipment_order_lines");

            migrationBuilder.DropForeignKey(
                name: "FK_transfer_order_lines_transfer_orders_TransferOrderId",
                schema: "nimbo",
                table: "transfer_order_lines");

            migrationBuilder.RenameColumn(
                name: "TransferOrderId",
                schema: "nimbo",
                table: "transfer_order_lines",
                newName: "DocumentId");

            migrationBuilder.RenameIndex(
                name: "IX_transfer_order_lines_TransferOrderId",
                schema: "nimbo",
                table: "transfer_order_lines",
                newName: "IX_transfer_order_lines_DocumentId");

            migrationBuilder.RenameColumn(
                name: "ShipmentOrderId",
                schema: "nimbo",
                table: "shipment_order_lines",
                newName: "DocumentId");

            migrationBuilder.RenameIndex(
                name: "IX_shipment_order_lines_ShipmentOrderId",
                schema: "nimbo",
                table: "shipment_order_lines",
                newName: "IX_shipment_order_lines_DocumentId");

            migrationBuilder.RenameColumn(
                name: "InventoryCountId",
                schema: "nimbo",
                table: "inventory_count_lines",
                newName: "DocumentId");

            migrationBuilder.RenameIndex(
                name: "IX_inventory_count_lines_InventoryCountId",
                schema: "nimbo",
                table: "inventory_count_lines",
                newName: "IX_inventory_count_lines_DocumentId");

            migrationBuilder.RenameColumn(
                name: "InboundDeliveryId",
                schema: "nimbo",
                table: "inbound_delivery_lines",
                newName: "DocumentId");

            migrationBuilder.RenameIndex(
                name: "IX_inbound_delivery_lines_InboundDeliveryId",
                schema: "nimbo",
                table: "inbound_delivery_lines",
                newName: "IX_inbound_delivery_lines_DocumentId");

            migrationBuilder.AddForeignKey(
                name: "FK_inbound_delivery_lines_inbound_deliveries_DocumentId",
                schema: "nimbo",
                table: "inbound_delivery_lines",
                column: "DocumentId",
                principalSchema: "nimbo",
                principalTable: "inbound_deliveries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_inventory_count_lines_inventory_counts_DocumentId",
                schema: "nimbo",
                table: "inventory_count_lines",
                column: "DocumentId",
                principalSchema: "nimbo",
                principalTable: "inventory_counts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_shipment_order_lines_shipment_orders_DocumentId",
                schema: "nimbo",
                table: "shipment_order_lines",
                column: "DocumentId",
                principalSchema: "nimbo",
                principalTable: "shipment_orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_transfer_order_lines_transfer_orders_DocumentId",
                schema: "nimbo",
                table: "transfer_order_lines",
                column: "DocumentId",
                principalSchema: "nimbo",
                principalTable: "transfer_orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_inbound_delivery_lines_inbound_deliveries_DocumentId",
                schema: "nimbo",
                table: "inbound_delivery_lines");

            migrationBuilder.DropForeignKey(
                name: "FK_inventory_count_lines_inventory_counts_DocumentId",
                schema: "nimbo",
                table: "inventory_count_lines");

            migrationBuilder.DropForeignKey(
                name: "FK_shipment_order_lines_shipment_orders_DocumentId",
                schema: "nimbo",
                table: "shipment_order_lines");

            migrationBuilder.DropForeignKey(
                name: "FK_transfer_order_lines_transfer_orders_DocumentId",
                schema: "nimbo",
                table: "transfer_order_lines");

            migrationBuilder.RenameColumn(
                name: "DocumentId",
                schema: "nimbo",
                table: "transfer_order_lines",
                newName: "TransferOrderId");

            migrationBuilder.RenameIndex(
                name: "IX_transfer_order_lines_DocumentId",
                schema: "nimbo",
                table: "transfer_order_lines",
                newName: "IX_transfer_order_lines_TransferOrderId");

            migrationBuilder.RenameColumn(
                name: "DocumentId",
                schema: "nimbo",
                table: "shipment_order_lines",
                newName: "ShipmentOrderId");

            migrationBuilder.RenameIndex(
                name: "IX_shipment_order_lines_DocumentId",
                schema: "nimbo",
                table: "shipment_order_lines",
                newName: "IX_shipment_order_lines_ShipmentOrderId");

            migrationBuilder.RenameColumn(
                name: "DocumentId",
                schema: "nimbo",
                table: "inventory_count_lines",
                newName: "InventoryCountId");

            migrationBuilder.RenameIndex(
                name: "IX_inventory_count_lines_DocumentId",
                schema: "nimbo",
                table: "inventory_count_lines",
                newName: "IX_inventory_count_lines_InventoryCountId");

            migrationBuilder.RenameColumn(
                name: "DocumentId",
                schema: "nimbo",
                table: "inbound_delivery_lines",
                newName: "InboundDeliveryId");

            migrationBuilder.RenameIndex(
                name: "IX_inbound_delivery_lines_DocumentId",
                schema: "nimbo",
                table: "inbound_delivery_lines",
                newName: "IX_inbound_delivery_lines_InboundDeliveryId");

            migrationBuilder.AddForeignKey(
                name: "FK_inbound_delivery_lines_inbound_deliveries_InboundDeliveryId",
                schema: "nimbo",
                table: "inbound_delivery_lines",
                column: "InboundDeliveryId",
                principalSchema: "nimbo",
                principalTable: "inbound_deliveries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_inventory_count_lines_inventory_counts_InventoryCountId",
                schema: "nimbo",
                table: "inventory_count_lines",
                column: "InventoryCountId",
                principalSchema: "nimbo",
                principalTable: "inventory_counts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_shipment_order_lines_shipment_orders_ShipmentOrderId",
                schema: "nimbo",
                table: "shipment_order_lines",
                column: "ShipmentOrderId",
                principalSchema: "nimbo",
                principalTable: "shipment_orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_transfer_order_lines_transfer_orders_TransferOrderId",
                schema: "nimbo",
                table: "transfer_order_lines",
                column: "TransferOrderId",
                principalSchema: "nimbo",
                principalTable: "transfer_orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
