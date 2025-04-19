using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mzad_Palestine_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ADDCATEGORYCONTROLLER : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Listings");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Listings",
                type: "datetime2",
                nullable: true,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETDATE()");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "Listings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Listings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSold",
                table: "Listings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "StartingPrice",
                table: "Listings",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Categories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Categories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Categories",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ListingId",
                table: "Bids",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ListingImage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsMainImage = table.Column<bool>(type: "bit", nullable: false),
                    ListingId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListingImage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ListingImage_Listings_ListingId",
                        column: x => x.ListingId,
                        principalTable: "Listings",
                        principalColumn: "ListingId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bids_ListingId",
                table: "Bids",
                column: "ListingId");

            migrationBuilder.CreateIndex(
                name: "IX_ListingImage_ListingId",
                table: "ListingImage",
                column: "ListingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bids_Listings_ListingId",
                table: "Bids",
                column: "ListingId",
                principalTable: "Listings",
                principalColumn: "ListingId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bids_Listings_ListingId",
                table: "Bids");

            migrationBuilder.DropTable(
                name: "ListingImage");

            migrationBuilder.DropIndex(
                name: "IX_Bids_ListingId",
                table: "Bids");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "IsSold",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "StartingPrice",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "ListingId",
                table: "Bids");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Listings",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValueSql: "GETDATE()");

            migrationBuilder.AddColumn<int>(
                name: "LocationId",
                table: "Listings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Listings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
