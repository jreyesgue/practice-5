using Microsoft.EntityFrameworkCore;
using Practice5_Model.Models;

namespace Practice5_DataAccess.Data
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Product> Product { get; set; }
        public DbSet<Purchase> Purchase { get; set; }
        public DbSet<Sale> Sale { get; set; }
        public DbSet<Inventory> Inventory { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Server=USQROJREYESGUE1;" +
                "Database=Practice5;" +
                "User Id=admin;" +
                "Password=root123;" +
                "Trusted_Connection=True;" +
                "TrustServerCertificate=True;");
        }
    }
}
