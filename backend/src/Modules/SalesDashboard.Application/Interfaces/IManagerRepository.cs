using SalesDashboard.Domain.Managers;

namespace SalesDashboard.Application.Interfaces;

public interface IManagerRepository
{
    Task<Manager?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    // что за метод?
    Task<IReadOnlyDictionary<Guid, Manager>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken);
    Task<IReadOnlyList<Manager>> GetAllAsync(CancellationToken cancellationToken);
}