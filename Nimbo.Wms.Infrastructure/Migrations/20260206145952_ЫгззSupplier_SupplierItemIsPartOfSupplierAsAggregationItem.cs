using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nimbo.Wms.Migrations
{
    /// <inheritdoc />
    public partial class ЫгззSupplier_SupplierItemIsPartOfSupplierAsAggregationItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_supplier_items_SupplierId",
                schema: "nimbo",
                table: "supplier_items");

            migrationBuilder.CreateIndex(
                name: "IX_supplier_items_SupplierId_ItemId",
                schema: "nimbo",
                table: "supplier_items",
                columns: new[] { "SupplierId", "ItemId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_supplier_items_SupplierId_ItemId",
                schema: "nimbo",
                table: "supplier_items");

            migrationBuilder.CreateIndex(
                name: "IX_supplier_items_SupplierId",
                schema: "nimbo",
                table: "supplier_items",
                column: "SupplierId");
        }
    }
}
