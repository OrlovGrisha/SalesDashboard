using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SalesDashboard.Application.Interfaces;
using SalesDashboard.Infrastructure.Repositories;
using SalesDashboard.Infrastructure.Seed;
using SalesDashboard.Infrastructure.Seed.Generators;

namespace SalesDashboard.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<ISaleRepository, SaleRepository>();
        services.AddScoped<IManagerRepository, ManagerRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        services.AddSingleton<SeedRandom>();
        services.AddScoped<CategoryProductGenerator>();
        services.AddScoped<ManagerGenerator>();
        services.AddScoped<CustomerGenerator>();
        services.AddScoped<SaleGenerator>();
        services.AddScoped<DatabaseSeeder>();
        services.AddHostedService<SeedHostedService>();

        return services;
    }
}