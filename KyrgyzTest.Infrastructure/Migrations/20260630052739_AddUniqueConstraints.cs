using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KyrgyzTest.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueConstraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Results_AttemptId",
                table: "Results");

            migrationBuilder.DropIndex(
                name: "IX_CompletedSections_AttemptId",
                table: "CompletedSections");

            migrationBuilder.DropIndex(
                name: "IX_CandidateAnswers_AttemptId",
                table: "CandidateAnswers");

            migrationBuilder.CreateIndex(
                name: "IX_Results_AttemptId",
                table: "Results",
                column: "AttemptId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CompletedSections_AttemptId_Section",
                table: "CompletedSections",
                columns: new[] { "AttemptId", "Section" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Candidates_AccessCode",
                table: "Candidates",
                column: "AccessCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Candidates_Inn",
                table: "Candidates",
                column: "Inn",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CandidateAnswers_AttemptId_QuestionId",
                table: "CandidateAnswers",
                columns: new[] { "AttemptId", "QuestionId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Results_AttemptId",
                table: "Results");

            migrationBuilder.DropIndex(
                name: "IX_CompletedSections_AttemptId_Section",
                table: "CompletedSections");

            migrationBuilder.DropIndex(
                name: "IX_Candidates_AccessCode",
                table: "Candidates");

            migrationBuilder.DropIndex(
                name: "IX_Candidates_Inn",
                table: "Candidates");

            migrationBuilder.DropIndex(
                name: "IX_CandidateAnswers_AttemptId_QuestionId",
                table: "CandidateAnswers");

            migrationBuilder.CreateIndex(
                name: "IX_Results_AttemptId",
                table: "Results",
                column: "AttemptId");

            migrationBuilder.CreateIndex(
                name: "IX_CompletedSections_AttemptId",
                table: "CompletedSections",
                column: "AttemptId");

            migrationBuilder.CreateIndex(
                name: "IX_CandidateAnswers_AttemptId",
                table: "CandidateAnswers",
                column: "AttemptId");
        }
    }
}
