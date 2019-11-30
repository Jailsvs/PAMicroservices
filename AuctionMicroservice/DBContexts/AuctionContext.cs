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
                    OpeningDate = DateTime.Parse("2019-11-20T20:00:00"),
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
                    OpeningDate = DateTime.Parse("2019-11-16T12:00:00"),
                    Description = "Furadeira Bosh 550W",
                    StopwatchTime = 100,
                    BidValue = 1,
                    URLImg = "https://a-static.mlcdn.com.br/618x463/furadeira-de-impacto-bosch-550w-velocidade-variavel-1-2-3-pecas-gsb-550-re-std/magazineluiza/221549500/4ed2414a6d8538e9712a58f7ce84beaa.jpg",
                    CompanyId = 2,
                    TenantId = 1
                }
            );
        }

    }
}