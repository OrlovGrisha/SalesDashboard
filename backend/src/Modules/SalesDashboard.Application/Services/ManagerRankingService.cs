using SalesDashboard.Application.Common;
using SalesDashboard.Application.Dtos;
using SalesDashboard.Application.Interfaces;

namespace SalesDashboard.Application.Services;

public class ManagerRankingService : IManagerRankingService
{
    private readonly ISaleRepository _saleRepository;
    private readonly IManagerRepository _managerRepository;

    public ManagerRankingService(ISaleRepository saleRepository, IManagerRepository managerRepository)
    {
        _saleRepository = saleRepository;
        _managerRepository = managerRepository;
    }

    public async Task<IReadOnlyList<ManagerRankingItemDto>> GetRankingAsync(DateRange period, RankingMode mode, CancellationToken cancellationToken)
    {
        var current = await _saleRepository.GetAggregateByManagerAsync(period, cancellationToken);
        var previous = await _saleRepository.GetAggregateByManagerAsync(period.PreviousPeriod, cancellationToken);

        var previousByManager = previous.ToDictionary(p => p.ManagerId);
        var currentByManager = current.ToDictionary(c => c.ManagerId);

        var allManagers = await _managerRepository.GetAllAsync(cancellationToken);

        var items = allManagers.Select(manager =>
            {
                currentByManager.TryGetValue(manager.Id, out var cur);

                var revenue = cur?.Revenue ?? 0;
                var cost = cur?.Cost ?? 0;
                var count = cur?.Count ?? 0;
                var grossProfit = revenue - cost;
                var averageCheck = count > 0 ? revenue / count : 0;

                previousByManager.TryGetValue(manager.Id, out var prev);

                var prevGrossProfit = (prev?.Revenue ?? 0) - (prev?.Cost ?? 0);
                var prevAverageCheck = prev is { Count: > 0 } ? prev.Revenue / prev.Count : 0;

                var currentMetric = mode == RankingMode.GrossProfit ? grossProfit : averageCheck;
                var prevMetrics = mode == RankingMode.GrossProfit ? prevGrossProfit : prevAverageCheck;

                return new ManagerRankingItemDto(
                    Rank: 0,
                    ManagerId: manager.Id,
                    ManagerName: manager.FullName,
                    SalesCount: count,
                    Revenue: revenue,
                    GrossProfit: grossProfit,
                    AverageCheck: averageCheck,
                    Margin: revenue > 0 ? grossProfit / revenue : 0,
                    ChangePercent: prevMetrics > 0
                        ? (currentMetric - prevMetrics) / prevMetrics * 100
                        : 0
                );
            })
            .OrderByDescending(x => mode == RankingMode.GrossProfit ? x.GrossProfit : x.AverageCheck)
            .ThenBy(x => x.ManagerName)
            .Select((x, i) => x with { Rank = i + 1 })
            .ToList();
        
        return items;
    }
}