using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mzad_Palestine_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class editwishlist : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ListingId",
                table: "Watchlists",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "AuctionId",
                table: "Watchlists",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Watchlists_AuctionId",
                table: "Watchlists",
                column: "AuctionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Watchlists_Auctions_AuctionId",
                table: "Watchlists",
                column: "AuctionId",
                principalTable: "Auctions",
                principalColumn: "AuctionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Watchlists_Auctions_AuctionId",
                table: "Watchlists");

            migrationBuilder.DropIndex(
                name: "IX_Watchlists_AuctionId",
                table: "Watchlists");

            migrationBuilder.DropColumn(
                name: "AuctionId",
                table: "Watchlists");

            migrationBuilder.AlterColumn<int>(
                name: "ListingId",
                table: "Watchlists",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
