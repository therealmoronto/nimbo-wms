using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nimbo.Wms.Migrations
{
    /// <inheritdoc />
    public partial class ReplaceBatchWithVendorLotAndStockLot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_inventory_items_batches_BatchId",
                schema: "nimbo",
                table: "inventory_items");

            migrationBuilder.DropTable(
                name: "batches",
                schema: "nimbo");

            migrationBuilder.DropIndex(
                name: "IX_inventory_items_BatchId",
                schema: "nimbo",
                table: "inventory_items");

            migrationBuilder.DropColumn(
                name: "BatchId",
                schema: "nimbo",
                table: "inventory_items");

            migrationBuilder.AddColumn<Guid>(
                name: "StockLotId",
                schema: "nimbo",
                table: "stock_ledger_entries",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "StockLotId",
                schema: "nimbo",
                table: "shipment_pick_lines",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "StockLotId",
                schema: "nimbo",
                table: "shipment_document_lines",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "StockLotId",
                schema: "nimbo",
                table: "relocation_document_lines",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BatchNumber",
                schema: "nimbo",
                table: "receiving_document_lines",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiryDate",
                schema: "nimbo",
                table: "receiving_document_lines",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsBatchManaged",
                schema: "nimbo",
                table: "items",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "StockLotId",
                schema: "nimbo",
                table: "inventory_items",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "StockLotId",
                schema: "nimbo",
                table: "cycle_count_document_lines",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "StockLotId",
                schema: "nimbo",
                table: "adjustment_document_lines",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "vendor_lots",
                schema: "nimbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    BatchNumber = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    SupplierId = table.Column<Guid>(type: "uuid", nullable: true),
                    ExpiryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vendor_lots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_vendor_lots_items_ItemId",
                        column: x => x.ItemId,
                        principalSchema: "nimbo",
                        principalTable: "items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_vendor_lots_suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalSchema: "nimbo",
                        principalTable: "suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "stock_lots",
                schema: "nimbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    ReceivingDocumentId = table.Column<Guid>(type: "uuid", nullable: false),
                    ReceivedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    VendorLotId = table.Column<Guid>(type: "uuid", nullable: true),
                    UnitCost = table.Column<decimal>(type: "numeric(18,4)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stock_lots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_stock_lots_items_ItemId",
                        column: x => x.ItemId,
                        principalSchema: "nimbo",
                        principalTable: "items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_stock_lots_receiving_documents_ReceivingDocumentId",
                        column: x => x.ReceivingDocumentId,
                        principalSchema: "nimbo",
                        principalTable: "receiving_documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_stock_lots_vendor_lots_VendorLotId",
                        column: x => x.VendorLotId,
                        principalSchema: "nimbo",
                        principalTable: "vendor_lots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_shipment_document_lines_StockLotId",
                schema: "nimbo",
                table: "shipment_document_lines",
                column: "StockLotId");

            migrationBuilder.CreateIndex(
                name: "IX_relocation_document_lines_StockLotId",
                schema: "nimbo",
                table: "relocation_document_lines",
                column: "StockLotId");

            migrationBuilder.CreateIndex(
                name: "IX_inventory_items_ItemId_StockLotId_LocationId",
                schema: "nimbo",
                table: "inventory_items",
                columns: new[] { "ItemId", "StockLotId", "LocationId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_inventory_items_StockLotId",
                schema: "nimbo",
                table: "inventory_items",
                column: "StockLotId");

            migrationBuilder.CreateIndex(
                name: "IX_cycle_count_document_lines_StockLotId",
                schema: "nimbo",
                table: "cycle_count_document_lines",
                column: "StockLotId");

            migrationBuilder.CreateIndex(
                name: "IX_adjustment_document_lines_StockLotId",
                schema: "nimbo",
                table: "adjustment_document_lines",
                column: "StockLotId");

            migrationBuilder.CreateIndex(
                name: "IX_stock_lots_ItemId_ReceivedAt",
                schema: "nimbo",
                table: "stock_lots",
                columns: new[] { "ItemId", "ReceivedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_stock_lots_ReceivingDocumentId",
                schema: "nimbo",
                table: "stock_lots",
                column: "ReceivingDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_stock_lots_VendorLotId",
                schema: "nimbo",
                table: "stock_lots",
                column: "VendorLotId");

            migrationBuilder.CreateIndex(
                name: "IX_vendor_lots_ExpiryDate",
                schema: "nimbo",
                table: "vendor_lots",
                column: "ExpiryDate");

            migrationBuilder.CreateIndex(
                name: "ix_vendor_lots_item_batch_expiry_only",
                schema: "nimbo",
                table: "vendor_lots",
                columns: new[] { "ItemId", "BatchNumber", "ExpiryDate" },
                unique: true,
                filter: "\"SupplierId\" IS NULL AND \"ExpiryDate\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "ix_vendor_lots_item_batch_only",
                schema: "nimbo",
                table: "vendor_lots",
                columns: new[] { "ItemId", "BatchNumber" },
                unique: true,
                filter: "\"SupplierId\" IS NULL AND \"ExpiryDate\" IS NULL");

            migrationBuilder.CreateIndex(
                name: "ix_vendor_lots_item_batch_supplier_expiry",
                schema: "nimbo",
                table: "vendor_lots",
                columns: new[] { "ItemId", "BatchNumber", "SupplierId", "ExpiryDate" },
                unique: true,
                filter: "\"SupplierId\" IS NOT NULL AND \"ExpiryDate\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "ix_vendor_lots_item_batch_supplier_only",
                schema: "nimbo",
                table: "vendor_lots",
                columns: new[] { "ItemId", "BatchNumber", "SupplierId" },
                unique: true,
                filter: "\"SupplierId\" IS NOT NULL AND \"ExpiryDate\" IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_vendor_lots_SupplierId",
                schema: "nimbo",
                table: "vendor_lots",
                column: "SupplierId");

            migrationBuilder.AddForeignKey(
                name: "FK_adjustment_document_lines_stock_lots_StockLotId",
                schema: "nimbo",
                table: "adjustment_document_lines",
                column: "StockLotId",
                principalSchema: "nimbo",
                principalTable: "stock_lots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_cycle_count_document_lines_stock_lots_StockLotId",
                schema: "nimbo",
                table: "cycle_count_document_lines",
                column: "StockLotId",
                principalSchema: "nimbo",
                principalTable: "stock_lots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_inventory_items_stock_lots_StockLotId",
                schema: "nimbo",
                table: "inventory_items",
                column: "StockLotId",
                principalSchema: "nimbo",
                principalTable: "stock_lots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_relocation_document_lines_stock_lots_StockLotId",
                schema: "nimbo",
                table: "relocation_document_lines",
                column: "StockLotId",
                principalSchema: "nimbo",
                principalTable: "stock_lots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_shipment_document_lines_stock_lots_StockLotId",
                schema: "nimbo",
                table: "shipment_document_lines",
                column: "StockLotId",
                principalSchema: "nimbo",
                principalTable: "stock_lots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_adjustment_document_lines_stock_lots_StockLotId",
                schema: "nimbo",
                table: "adjustment_document_lines");

            migrationBuilder.DropForeignKey(
                name: "FK_cycle_count_document_lines_stock_lots_StockLotId",
                schema: "nimbo",
                table: "cycle_count_document_lines");

            migrationBuilder.DropForeignKey(
                name: "FK_inventory_items_stock_lots_StockLotId",
                schema: "nimbo",
                table: "inventory_items");

            migrationBuilder.DropForeignKey(
                name: "FK_relocation_document_lines_stock_lots_StockLotId",
                schema: "nimbo",
                table: "relocation_document_lines");

            migrationBuilder.DropForeignKey(
                name: "FK_shipment_document_lines_stock_lots_StockLotId",
                schema: "nimbo",
                table: "shipment_document_lines");

            migrationBuilder.DropTable(
                name: "stock_lots",
                schema: "nimbo");

            migrationBuilder.DropTable(
                name: "vendor_lots",
                schema: "nimbo");

            migrationBuilder.DropIndex(
                name: "IX_shipment_document_lines_StockLotId",
                schema: "nimbo",
                table: "shipment_document_lines");

            migrationBuilder.DropIndex(
                name: "IX_relocation_document_lines_StockLotId",
                schema: "nimbo",
                table: "relocation_document_lines");

            migrationBuilder.DropIndex(
                name: "IX_inventory_items_ItemId_StockLotId_LocationId",
                schema: "nimbo",
                table: "inventory_items");

            migrationBuilder.DropIndex(
                name: "IX_inventory_items_StockLotId",
                schema: "nimbo",
                table: "inventory_items");

            migrationBuilder.DropIndex(
                name: "IX_cycle_count_document_lines_StockLotId",
                schema: "nimbo",
                table: "cycle_count_document_lines");

            migrationBuilder.DropIndex(
                name: "IX_adjustment_document_lines_StockLotId",
                schema: "nimbo",
                table: "adjustment_document_lines");

            migrationBuilder.DropColumn(
                name: "StockLotId",
                schema: "nimbo",
                table: "stock_ledger_entries");

            migrationBuilder.DropColumn(
                name: "StockLotId",
                schema: "nimbo",
                table: "shipment_pick_lines");

            migrationBuilder.DropColumn(
                name: "StockLotId",
                schema: "nimbo",
                table: "shipment_document_lines");

            migrationBuilder.DropColumn(
                name: "StockLotId",
                schema: "nimbo",
                table: "relocation_document_lines");

            migrationBuilder.DropColumn(
                name: "BatchNumber",
                schema: "nimbo",
                table: "receiving_document_lines");

            migrationBuilder.DropColumn(
                name: "ExpiryDate",
                schema: "nimbo",
                table: "receiving_document_lines");

            migrationBuilder.DropColumn(
                name: "IsBatchManaged",
                schema: "nimbo",
                table: "items");

            migrationBuilder.DropColumn(
                name: "StockLotId",
                schema: "nimbo",
                table: "inventory_items");

            migrationBuilder.DropColumn(
                name: "StockLotId",
                schema: "nimbo",
                table: "cycle_count_document_lines");

            migrationBuilder.DropColumn(
                name: "StockLotId",
                schema: "nimbo",
                table: "adjustment_document_lines");

            migrationBuilder.AddColumn<Guid>(
                name: "BatchId",
                schema: "nimbo",
                table: "inventory_items",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "batches",
                schema: "nimbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BatchNumber = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    ManufacturedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Notes = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    ReceivedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SupplierId = table.Column<Guid>(type: "uuid", nullable: true)
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

            migrationBuilder.CreateIndex(
                name: "IX_inventory_items_BatchId",
                schema: "nimbo",
                table: "inventory_items",
                column: "BatchId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_inventory_items_batches_BatchId",
                schema: "nimbo",
                table: "inventory_items",
                column: "BatchId",
                principalSchema: "nimbo",
                principalTable: "batches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
