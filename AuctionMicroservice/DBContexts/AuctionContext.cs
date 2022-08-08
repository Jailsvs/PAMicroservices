using AuctionMicroservice.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace AuctionMicroservice.DBContexts
{
    public class AuctionContext : DbContext
    {

        public AuctionContext(DbContextOptions<AuctionContext> options): base(options)
        {

        }
        public DbSet<AuctionProduct> Tb_AuctionProduct { get; set; }
        public DbSet<AuctionBid> Tb_AuctionBid { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
             modelBuilder.Entity<AuctionProduct>().HasData(
                new AuctionProduct
                {
                    Id = 1,
                    MinValue = 100,
                    OpeningDate = DateTime.Parse("2022-08-06T17:00:00"),
                    Description = "Bola Futsal Topper",
                    StopwatchTime = 100,
                    BidValue = 1,
                    URLImg = "https://static.netshoes.com.br/produtos/bola-futsal-topper-slick-ii-exclusiva/20/D30-1269-120/D30-1269-120_zoom1.jpg",
                    CompanyId = 1,
                    TenantId = 1
                },
                new AuctionProduct
                {
                    Id = 2,
                    MinValue = 200,
                    OpeningDate = DateTime.Parse("2022-08-06T17:00:00"),
                    Description = "Furadeira Bosh 550W",
                    StopwatchTime = 100,
                    BidValue = 1,
                    URLImg = "https://www.taqi.com.br/ccstore/v1/images/?source=/file/v8706143511674300263/products/178325.00-furadeira-hobby-impacto-black-decker-tm500kbr-220-volts.jpg&height=1000&width=1000&quality=0.9",
                    CompanyId = 2,
                    TenantId = 1
                }
            );
        }

    }
}