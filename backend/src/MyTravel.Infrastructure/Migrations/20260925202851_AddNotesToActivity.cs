using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyTravel.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNotesToActivity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Activities",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Activities");
        }
    }
}
