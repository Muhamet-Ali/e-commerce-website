using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_StoreMVCTemplate.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSentenceAtHeade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "HeaderImages",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "HeaderImages");
        }
    }
}
