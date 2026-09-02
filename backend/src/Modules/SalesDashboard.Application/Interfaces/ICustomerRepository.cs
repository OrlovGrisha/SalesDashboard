using SalesDashboard.Domain.Customer;

namespace SalesDashboard.Application.Interfaces;

public interface ICustomerRepository
{
    Task<IReadOnlyDictionary<Guid, Customer>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken);
}