using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalesDashboard.Domain.Customer;
using SalesDashboard.Domain.Entities;
using SalesDashboard.Domain.Managers;

namespace SalesDashboard.Infrastructure.Configurations;

public class SaleConfiguration : IEntityTypeConfiguration<Sale>
{
    public void Configure(EntityTypeBuilder<Sale> builder)
    {
        builder.ToTable("sales");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.SaleDate)
            .IsRequired();

        builder.Property(s => s.Status)
            .HasConversion<string>()   // enum хранится как текст, а не число — читаемо в БД напрямую
            .HasMaxLength(20)
            .IsRequired();

        // Приватная коллекция _items — маппим через backing field
        builder.HasMany(s => s.Items)
            .WithOne()
            .HasForeignKey(i => i.SaleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(s => s.Items)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasField("_items");

        // Индексы под частые фильтры дашборда
        builder.HasIndex(s => s.SaleDate);
        builder.HasIndex(s => new { s.ManagerId, s.SaleDate });
        builder.HasIndex(s => s.Status);

        builder.HasOne<Manager>()
            .WithMany()
            .HasForeignKey(s => s.ManagerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Customer>()
            .WithMany()
            .HasForeignKey(s => s.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}