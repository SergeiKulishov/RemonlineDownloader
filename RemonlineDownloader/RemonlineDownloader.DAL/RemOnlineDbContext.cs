using Microsoft.EntityFrameworkCore;
using RemskladDesktop;
using RemskladDesktop.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemonlineDownloader.Core
{
    public class RemOnlineDbContext : DbContext

    {
        public DbSet<Datum>? Datums { get; set; }
        public DbSet<Order>? Orders { get; set; }

        public RemOnlineDbContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=remskladapi;Trusted_Connection=True;");
            //optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=usersdb;Username=postgres;Password=greatsteve");
            optionsBuilder.UseSqlite("Data Source = Remonline.db");
        }
    }
}
