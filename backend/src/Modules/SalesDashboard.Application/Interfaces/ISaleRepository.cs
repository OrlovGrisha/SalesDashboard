using SalesDashboard.Application.Common;
using SalesDashboard.Application.Results;
using SalesDashboard.Domain.Entities;

namespace SalesDashboard.Application.Interfaces;

public interface ISaleRepository
{
    Task<SalesAggregateResult> GetAggregateAsync(DateRange period, CancellationToken cancellationToken);
    Task<IReadOnlyList<ManagerAggregateResult>> GetAggregateByManagerAsync(DateRange period, CancellationToken cancellationToken);
    Task<IReadOnlyList<TrendPointResult>> GetTrendAsync(DateRange period, TrendGranularity granularity, CancellationToken cancellationToken);
    Task<IReadOnlyList<CategoryAggregateResult>> GetAggregateByCategoryAsync(DateRange period, CancellationToken cancellationToken);
    Task<IReadOnlyList<ProductAggregateResult>> GetTopProductsAsync(DateRange period, int take, CancellationToken cancellationToken);
    Task<IReadOnlyList<Sale>> GetRecentAsync(DateRange period, int take, CancellationToken cancellationToken);
    
    Task AddAsync(Sale sale, CancellationToken cancellationToken);
}