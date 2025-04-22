using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cantina.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AuditFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "MenuItems",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Timestamp",
                table: "MenuAudit",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "e2d35ad5-21b1-40ff-976d-f38c4df46f2c",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8ce184cf-4a11-4389-a184-f5476f9c09f5", "AQAAAAIAAYagAAAAEKMF2sgsUSi2Ib2g6MDT5g6Cz0t2mqBE3d5m+cxaGM4GwM2IJKu45oR4Q0jFaO3NyA==", "6c396980-f6cb-4bb5-98df-a50faeaa731b" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "MenuItems",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Timestamp",
                table: "MenuAudit",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETDATE()");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "e2d35ad5-21b1-40ff-976d-f38c4df46f2c",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "4ddccac9-a37b-4efc-93b0-e7b8aeafb4db", "AQAAAAIAAYagAAAAEBu/wWZFUGLNjGNeYSBqZteQYQGSKXd+KM3ZK6WYNLKNlJ4rMvdlH9R7vDIWpsEmuw==", "70853252-cd24-4b45-9fc9-04595fe964e3" });
        }
    }
}
