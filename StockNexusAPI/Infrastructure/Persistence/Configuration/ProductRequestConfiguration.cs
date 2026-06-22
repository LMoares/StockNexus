using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StockNexusAPI.Domain.Entities;
namespace StockNexusAPI.Infrastructure.Persistence.Configuration
{
    public class ProductRequestConfiguration : IEntityTypeConfiguration<ProductRequest>
    {
        public void Configure(EntityTypeBuilder<ProductRequest> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Status)
                .HasConversion<string>();

            builder.HasOne(x => x.Employee)
                .WithMany()
                .HasForeignKey(x => x.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Manager)
                .WithMany()
                .HasForeignKey(x => x.ManagerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Product)
               .WithMany()
               .HasForeignKey(x => x.ProductId)
               .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
