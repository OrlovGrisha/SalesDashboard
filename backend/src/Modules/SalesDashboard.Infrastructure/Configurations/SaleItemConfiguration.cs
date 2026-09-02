using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalesDashboard.Domain.Catalog;
using SalesDashboard.Domain.Entities;

namespace SalesDashboard.Infrastructure.Configurations;

public class SaleItemConfiguration : IEntityTypeConfiguration<SaleItem>
{
    public void Configure(EntityTypeBuilder<SaleItem> builder)
    {
        builder.ToTable("sale_items");
        builder.HasKey(i => i.Id);

        builder.Property(i => i.Quantity).IsRequired();

        builder.ComplexProperty(i => i.UnitPrice, m =>
        {
            m.Property(x => x.Amount)
                .HasColumnName("unit_price")
                .HasColumnType("decimal(18,2)");
        });

        builder.ComplexProperty(i => i.UnitCost, m =>
        {
            m.Property(x => x.Amount)
                .HasColumnName("unit_cost")
                .HasColumnType("decimal(18,2)");
        });

        builder.HasOne<Product>()
            .WithMany()
            .HasForeignKey(i => i.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(i => i.ProductId);
    }
}