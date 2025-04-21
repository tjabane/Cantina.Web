using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Cantina.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdminUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0fb3a09d-320e-43b2-9a3f-bc1a3597aefc", null, "Admin", "ADMIN" },
                    { "4901a6a2-7630-4573-a2c2-b98acbe3c276", null, "Member", "MEMBER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Discriminator", "Email", "EmailConfirmed", "FullName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "e2d35ad5-21b1-40ff-976d-f38c4df46f2c", 0, "18c4af11-41ab-4d17-a466-66cee8e95159", "ApplicationUser", "admin@cantina.com", true, "Cantina Admin", false, null, "ADMIN@CANTINA.COM", "ADMIN@CANTINA.COM", "AQAAAAIAAYagAAAAEL6OK202AYsOJW1DmoFueGcH4H5fOU1OpdzcuALzUdFY2KH8TeneptreZ7ZuF+hPNQ==", null, false, "ae6939b8-3fd0-40bf-864f-7c017a49a810", false, "admin@cantina.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "0fb3a09d-320e-43b2-9a3f-bc1a3597aefc", "e2d35ad5-21b1-40ff-976d-f38c4df46f2c" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4901a6a2-7630-4573-a2c2-b98acbe3c276");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "0fb3a09d-320e-43b2-9a3f-bc1a3597aefc", "e2d35ad5-21b1-40ff-976d-f38c4df46f2c" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0fb3a09d-320e-43b2-9a3f-bc1a3597aefc");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "e2d35ad5-21b1-40ff-976d-f38c4df46f2c");
        }
    }
}
