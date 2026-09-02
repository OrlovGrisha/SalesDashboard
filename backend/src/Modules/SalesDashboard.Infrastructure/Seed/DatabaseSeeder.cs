using SalesDashboard.Infrastructure.Seed.Generators;

namespace SalesDashboard.Infrastructure.Seed;

public class DatabaseSeeder
{
    private readonly AppDbContext _db;
    private readonly CategoryProductGenerator _categoryProductGenerator;
    private readonly ManagerGenerator _managerGenerator;
    private readonly CustomerGenerator _customerGenerator;
    private readonly SaleGenerator _saleGenerator;

    public DatabaseSeeder(
        AppDbContext db,
        CategoryProductGenerator categoryProductGenerator,
        ManagerGenerator managerGenerator,
        CustomerGenerator customerGenerator,
        SaleGenerator saleGenerator)
    {
        _db = db;
        _categoryProductGenerator = categoryProductGenerator;
        _managerGenerator = managerGenerator;
        _customerGenerator = customerGenerator;
        _saleGenerator = saleGenerator;
    }

    public async Task SeedAsync(CancellationToken ct)
    {
        var (categories, products) = _categoryProductGenerator.Generate();
        await _db.Categories.AddRangeAsync(categories, ct);
        await _db.Products.AddRangeAsync(products, ct);
        await _db.SaveChangesAsync(ct);

        var managerCount = 20;
        var managersWithProfiles = _managerGenerator.Generate(managerCount);
        await _db.Managers.AddRangeAsync(managersWithProfiles.Select(m => m.Manager), ct);
        await _db.SaveChangesAsync(ct);

        var customers = _customerGenerator.Generate(75);
        await _db.Customers.AddRangeAsync(customers, ct);
        await _db.SaveChangesAsync(ct);

        var periodEnd = DateTime.UtcNow.Date;
        var periodStart = periodEnd.AddMonths(-12);

        var sales = _saleGenerator.Generate(
            managersWithProfiles, customers, products, periodStart, periodEnd, targetCount: 3500);

        // Батчами по 500, чтобы не создавать один гигантский INSERT и не раздувать change tracker
        const int batchSize = 500;
        for (int i = 0; i < sales.Count; i += batchSize)
        {
            var batch = sales.Skip(i).Take(batchSize).ToList();
            await _db.Sales.AddRangeAsync(batch, ct);
            await _db.SaveChangesAsync(ct);
            _db.ChangeTracker.Clear(); // освобождаем tracked entities между батчами
        }
    }
}