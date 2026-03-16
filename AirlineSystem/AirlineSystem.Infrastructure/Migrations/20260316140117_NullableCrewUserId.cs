using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AirlineSystem.Migrations
{
    /// <inheritdoc />
    public partial class NullableCrewUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CrewMembers_UserId",
                table: "CrewMembers");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "CrewMembers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_CrewMembers_UserId",
                table: "CrewMembers",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CrewMembers_UserId",
                table: "CrewMembers");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "CrewMembers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CrewMembers_UserId",
                table: "CrewMembers",
                column: "UserId",
                unique: true);
        }
    }
}
