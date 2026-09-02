using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SalesDashboard.Infrastructure.Seed;

public class SeedHostedService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<SeedHostedService> _logger;

    public SeedHostedService(IServiceProvider serviceProvider, ILogger<SeedHostedService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken ct)
    {
        using var scope = _serviceProvider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        _logger.LogInformation("Applying pending migrations...");
        await db.Database.MigrateAsync(ct);

        if (await db.Sales.AnyAsync(ct))
        {
            _logger.LogInformation("Database already seeded, skipping.");
            return;
        }

        _logger.LogInformation("Seeding database...");
        var seeder = scope.ServiceProvider.GetRequiredService<DatabaseSeeder>();
        await seeder.SeedAsync(ct);
        _logger.LogInformation("Seeding completed.");
    }

    public Task StopAsync(CancellationToken ct) => Task.CompletedTask;
}