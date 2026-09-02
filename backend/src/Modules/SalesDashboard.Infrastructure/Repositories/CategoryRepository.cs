using Microsoft.EntityFrameworkCore;
using SalesDashboard.Application.Interfaces;
using SalesDashboard.Domain.Catalog;

namespace SalesDashboard.Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly AppDbContext _db;
    public CategoryRepository(AppDbContext db) => _db = db;

    public async Task<IReadOnlyDictionary<Guid, Category>> GetByIdsAsync(
        IEnumerable<Guid> ids, CancellationToken ct)
    {
        var idList = ids.Distinct().ToList();
        var categories = await _db.Categories.AsNoTracking()
            .Where(c => idList.Contains(c.Id))
            .ToListAsync(ct);
        return categories.ToDictionary(c => c.Id);
    }
}