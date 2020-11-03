using Chat.Server.Entities;
using Microsoft.EntityFrameworkCore;

namespace Chat.Server
{
    public class ApplicationContext : DbContext
    {
            public DbSet<User> Users { get; set; }

            public ApplicationContext()
            {
                Database.EnsureCreated();
            }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=chatserverdb;Trusted_Connection=True;");
            }
    }
}
