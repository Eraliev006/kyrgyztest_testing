using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KyrgyzTest.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCompletedSection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CompletedSections",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AttemptId = table.Column<Guid>(type: "uuid", nullable: false),
                    Section = table.Column<int>(type: "integer", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompletedSections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompletedSections_Attempts_AttemptId",
                        column: x => x.AttemptId,
                        principalTable: "Attempts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 5, 3, 18, 27, 55, 25, DateTimeKind.Utc).AddTicks(3220), "$2a$11$qmA8dkeuyEKj8ALDXOKcWu1WSYBuxpLxXoyzGQfDiRcQIP8CIqhDW" });

            migrationBuilder.CreateIndex(
                name: "IX_CompletedSections_AttemptId",
                table: "CompletedSections",
                column: "AttemptId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompletedSections");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 4, 21, 10, 29, 13, 641, DateTimeKind.Utc).AddTicks(2610), "$2a$11$QqZ7zPG9TttEGJQO4o5Ty.1Z3N2F1OWl44lhr9Iy.u8ZAe1svlC6C" });
        }
    }
}
