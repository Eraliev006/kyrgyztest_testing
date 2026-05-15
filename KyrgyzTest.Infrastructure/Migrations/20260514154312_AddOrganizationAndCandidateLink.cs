using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KyrgyzTest.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddOrganizationAndCandidateLink : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "OrganizationId",
                table: "Candidates",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Organizations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    NameRu = table.Column<string>(type: "text", nullable: false),
                    NameKg = table.Column<string>(type: "text", nullable: false),
                    ShortName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizations", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 5, 14, 15, 43, 12, 286, DateTimeKind.Utc).AddTicks(7530), "$2a$11$cnf2AE5JrUaICoFPCvJVwujnekIlUnP2tYU5ru0IwblRmYNY75lSW" });

            migrationBuilder.CreateIndex(
                name: "IX_Candidates_OrganizationId",
                table: "Candidates",
                column: "OrganizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Candidates_Organizations_OrganizationId",
                table: "Candidates",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Candidates_Organizations_OrganizationId",
                table: "Candidates");

            migrationBuilder.DropTable(
                name: "Organizations");

            migrationBuilder.DropIndex(
                name: "IX_Candidates_OrganizationId",
                table: "Candidates");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "Candidates");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 5, 3, 18, 27, 55, 25, DateTimeKind.Utc).AddTicks(3220), "$2a$11$qmA8dkeuyEKj8ALDXOKcWu1WSYBuxpLxXoyzGQfDiRcQIP8CIqhDW" });
        }
    }
}
