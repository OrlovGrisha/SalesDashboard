using SalesDashboard.Domain.Catalog;

namespace SalesDashboard.Application.Interfaces;

public interface IProductRepository
{
    Task<IReadOnlyDictionary<Guid, Product>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken);
    Task<IReadOnlyDictionary<Guid, Product>> GetAllWithCategoryAsync(CancellationToken cancellationToken); // для top products, где нужно и имя категории
}