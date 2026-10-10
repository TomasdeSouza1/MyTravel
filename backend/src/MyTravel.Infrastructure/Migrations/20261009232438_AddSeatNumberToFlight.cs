using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyTravel.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSeatNumberToFlight : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SeatNumber",
                table: "Flights",
                type: "character varying(10)",
                maxLength: 10,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SeatNumber",
                table: "Flights");
        }
    }
}
