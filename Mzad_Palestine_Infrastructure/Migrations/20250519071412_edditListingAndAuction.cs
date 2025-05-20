using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mzad_Palestine_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class edditListingAndAuction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Auctions_Listings_ListingId",
                table: "Auctions");

            migrationBuilder.DropIndex(
                name: "IX_Auctions_ListingId",
                table: "Auctions");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Auctions");

            migrationBuilder.RenameColumn(
                name: "StartTime",
                table: "Auctions",
                newName: "StartDate");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Auctions",
                newName: "Address");

            migrationBuilder.RenameColumn(
                name: "ListingId",
                table: "Auctions",
                newName: "CategoryId");

            migrationBuilder.RenameColumn(
                name: "EndTime",
                table: "Auctions",
                newName: "EndDate");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Listings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "AuctionId",
                table: "Listings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Categories",
                type: "nvarchar(max)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Auctions",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValueSql: "GETDATE()");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Auctions",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Auctions",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Auctions",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "AuctionImages",
                columns: table => new
                {
                    ImageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuctionId = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IsMain = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuctionImages", x => x.ImageId);
                    table.ForeignKey(
                        name: "FK_AuctionImages_Auctions_AuctionId",
                        column: x => x.AuctionId,
                        principalTable: "Auctions",
                        principalColumn: "AuctionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Phones",
                columns: table => new
                {
                    PhoneId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeviceName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RamExpandable = table.Column<bool>(type: "bit", nullable: false),
                    BatteryCapacity = table.Column<int>(type: "int", nullable: false),
                    DisplaySize = table.Column<double>(type: "float", nullable: false),
                    Storage = table.Column<int>(type: "int", nullable: false),
                    Ram = table.Column<int>(type: "int", nullable: false),
                    RefreshRate = table.Column<int>(type: "int", nullable: false),
                    FrontCamera = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RearCamera = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChargingSpeed = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Phones", x => x.PhoneId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Listings_AuctionId",
                table: "Listings",
                column: "AuctionId");

            migrationBuilder.CreateIndex(
                name: "IX_Auctions_CategoryId",
                table: "Auctions",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_AuctionImages_AuctionId",
                table: "AuctionImages",
                column: "AuctionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Auctions_Categories_CategoryId",
                table: "Auctions",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Listings_Auctions_AuctionId",
                table: "Listings",
                column: "AuctionId",
                principalTable: "Auctions",
                principalColumn: "AuctionId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Auctions_Categories_CategoryId",
                table: "Auctions");

            migrationBuilder.DropForeignKey(
                name: "FK_Listings_Auctions_AuctionId",
                table: "Listings");

            migrationBuilder.DropTable(
                name: "AuctionImages");

            migrationBuilder.DropTable(
                name: "Phones");

            migrationBuilder.DropIndex(
                name: "IX_Listings_AuctionId",
                table: "Listings");

            migrationBuilder.DropIndex(
                name: "IX_Auctions_CategoryId",
                table: "Auctions");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "AuctionId",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Auctions");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Auctions");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "Auctions",
                newName: "StartTime");

            migrationBuilder.RenameColumn(
                name: "EndDate",
                table: "Auctions",
                newName: "EndTime");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "Auctions",
                newName: "ListingId");

            migrationBuilder.RenameColumn(
                name: "Address",
                table: "Auctions",
                newName: "Name");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Categories",
                type: "text",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Auctions",
                type: "datetime2",
                nullable: true,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETDATE()");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Auctions",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Auctions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Auctions_ListingId",
                table: "Auctions",
                column: "ListingId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Auctions_Listings_ListingId",
                table: "Auctions",
                column: "ListingId",
                principalTable: "Listings",
                principalColumn: "ListingId");
        }
    }
}
