using Microsoft.EntityFrameworkCore;
using SalesDashboard.Application.Interfaces;
using SalesDashboard.Domain.Customer;

namespace SalesDashboard.Infrastructure.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly AppDbContext _db;
    public CustomerRepository(AppDbContext db) => _db = db;

    public async Task<IReadOnlyDictionary<Guid, Customer>> GetByIdsAsync(
        IEnumerable<Guid> ids, CancellationToken ct)
    {
        var idList = ids.Distinct().ToList();
        var customers = await _db.Customers.AsNoTracking()
            .Where(c => idList.Contains(c.Id))
            .ToListAsync(ct);
        return customers.ToDictionary(c => c.Id);
    }
}