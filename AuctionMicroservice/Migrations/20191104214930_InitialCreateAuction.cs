using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AuctionMicroservice.Migrations
{
    public partial class InitialCreateAuction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tb_AuctionBid",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    BidDate = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    AuctionProductId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_AuctionBid", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tb_AuctionProduct",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    URLImg = table.Column<string>(nullable: true),
                    URLDescExt = table.Column<string>(nullable: true),
                    OpeningDate = table.Column<DateTime>(nullable: false),
                    MinValue = table.Column<double>(nullable: false),
                    BidAmount = table.Column<double>(nullable: false),
                    BidValue = table.Column<double>(nullable: false),
                    StopwatchTime = table.Column<int>(nullable: false),
                    CompanyId = table.Column<int>(nullable: false),
                    WinnerAuctionUserId = table.Column<int>(nullable: false),
                    LastBidDate = table.Column<DateTime>(nullable: false),
                    CurrentValue = table.Column<double>(nullable: false),
                    Closed = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_AuctionProduct", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tb_AuctionBid");

            migrationBuilder.DropTable(
                name: "Tb_AuctionProduct");
        }
    }
}
