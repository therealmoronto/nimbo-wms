using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nimbo.Wms.Migrations
{
    /// <inheritdoc />
    public partial class Shipment_RefactorArchitectureToStorePickedLocations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "shipped_quantity_amount",
                schema: "nimbo",
                table: "shipment_document_lines");

            migrationBuilder.DropColumn(
                name: "shipped_quantity_uom",
                schema: "nimbo",
                table: "shipment_document_lines");

            migrationBuilder.CreateTable(
                name: "shipment_pick_lines",
                schema: "nimbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DocumentId = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    FromLocation = table.Column<Guid>(type: "uuid", nullable: false),
                    picked_quantity_amount = table.Column<decimal>(type: "numeric(18,3)", nullable: false),
                    picked_quantity_uom = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    Notes = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shipment_pick_lines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_shipment_pick_lines_shipment_documents_DocumentId",
                        column: x => x.DocumentId,
                        principalSchema: "nimbo",
                        principalTable: "shipment_documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_shipment_pick_lines_DocumentId",
                schema: "nimbo",
                table: "shipment_pick_lines",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_shipment_pick_lines_DocumentId_ItemId_FromLocation",
                schema: "nimbo",
                table: "shipment_pick_lines",
                columns: new[] { "DocumentId", "ItemId", "FromLocation" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "shipment_pick_lines",
                schema: "nimbo");

            migrationBuilder.AddColumn<decimal>(
                name: "shipped_quantity_amount",
                schema: "nimbo",
                table: "shipment_document_lines",
                type: "numeric(18,3)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "shipped_quantity_uom",
                schema: "nimbo",
                table: "shipment_document_lines",
                type: "character varying(16)",
                maxLength: 16,
                nullable: true);
        }
    }
}
