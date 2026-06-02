using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Store.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class AddUserSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "FullName", "InsertTime", "IsActive", "IsRemoved", "Password", "RemovedTime", "UpdateTime" },
                values: new object[,]
                {
                    { 1L, "digitroadmin@gmail.com", "Admin", new DateTime(2026, 6, 2, 20, 36, 0, 599, DateTimeKind.Local).AddTicks(1775), true, false, "$2a$11$gxT6FO8RsSCghxHU9UW7pulcjTYwrM4AFFnRxK7ftQhK7bkfKiXEq", null, null },
                    { 2L, "digitrooperator@gmail.com", "Admin", new DateTime(2026, 6, 2, 20, 36, 0, 880, DateTimeKind.Local).AddTicks(8483), true, false, "$2a$11$EBeRTRXX3RU6wODFCBYK1ef4oe8VnoanJu7WdduRELAJZkUH/aC1G", null, null }
                });

            migrationBuilder.InsertData(
                table: "UserInRoles",
                columns: new[] { "Id", "RoleId", "UserId" },
                values: new object[,]
                {
                    { 1L, 1L, 1L },
                    { 2L, 2L, 2L }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserInRoles",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "UserInRoles",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2L);
        }
    }
}
