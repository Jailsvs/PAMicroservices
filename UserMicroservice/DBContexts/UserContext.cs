using Microsoft.EntityFrameworkCore;
using UserMicroservice.Models;

namespace UserMicroservice.DBContexts
{
    public class UserContext: DbContext
    {

        public UserContext(DbContextOptions<UserContext> options): base(options)
        {

        }
        public DbSet<User> Tb_User { get; set; }
       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
             modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Name = "Jailson VS",
                    AvailableBids = 100,
                    Email = "user001@gmail.com",
                    Password = "R$%TGss5",
                    TenantId = 1,
                    UserType = "C",
                    Whats = "47999999999"

                },
                new User
                {
                    Id = 2,
                    Name = "Jhon WF",
                    AvailableBids = 100,
                    Email = "user002@gmail.com",
                    Password = "#$#HJJTT@88",
                    TenantId = 1,
                    UserType = "C",
                    Whats = "47999000000"
                },
                new User
                {
                    Id = 3,
                    Name = "Kay QG",
                    AvailableBids = 100,
                    Email = "user002@gmail.com",
                    Password = "UY6%$$885",
                    TenantId = 1,
                    UserType = "C",
                    Whats = "47999888888"
                }
            );
        }

    }
}