using SalesDashboard.Application.Common;
using SalesDashboard.Application.Dtos;

namespace SalesDashboard.Application.Services;

public interface IAnalyticsService
{
    Task<KpiSummaryDto> GetKpiSummaryAsync(DateRange period, CancellationToken cancellationToken);
    Task<IReadOnlyList<SalesTrendPointDto>> GetSalesTrendAsync(DateRange period, TrendGranularity granularity, CancellationToken cancellationToken);
    Task<IReadOnlyList<CategoryBreakdownDto>> GetCategoryBreakdownAsync(DateRange period, CancellationToken ct);
    Task<IReadOnlyList<ProductBreakdownDto>> GetTopProductsAsync(DateRange period, int take, CancellationToken ct);
    Task<IReadOnlyList<RecentSaleDto>> GetRecentSalesAsync(DateRange period, int take, CancellationToken ct);
}