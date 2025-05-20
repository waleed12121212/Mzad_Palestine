using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mzad_Palestine_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class editReview : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ListingId" ,
                table: "Reviews" ,
                type: "int" ,
                nullable: true ,
                oldClrType: typeof(int) ,
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "AuctionId" ,
                table: "Reviews" ,
                type: "int" ,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_AuctionId" ,
                table: "Reviews" ,
                column: "AuctionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Auctions_AuctionId" ,
                table: "Reviews" ,
                column: "AuctionId" ,
                principalTable: "Auctions" ,
                principalColumn: "AuctionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Auctions_AuctionId" ,
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_AuctionId" ,
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "AuctionId" ,
                table: "Reviews");

            migrationBuilder.AlterColumn<int>(
                name: "ListingId" ,
                table: "Reviews" ,
                type: "int" ,
                nullable: false ,
                defaultValue: 0 ,
                oldClrType: typeof(int) ,
                oldType: "int" ,
                oldNullable: true);
        }
    }
}
