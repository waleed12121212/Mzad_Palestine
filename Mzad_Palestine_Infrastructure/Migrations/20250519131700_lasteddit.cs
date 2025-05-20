using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mzad_Palestine_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class lasteddit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Listings_Auctions_AuctionId",
                table: "Listings");

            migrationBuilder.DropIndex(
                name: "IX_Listings_AuctionId",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "AuctionId",
                table: "Listings");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AuctionId",
                table: "Listings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Listings_AuctionId",
                table: "Listings",
                column: "AuctionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Listings_Auctions_AuctionId",
                table: "Listings",
                column: "AuctionId",
                principalTable: "Auctions",
                principalColumn: "AuctionId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
