using Microsoft.EntityFrameworkCore;
using SalesDashboard.Application.Interfaces;
using SalesDashboard.Domain.Managers;

namespace SalesDashboard.Infrastructure.Repositories;

public class ManagerRepository : IManagerRepository
{
    private readonly AppDbContext _db;
    public ManagerRepository(AppDbContext db) => _db = db;

    public async Task<Manager?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await _db.Managers
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == id, ct);
    }
    

    public async Task<IReadOnlyDictionary<Guid, Manager>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken ct)
    {
        var idList = ids.Distinct().ToList();
        var managers = await _db.Managers.AsNoTracking()
            .Where(m => idList.Contains(m.Id))
            .ToListAsync(ct);
        return managers.ToDictionary(m => m.Id);
    }

    public async Task<IReadOnlyList<Manager>> GetAllAsync(CancellationToken ct)
    {
        return await _db.Managers
            .AsNoTracking()
            .ToListAsync(ct);
    }
}