using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nimbo.Wms.Migrations
{
    /// <inheritdoc />
    public partial class Shipment_AddDocumentAndLineConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "shipment_documents",
                schema: "nimbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WarehouseId = table.Column<Guid>(type: "uuid", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: true),
                    Code = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Title = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Status = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PostedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    Notes = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shipment_documents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "shipment_document_lines",
                schema: "nimbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    shipped_quantity_amount = table.Column<decimal>(type: "numeric(18,3)", nullable: true),
                    shipped_quantity_uom = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: true),
                    DocumentId = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    requested_quantity_amount = table.Column<decimal>(type: "numeric(18,3)", nullable: false),
                    requested_quantity_uom = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    Notes = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shipment_document_lines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_shipment_document_lines_items_ItemId",
                        column: x => x.ItemId,
                        principalSchema: "nimbo",
                        principalTable: "items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_shipment_document_lines_shipment_documents_DocumentId",
                        column: x => x.DocumentId,
                        principalSchema: "nimbo",
                        principalTable: "shipment_documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_shipment_document_lines_DocumentId",
                schema: "nimbo",
                table: "shipment_document_lines",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_shipment_document_lines_DocumentId_ItemId",
                schema: "nimbo",
                table: "shipment_document_lines",
                columns: new[] { "DocumentId", "ItemId" });

            migrationBuilder.CreateIndex(
                name: "IX_shipment_document_lines_ItemId",
                schema: "nimbo",
                table: "shipment_document_lines",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_shipment_documents_Code",
                schema: "nimbo",
                table: "shipment_documents",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_shipment_documents_CreatedAt",
                schema: "nimbo",
                table: "shipment_documents",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_shipment_documents_Status",
                schema: "nimbo",
                table: "shipment_documents",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_shipment_documents_WarehouseId",
                schema: "nimbo",
                table: "shipment_documents",
                column: "WarehouseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "shipment_document_lines",
                schema: "nimbo");

            migrationBuilder.DropTable(
                name: "shipment_documents",
                schema: "nimbo");
        }
    }
}
