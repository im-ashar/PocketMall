using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PocketMall.Migrations
{
    /// <inheritdoc />
    public partial class Second : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Quantity",
                columns: table => new
                {
                    QuantityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuantityOfProduct = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quantity", x => x.QuantityId);
                    table.ForeignKey(
                        name: "FK_Quantity_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Quantity_ProductId",
                table: "Quantity",
                column: "ProductId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Quantity");
        }
    }
}
