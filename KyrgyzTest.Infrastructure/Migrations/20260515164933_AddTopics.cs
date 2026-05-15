using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace KyrgyzTest.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTopics : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TopicId",
                table: "Questions",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Topics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    SectionType = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Topics", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Topics",
                columns: new[] { "Id", "Name", "SectionType" },
                values: new object[,]
                {
                    { new Guid("a0000001-0000-0000-0000-000000000001"), "Зат атооч", 0 },
                    { new Guid("a0000001-0000-0000-0000-000000000002"), "Сын атооч", 0 },
                    { new Guid("a0000001-0000-0000-0000-000000000003"), "Этиш", 0 },
                    { new Guid("a0000001-0000-0000-0000-000000000004"), "Ат атооч", 0 },
                    { new Guid("a0000001-0000-0000-0000-000000000005"), "Сан атооч", 0 },
                    { new Guid("a0000001-0000-0000-0000-000000000006"), "Тактооч", 0 },
                    { new Guid("a0000001-0000-0000-0000-000000000007"), "Байламта", 0 },
                    { new Guid("a0000001-0000-0000-0000-000000000008"), "Жалгоо", 0 }
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 5, 15, 16, 49, 32, 737, DateTimeKind.Utc).AddTicks(120), "$2a$11$hZgUzWM6xbFhFpLHRyjA4OZkyhZjNCiqXWzTmP5Yv3qvOhMo3KpAq" });

            migrationBuilder.CreateIndex(
                name: "IX_Questions_TopicId",
                table: "Questions",
                column: "TopicId");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Topics_TopicId",
                table: "Questions",
                column: "TopicId",
                principalTable: "Topics",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Topics_TopicId",
                table: "Questions");

            migrationBuilder.DropTable(
                name: "Topics");

            migrationBuilder.DropIndex(
                name: "IX_Questions_TopicId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "TopicId",
                table: "Questions");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 5, 15, 16, 43, 19, 535, DateTimeKind.Utc).AddTicks(5170), "$2a$11$b0t2niWhNpQsmz0J/0XgIeRPh7DHqgcvlCNw2KHoUT9jTFA0bzZLu" });
        }
    }
}
