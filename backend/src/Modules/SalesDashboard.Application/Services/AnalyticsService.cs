using SalesDashboard.Application.Common;
using SalesDashboard.Application.Dtos;

namespace SalesDashboard.Application.Services;

public class AnalyticsService : IAnalyticsService
{
    public Task<KpiSummaryDto> GetKpiSummaryAsync(DateRange period, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<SalesTrendPointDto>> GetSalesTrendAsync(DateRange period, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<CategoryBreakdownDto>> GetCategoryBreakdownAsync(DateRange period, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<ProductBreakdownDto>> GetTopProductsAsync(DateRange period, int take, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<RecentSaleDto>> GetRecentSalesAsync(DateRange period, int take, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}