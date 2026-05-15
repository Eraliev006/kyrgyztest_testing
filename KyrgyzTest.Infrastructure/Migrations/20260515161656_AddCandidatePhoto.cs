using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KyrgyzTest.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCandidatePhoto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Photo",
                table: "Candidates",
                type: "text",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 5, 15, 16, 16, 55, 744, DateTimeKind.Utc).AddTicks(3330), "$2a$11$J1novLhoDgpmwy78C2eWfeY7wUHTTj8CWEhgtsDG8pEkScyo/0CRS" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Photo",
                table: "Candidates");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 5, 15, 15, 59, 52, 753, DateTimeKind.Utc).AddTicks(6290), "$2a$11$dwt0sjUp1vqYYpNYoqCLMu1jn0Sxp0pdm0fs0P5Jon6FYrld8yliG" });
        }
    }
}
