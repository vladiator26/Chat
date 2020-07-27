using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatServer
{
    class AppContext : DbContext
    {
            public DbSet<User> Users { get; set; }

            public AppContext()
            {
                Database.EnsureCreated();
            }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=chatserverdb;Trusted_Connection=True;");
            }
    }
}
