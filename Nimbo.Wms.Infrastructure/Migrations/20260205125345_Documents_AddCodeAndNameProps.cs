using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nimbo.Wms.Migrations
{
    /// <inheritdoc />
    public partial class Documents_AddCodeAndNameProps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                schema: "nimbo",
                table: "transfer_orders",
                type: "character varying(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "nimbo",
                table: "transfer_orders",
                type: "character varying(512)",
                maxLength: 512,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                schema: "nimbo",
                table: "shipment_orders",
                type: "character varying(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "nimbo",
                table: "shipment_orders",
                type: "character varying(512)",
                maxLength: 512,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                schema: "nimbo",
                table: "inventory_counts",
                type: "character varying(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "nimbo",
                table: "inventory_counts",
                type: "character varying(512)",
                maxLength: 512,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                schema: "nimbo",
                table: "inbound_deliveries",
                type: "character varying(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "nimbo",
                table: "inbound_deliveries",
                type: "character varying(512)",
                maxLength: 512,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                schema: "nimbo",
                table: "transfer_orders");

            migrationBuilder.DropColumn(
                name: "Name",
                schema: "nimbo",
                table: "transfer_orders");

            migrationBuilder.DropColumn(
                name: "Code",
                schema: "nimbo",
                table: "shipment_orders");

            migrationBuilder.DropColumn(
                name: "Name",
                schema: "nimbo",
                table: "shipment_orders");

            migrationBuilder.DropColumn(
                name: "Code",
                schema: "nimbo",
                table: "inventory_counts");

            migrationBuilder.DropColumn(
                name: "Name",
                schema: "nimbo",
                table: "inventory_counts");

            migrationBuilder.DropColumn(
                name: "Code",
                schema: "nimbo",
                table: "inbound_deliveries");

            migrationBuilder.DropColumn(
                name: "Name",
                schema: "nimbo",
                table: "inbound_deliveries");
        }
    }
}
