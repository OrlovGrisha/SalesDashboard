using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalesDashboard.Domain.Catalog;

namespace SalesDashboard.Infrastructure.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("products");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name).HasMaxLength(200).IsRequired();

        builder.ComplexProperty(p => p.BasePrice, m =>
            m.Property(x => x.Amount).HasColumnName("base_price").HasColumnType("decimal(18,2)"));

        builder.ComplexProperty(p => p.BaseCost, m =>
            m.Property(x => x.Amount).HasColumnName("base_cost").HasColumnType("decimal(18,2)"));

        builder.HasOne<Category>()
            .WithMany()
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(p => p.CategoryId);
    }
}