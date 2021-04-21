using apiEncomendei.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apiEncomendei.Persistence
{
    public class EncomendeiDbContext : DbContext
    {
        public EncomendeiDbContext(DbContextOptions<EncomendeiDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Shopkeeper> Shopkeepers { get; set; }
        public DbSet<Category> Shopkeepers { get; set; }
        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        }
    }
}
