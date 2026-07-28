using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KyrgyzTest.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTestVariantIsArchived : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsArchived",
                table: "TestVariants",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            // Variants generated before this migration were all auto-created per exam
            // login rather than pre-built by an admin — archive the whole existing pile
            // so they drop out of the active assignment pool and the default list view.
            migrationBuilder.Sql("UPDATE \"TestVariants\" SET \"IsArchived\" = true;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsArchived",
                table: "TestVariants");
        }
    }
}
