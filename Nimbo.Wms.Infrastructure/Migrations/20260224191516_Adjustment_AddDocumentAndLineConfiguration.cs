using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nimbo.Wms.Migrations
{
    /// <inheritdoc />
    public partial class Adjustment_AddDocumentAndLineConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "adjustment_documents",
                schema: "nimbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WarehouseId = table.Column<Guid>(type: "uuid", nullable: false),
                    ReasonCode = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    ReasonText = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
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
                    table.PrimaryKey("PK_adjustment_documents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "adjustment_document_lines",
                schema: "nimbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LocationId = table.Column<Guid>(type: "uuid", nullable: false),
                    delta_amount = table.Column<decimal>(type: "numeric(18,3)", nullable: false),
                    delta_uom = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    DocumentId = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    Notes = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_adjustment_document_lines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_adjustment_document_lines_adjustment_documents_DocumentId",
                        column: x => x.DocumentId,
                        principalSchema: "nimbo",
                        principalTable: "adjustment_documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_adjustment_document_lines_DocumentId",
                schema: "nimbo",
                table: "adjustment_document_lines",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_adjustment_document_lines_DocumentId_ItemId_LocationId",
                schema: "nimbo",
                table: "adjustment_document_lines",
                columns: new[] { "DocumentId", "ItemId", "LocationId" });

            migrationBuilder.CreateIndex(
                name: "IX_adjustment_documents_Code",
                schema: "nimbo",
                table: "adjustment_documents",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_adjustment_documents_CreatedAt",
                schema: "nimbo",
                table: "adjustment_documents",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_adjustment_documents_Status",
                schema: "nimbo",
                table: "adjustment_documents",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_adjustment_documents_WarehouseId",
                schema: "nimbo",
                table: "adjustment_documents",
                column: "WarehouseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "adjustment_document_lines",
                schema: "nimbo");

            migrationBuilder.DropTable(
                name: "adjustment_documents",
                schema: "nimbo");
        }
    }
}
