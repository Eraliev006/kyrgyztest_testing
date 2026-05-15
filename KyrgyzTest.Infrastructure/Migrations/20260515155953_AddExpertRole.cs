using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KyrgyzTest.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddExpertRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 5, 15, 15, 59, 52, 753, DateTimeKind.Utc).AddTicks(6290), "$2a$11$dwt0sjUp1vqYYpNYoqCLMu1jn0Sxp0pdm0fs0P5Jon6FYrld8yliG" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 5, 14, 15, 43, 12, 286, DateTimeKind.Utc).AddTicks(7530), "$2a$11$cnf2AE5JrUaICoFPCvJVwujnekIlUnP2tYU5ru0IwblRmYNY75lSW" });
        }
    }
}
