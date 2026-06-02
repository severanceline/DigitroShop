using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class EditOperatorName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "InsertTime", "Password" },
                values: new object[] { new DateTime(2026, 6, 2, 20, 43, 20, 22, DateTimeKind.Local).AddTicks(6511), "$2a$11$Fy5q0V7T2e5peffFnO2ZFeYMqmcixBcYLFpz6OE1gAX6z9CT/b2xG" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "FullName", "InsertTime", "Password" },
                values: new object[] { "Opertor", new DateTime(2026, 6, 2, 20, 43, 20, 259, DateTimeKind.Local).AddTicks(1176), "$2a$11$hRLkFc.CKajia.y/hSC9fOKac2aE1Q7u3KuqvZNf2Vt/nl5qxIlnC" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "InsertTime", "Password" },
                values: new object[] { new DateTime(2026, 6, 2, 20, 36, 0, 599, DateTimeKind.Local).AddTicks(1775), "$2a$11$gxT6FO8RsSCghxHU9UW7pulcjTYwrM4AFFnRxK7ftQhK7bkfKiXEq" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "FullName", "InsertTime", "Password" },
                values: new object[] { "Admin", new DateTime(2026, 6, 2, 20, 36, 0, 880, DateTimeKind.Local).AddTicks(8483), "$2a$11$EBeRTRXX3RU6wODFCBYK1ef4oe8VnoanJu7WdduRELAJZkUH/aC1G" });
        }
    }
}
