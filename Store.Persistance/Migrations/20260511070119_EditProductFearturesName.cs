using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class EditProductFearturesName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductFetures_Products_ProductId",
                table: "ProductFetures");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductFetures",
                table: "ProductFetures");

            migrationBuilder.RenameTable(
                name: "ProductFetures",
                newName: "ProductFeatures");

            migrationBuilder.RenameIndex(
                name: "IX_ProductFetures_ProductId",
                table: "ProductFeatures",
                newName: "IX_ProductFeatures_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductFeatures",
                table: "ProductFeatures",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductFeatures_Products_ProductId",
                table: "ProductFeatures",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductFeatures_Products_ProductId",
                table: "ProductFeatures");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductFeatures",
                table: "ProductFeatures");

            migrationBuilder.RenameTable(
                name: "ProductFeatures",
                newName: "ProductFetures");

            migrationBuilder.RenameIndex(
                name: "IX_ProductFeatures_ProductId",
                table: "ProductFetures",
                newName: "IX_ProductFetures_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductFetures",
                table: "ProductFetures",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductFetures_Products_ProductId",
                table: "ProductFetures",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
