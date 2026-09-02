using Microsoft.EntityFrameworkCore;
using SalesDashboard.Domain.Catalog;
using SalesDashboard.Domain.Customer;
using SalesDashboard.Domain.Entities;
using SalesDashboard.Domain.Managers;

namespace SalesDashboard.Infrastructure;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Sale> Sales => Set<Sale>();
    public DbSet<Manager> Managers => Set<Manager>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Product> Products => Set<Product>();
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}