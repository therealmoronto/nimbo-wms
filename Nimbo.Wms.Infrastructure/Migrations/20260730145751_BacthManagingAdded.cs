using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nimbo.Wms.Migrations
{
    /// <inheritdoc />
    public partial class BacthManagingAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BatchId",
                schema: "nimbo",
                table: "stock_ledger_entries",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "BatchId",
                schema: "nimbo",
                table: "shipment_pick_lines",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "BatchId",
                schema: "nimbo",
                table: "shipment_document_lines",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "BatchId",
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
                name: "BatchId",
                schema: "nimbo",
                table: "cycle_count_document_lines",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "BatchId",
                schema: "nimbo",
                table: "adjustment_document_lines",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BatchId",
                schema: "nimbo",
                table: "stock_ledger_entries");

            migrationBuilder.DropColumn(
                name: "BatchId",
                schema: "nimbo",
                table: "shipment_pick_lines");

            migrationBuilder.DropColumn(
                name: "BatchId",
                schema: "nimbo",
                table: "shipment_document_lines");

            migrationBuilder.DropColumn(
                name: "BatchId",
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
                name: "BatchId",
                schema: "nimbo",
                table: "cycle_count_document_lines");

            migrationBuilder.DropColumn(
                name: "BatchId",
                schema: "nimbo",
                table: "adjustment_document_lines");
        }
    }
}
