using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nimbo.Wms.Migrations
{
    /// <inheritdoc />
    public partial class Documents_ReceivingExpectedQuantityIsOptional : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "expected_quantity_uom",
                schema: "nimbo",
                table: "receiving_document_lines",
                type: "character varying(16)",
                maxLength: 16,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(16)",
                oldMaxLength: 16);

            migrationBuilder.AlterColumn<decimal>(
                name: "expected_quantity_amount",
                schema: "nimbo",
                table: "receiving_document_lines",
                type: "numeric(18,3)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,3)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "expected_quantity_uom",
                schema: "nimbo",
                table: "receiving_document_lines",
                type: "character varying(16)",
                maxLength: 16,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(16)",
                oldMaxLength: 16,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "expected_quantity_amount",
                schema: "nimbo",
                table: "receiving_document_lines",
                type: "numeric(18,3)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,3)",
                oldNullable: true);
        }
    }
}
