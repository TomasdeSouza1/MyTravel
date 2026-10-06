using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyTravel.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDatabaseConstraintsAndRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ItineraryDays_TripId",
                table: "ItineraryDays");

            migrationBuilder.DropIndex(
                name: "IX_Activities_ItineraryDayId",
                table: "Activities");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Trips",
                type: "character varying(120)",
                maxLength: 120,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "InviteToken",
                table: "Trips",
                type: "character varying(64)",
                maxLength: 64,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "DestinationCountry",
                table: "Trips",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "DestinationCity",
                table: "Trips",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Trips",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CoverImageUrl",
                table: "Trips",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BaseCurrency",
                table: "Trips",
                type: "character varying(3)",
                maxLength: 3,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "WeatherSumm",
                table: "ItineraryDays",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "ItineraryDays",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LocationCountry",
                table: "ItineraryDays",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "LocationCity",
                table: "ItineraryDays",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "OriginalCurrency",
                table: "Expenses",
                type: "character varying(3)",
                maxLength: 3,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "AspNetUsers",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "Activities",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Activities",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "BookingReference",
                table: "Activities",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Activities",
                type: "character varying(300)",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Trips_InviteToken",
                table: "Trips",
                column: "InviteToken",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Trips_UserId",
                table: "Trips",
                column: "UserId");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Trips_Dates",
                table: "Trips",
                sql: "\"EndDate\" >= \"StartDate\"");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Trips_TotalBudget",
                table: "Trips",
                sql: "\"TotalBudget\" >= 0");

            migrationBuilder.CreateIndex(
                name: "IX_ItineraryDays_TripId_DayNumber",
                table: "ItineraryDays",
                columns: new[] { "TripId", "DayNumber" });

            migrationBuilder.AddCheckConstraint(
                name: "CK_ItineraryDays_DayNumber",
                table: "ItineraryDays",
                sql: "\"DayNumber\" > 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_ItineraryDays_TemperatureC",
                table: "ItineraryDays",
                sql: "\"TemperatureC\" IS NULL OR (\"TemperatureC\" >= -60 AND \"TemperatureC\" <= 60)");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_ActivityId",
                table: "Expenses",
                column: "ActivityId");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Expenses_ConvertedAmount",
                table: "Expenses",
                sql: "\"ConvertedAmount\" >= 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Expenses_ExchangeRateUsed",
                table: "Expenses",
                sql: "\"ExchangeRateUsed\" > 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Expenses_OriginalAmount",
                table: "Expenses",
                sql: "\"OriginalAmount\" >= 0");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_ItineraryDayId_OrderIndex",
                table: "Activities",
                columns: new[] { "ItineraryDayId", "OrderIndex" });

            migrationBuilder.AddCheckConstraint(
                name: "CK_Activities_Latitude",
                table: "Activities",
                sql: "\"Latitude\" >= -90.0 AND \"Latitude\" <= 90.0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Activities_Longitude",
                table: "Activities",
                sql: "\"Longitude\" >= -180.0 AND \"Longitude\" <= 180.0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Activities_OrderIndex",
                table: "Activities",
                sql: "\"OrderIndex\" >= 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Activities_Times",
                table: "Activities",
                sql: "\"EndTime\" >= \"StartTime\"");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_Activities_ActivityId",
                table: "Expenses",
                column: "ActivityId",
                principalTable: "Activities",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_AspNetUsers_UserId",
                table: "Trips",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_Activities_ActivityId",
                table: "Expenses");

            migrationBuilder.DropForeignKey(
                name: "FK_Trips_AspNetUsers_UserId",
                table: "Trips");

            migrationBuilder.DropIndex(
                name: "IX_Trips_InviteToken",
                table: "Trips");

            migrationBuilder.DropIndex(
                name: "IX_Trips_UserId",
                table: "Trips");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Trips_Dates",
                table: "Trips");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Trips_TotalBudget",
                table: "Trips");

            migrationBuilder.DropIndex(
                name: "IX_ItineraryDays_TripId_DayNumber",
                table: "ItineraryDays");

            migrationBuilder.DropCheckConstraint(
                name: "CK_ItineraryDays_DayNumber",
                table: "ItineraryDays");

            migrationBuilder.DropCheckConstraint(
                name: "CK_ItineraryDays_TemperatureC",
                table: "ItineraryDays");

            migrationBuilder.DropIndex(
                name: "IX_Expenses_ActivityId",
                table: "Expenses");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Expenses_ConvertedAmount",
                table: "Expenses");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Expenses_ExchangeRateUsed",
                table: "Expenses");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Expenses_OriginalAmount",
                table: "Expenses");

            migrationBuilder.DropIndex(
                name: "IX_Activities_ItineraryDayId_OrderIndex",
                table: "Activities");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Activities_Latitude",
                table: "Activities");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Activities_Longitude",
                table: "Activities");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Activities_OrderIndex",
                table: "Activities");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Activities_Times",
                table: "Activities");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Trips",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(120)",
                oldMaxLength: 120);

            migrationBuilder.AlterColumn<string>(
                name: "InviteToken",
                table: "Trips",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(64)",
                oldMaxLength: 64);

            migrationBuilder.AlterColumn<string>(
                name: "DestinationCountry",
                table: "Trips",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "DestinationCity",
                table: "Trips",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Trips",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CoverImageUrl",
                table: "Trips",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BaseCurrency",
                table: "Trips",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(3)",
                oldMaxLength: 3);

            migrationBuilder.AlterColumn<string>(
                name: "WeatherSumm",
                table: "ItineraryDays",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "ItineraryDays",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(2000)",
                oldMaxLength: 2000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LocationCountry",
                table: "ItineraryDays",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "LocationCity",
                table: "ItineraryDays",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "OriginalCurrency",
                table: "Expenses",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(3)",
                oldMaxLength: 3);

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "Activities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Activities",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(150)",
                oldMaxLength: 150);

            migrationBuilder.AlterColumn<string>(
                name: "BookingReference",
                table: "Activities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Activities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(300)",
                oldMaxLength: 300,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItineraryDays_TripId",
                table: "ItineraryDays",
                column: "TripId");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_ItineraryDayId",
                table: "Activities",
                column: "ItineraryDayId");
        }
    }
}
