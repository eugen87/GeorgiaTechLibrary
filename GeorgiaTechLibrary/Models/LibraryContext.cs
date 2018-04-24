using GeorgiaTechLibrary.Models.Items;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeorgiaTechLibrary.Models
{
    public class LibraryContext : DbContext
    {
        public DbSet<Item> Items { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=Kraka.ucn.dk;Initial Catalog=Psu0218_1055997;user id=Psu0218_1055997; password=Password1!");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Book>();
            builder.Entity<Map>();

            base.OnModelCreating(builder);

        }
    }
}
