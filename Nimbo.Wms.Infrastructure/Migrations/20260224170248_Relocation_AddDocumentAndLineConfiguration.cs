using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nimbo.Wms.Migrations
{
    /// <inheritdoc />
    public partial class Relocation_AddDocumentAndLineConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "relocation_documents",
                schema: "nimbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WarehouseId = table.Column<Guid>(type: "uuid", nullable: false),
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
                    table.PrimaryKey("PK_relocation_documents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "relocation_document_lines",
                schema: "nimbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    From = table.Column<Guid>(type: "uuid", nullable: false),
                    To = table.Column<Guid>(type: "uuid", nullable: false),
                    DocumentId = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    quantity_amount = table.Column<decimal>(type: "numeric(18,3)", nullable: false),
                    quantity_uom = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    Notes = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_relocation_document_lines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_relocation_document_lines_items_ItemId",
                        column: x => x.ItemId,
                        principalSchema: "nimbo",
                        principalTable: "items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_relocation_document_lines_locations_From",
                        column: x => x.From,
                        principalSchema: "nimbo",
                        principalTable: "locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_relocation_document_lines_locations_To",
                        column: x => x.To,
                        principalSchema: "nimbo",
                        principalTable: "locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_relocation_document_lines_relocation_documents_DocumentId",
                        column: x => x.DocumentId,
                        principalSchema: "nimbo",
                        principalTable: "relocation_documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_relocation_document_lines_DocumentId",
                schema: "nimbo",
                table: "relocation_document_lines",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_relocation_document_lines_DocumentId_ItemId",
                schema: "nimbo",
                table: "relocation_document_lines",
                columns: new[] { "DocumentId", "ItemId" });

            migrationBuilder.CreateIndex(
                name: "IX_relocation_document_lines_DocumentId_ItemId_From_To",
                schema: "nimbo",
                table: "relocation_document_lines",
                columns: new[] { "DocumentId", "ItemId", "From", "To" });

            migrationBuilder.CreateIndex(
                name: "IX_relocation_document_lines_From",
                schema: "nimbo",
                table: "relocation_document_lines",
                column: "From");

            migrationBuilder.CreateIndex(
                name: "IX_relocation_document_lines_ItemId",
                schema: "nimbo",
                table: "relocation_document_lines",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_relocation_document_lines_To",
                schema: "nimbo",
                table: "relocation_document_lines",
                column: "To");

            migrationBuilder.CreateIndex(
                name: "IX_relocation_documents_Code",
                schema: "nimbo",
                table: "relocation_documents",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_relocation_documents_CreatedAt",
                schema: "nimbo",
                table: "relocation_documents",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_relocation_documents_Status",
                schema: "nimbo",
                table: "relocation_documents",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_relocation_documents_WarehouseId",
                schema: "nimbo",
                table: "relocation_documents",
                column: "WarehouseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "relocation_document_lines",
                schema: "nimbo");

            migrationBuilder.DropTable(
                name: "relocation_documents",
                schema: "nimbo");
        }
    }
}
