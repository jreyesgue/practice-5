using Microsoft.EntityFrameworkCore;
using Practice5_Model.Models;

namespace Practice5_DataAccess.Data
{
    public class DatabaseContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Product> Product { get; set; }
        public DbSet<Purchase> Purchase { get; set; }
        public DbSet<Sale> Sale { get; set; }
        public DbSet<Inventory> Inventory { get; set; }
    }
}
