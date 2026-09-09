using Microsoft.EntityFrameworkCore;
using SalesDashboard.Application.Interfaces;
using SalesDashboard.Domain.Catalog;

namespace SalesDashboard.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _db;
    public ProductRepository(AppDbContext db) => _db = db;

    public async Task<IReadOnlyDictionary<Guid, Product>> GetByIdsAsync(
        IEnumerable<Guid> ids, CancellationToken ct)
    {
        var idList = ids.Distinct().ToList();
        var products = await _db.Products.AsNoTracking()
            .Where(p => idList.Contains(p.Id))
            .ToListAsync(ct);
        
        return products.ToDictionary(p => p.Id);
    }
}