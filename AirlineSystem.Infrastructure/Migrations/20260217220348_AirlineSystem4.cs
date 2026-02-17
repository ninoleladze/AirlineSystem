using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AirlineSystem.Migrations
{
    /// <inheritdoc />
    public partial class AirlineSystem4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "BasePrice",
                table: "Flights",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "PriceCurrency",
                table: "Flights",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BasePrice",
                table: "Flights");

            migrationBuilder.DropColumn(
                name: "PriceCurrency",
                table: "Flights");
        }
    }
}
