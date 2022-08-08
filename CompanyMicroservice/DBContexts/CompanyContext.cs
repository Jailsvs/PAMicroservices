using Microsoft.EntityFrameworkCore;
using CompanyMicroservice.Models;

namespace CompanyMicroservice.DBContexts
{
    public class CompanyContext: DbContext
    {

        public CompanyContext(DbContextOptions<CompanyContext> options): base(options)
        {}
        public DbSet<Company> Tb_Company { get; set; }
       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
             modelBuilder.Entity<Company>().HasData(
                new Company
                {
                    Id = 1,
                    Name = "Empresa Teste 001",
                    CNPJ = "51.965.287/0001-25",
                    Email = "empteste001@api.com.br",
                    Whats = "47999999999",
                    TenantId = 1
                    
                },
                new Company
                {
                    Id = 2,
                    Name = "Empresa Teste 002",
                    CNPJ = "40.871.824/0001-51",
                    Email = "empteste002@api.com.br",
                    Whats = "47999000000",
                    TenantId = 1

                },
                new Company
                {
                    Id = 3,
                    Name = "Empresa Teste 003",
                    CNPJ = "83.145.029/0001-99",
                    Email = "empteste003@api.com.br",
                    Whats = "47999888888",
                    TenantId = 1

                },
                new Company
                {
                    Id = 4,
                    Name = "Empresa Teste 004",
                    CNPJ = "14.145.576/0001-51",
                    Email = "empteste004@api.com.br",
                    Whats = "47999111111",
                    TenantId = 1

                });
        }

    }
}