using Microsoft.EntityFrameworkCore;
using StockNexusAPI.Domain.Entities;
using System.Reflection;
namespace StockNexusAPI.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Stock> Stock { get; set; }

        public DbSet<ProductRequest> ProductRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //Scan for all IEntityTypeConfigurations in this assembly and apply them
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
