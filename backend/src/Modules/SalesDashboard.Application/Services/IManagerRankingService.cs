using SalesDashboard.Application.Common;
using SalesDashboard.Application.Dtos;
using SalesDashboard.Application.Results;

namespace SalesDashboard.Application.Services;

public interface IManagerRankingService
{
    Task<IReadOnlyList<ManagerRankingItemDto>> GetRankingAsync(DateRange period, RankingMode mode, CancellationToken cancellationToken);
}