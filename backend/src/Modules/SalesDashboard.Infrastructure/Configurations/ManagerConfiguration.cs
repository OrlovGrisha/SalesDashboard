using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalesDashboard.Domain.Managers;

namespace SalesDashboard.Infrastructure.Configurations;

public class ManagerConfiguration : IEntityTypeConfiguration<Manager>
{
    public void Configure(EntityTypeBuilder<Manager> builder)
    {
        builder.ToTable("managers");
        builder.HasKey(m => m.Id);

        builder.Property(m => m.FullName)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(m => m.Team)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(m => m.IsActive)
            .IsRequired();
    }
}