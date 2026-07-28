using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KyrgyzTest.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddManualGradeAndSpeaking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SpeakingScore",
                table: "Results",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "AudioAnswerUrl",
                table: "CandidateAnswers",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ManualGrades",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AttemptId = table.Column<Guid>(type: "uuid", nullable: false),
                    QuestionId = table.Column<Guid>(type: "uuid", nullable: false),
                    Score = table.Column<int>(type: "integer", nullable: false),
                    GradedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    GradedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ManualGrades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ManualGrades_Attempts_AttemptId",
                        column: x => x.AttemptId,
                        principalTable: "Attempts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ManualGrades_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ManualGrades_Users_GradedByUserId",
                        column: x => x.GradedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ManualGrades_AttemptId_QuestionId",
                table: "ManualGrades",
                columns: new[] { "AttemptId", "QuestionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ManualGrades_GradedByUserId",
                table: "ManualGrades",
                column: "GradedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ManualGrades_QuestionId",
                table: "ManualGrades",
                column: "QuestionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ManualGrades");

            migrationBuilder.DropColumn(
                name: "SpeakingScore",
                table: "Results");

            migrationBuilder.DropColumn(
                name: "AudioAnswerUrl",
                table: "CandidateAnswers");
        }
    }
}
