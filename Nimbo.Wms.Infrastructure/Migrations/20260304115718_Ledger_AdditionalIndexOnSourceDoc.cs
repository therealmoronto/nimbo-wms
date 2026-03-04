using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nimbo.Wms.Migrations
{
    /// <inheritdoc />
    public partial class Ledger_AdditionalIndexOnSourceDoc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_stock_ledger_entries_SourceDocumentId",
                schema: "nimbo",
                table: "stock_ledger_entries",
                column: "SourceDocumentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_stock_ledger_entries_SourceDocumentId",
                schema: "nimbo",
                table: "stock_ledger_entries");
        }
    }
}
