using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nimbo.Wms.Migrations
{
    /// <inheritdoc />
    public partial class Ledger_AddLedgerEntryClass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "stock_ledger_entries",
                schema: "nimbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    InventoryItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    LocationId = table.Column<Guid>(type: "uuid", nullable: false),
                    WarehouseId = table.Column<Guid>(type: "uuid", nullable: false),
                    quantity_delta_amount = table.Column<decimal>(type: "numeric(18,3)", nullable: false),
                    quantity_delta_uom = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    balance_after_amount = table.Column<decimal>(type: "numeric(18,3)", nullable: false),
                    balance_after_uom = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    SourceDocumentId = table.Column<Guid>(type: "uuid", nullable: false),
                    SourceDocumentLineId = table.Column<Guid>(type: "uuid", nullable: false),
                    TransactionType = table.Column<string>(type: "text", nullable: false),
                    OccurredAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stock_ledger_entries", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_stock_ledger_entries_InventoryItemId_OccurredAt",
                schema: "nimbo",
                table: "stock_ledger_entries",
                columns: new[] { "InventoryItemId", "OccurredAt" });

            migrationBuilder.CreateIndex(
                name: "IX_stock_ledger_entries_ItemId_LocationId",
                schema: "nimbo",
                table: "stock_ledger_entries",
                columns: new[] { "ItemId", "LocationId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "stock_ledger_entries",
                schema: "nimbo");
        }
    }
}
