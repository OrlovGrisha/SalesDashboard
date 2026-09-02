using SalesDashboard.Domain.Catalog;

namespace SalesDashboard.Application.Interfaces;

public interface IProductRepository
{
    Task<IReadOnlyDictionary<Guid, Product>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken);
}