using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AirlineSystem.Migrations
{
    /// <inheritdoc />
    public partial class AirlineSystem3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketPromotion_Promotion_PromotionId",
                table: "TicketPromotion");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketPromotion_Tickets_TicketId",
                table: "TicketPromotion");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TicketPromotion",
                table: "TicketPromotion");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Promotion",
                table: "Promotion");

            migrationBuilder.RenameTable(
                name: "TicketPromotion",
                newName: "TicketPromotions");

            migrationBuilder.RenameTable(
                name: "Promotion",
                newName: "Promotions");

            migrationBuilder.RenameIndex(
                name: "IX_TicketPromotion_PromotionId",
                table: "TicketPromotions",
                newName: "IX_TicketPromotions_PromotionId");

            migrationBuilder.RenameIndex(
                name: "IX_Promotion_Code",
                table: "Promotions",
                newName: "IX_Promotions_Code");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TicketPromotions",
                table: "TicketPromotions",
                columns: new[] { "TicketId", "PromotionId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Promotions",
                table: "Promotions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketPromotions_Promotions_PromotionId",
                table: "TicketPromotions",
                column: "PromotionId",
                principalTable: "Promotions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TicketPromotions_Tickets_TicketId",
                table: "TicketPromotions",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketPromotions_Promotions_PromotionId",
                table: "TicketPromotions");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketPromotions_Tickets_TicketId",
                table: "TicketPromotions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TicketPromotions",
                table: "TicketPromotions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Promotions",
                table: "Promotions");

            migrationBuilder.RenameTable(
                name: "TicketPromotions",
                newName: "TicketPromotion");

            migrationBuilder.RenameTable(
                name: "Promotions",
                newName: "Promotion");

            migrationBuilder.RenameIndex(
                name: "IX_TicketPromotions_PromotionId",
                table: "TicketPromotion",
                newName: "IX_TicketPromotion_PromotionId");

            migrationBuilder.RenameIndex(
                name: "IX_Promotions_Code",
                table: "Promotion",
                newName: "IX_Promotion_Code");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TicketPromotion",
                table: "TicketPromotion",
                columns: new[] { "TicketId", "PromotionId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Promotion",
                table: "Promotion",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketPromotion_Promotion_PromotionId",
                table: "TicketPromotion",
                column: "PromotionId",
                principalTable: "Promotion",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TicketPromotion_Tickets_TicketId",
                table: "TicketPromotion",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
