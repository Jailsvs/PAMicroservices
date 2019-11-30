using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CompanyMicroservice.Models;

namespace CompanyMicroservice.DBContexts
{
    public class TenantContext : DbContext
    {

        public TenantContext(DbContextOptions<TenantContext> options): base(options)
        {

        }
        public DbSet<Tenant> Tb_Tenant { get; set; }
       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
             modelBuilder.Entity<Tenant>().HasData(
                new Tenant
                {
                    Id = 1,
                    Name = "Fast and Furious Auto Postos",
                    Host = "www.fastandfouriousAP.com.br",
                    TenantId = 0
                }
            );
        }

    }
}