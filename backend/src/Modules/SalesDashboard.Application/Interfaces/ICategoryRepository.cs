using SalesDashboard.Domain.Catalog;
using SalesDashboard.Domain.Managers;

namespace SalesDashboard.Application.Interfaces;

public interface ICategoryRepository
{
    Task<IReadOnlyDictionary<Guid, Category>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken);
}