using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class EditRelationBetweenCartAndRequestPay : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CartId",
                table: "RequestPays",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_RequestPays_CartId",
                table: "RequestPays",
                column: "CartId");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestPays_Carts_CartId",
                table: "RequestPays",
                column: "CartId",
                principalTable: "Carts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestPays_Carts_CartId",
                table: "RequestPays");

            migrationBuilder.DropIndex(
                name: "IX_RequestPays_CartId",
                table: "RequestPays");

            migrationBuilder.DropColumn(
                name: "CartId",
                table: "RequestPays");
        }
    }
}
