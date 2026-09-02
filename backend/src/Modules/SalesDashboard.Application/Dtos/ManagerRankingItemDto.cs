namespace SalesDashboard.Application.Dtos;

public record ManagerRankingItemDto(
    int Rank,
    Guid ManagerId,
    string ManagerName,
    int SalesCount,
    decimal Revenue,
    decimal GrossProfit,
    decimal AverageCheck,
    decimal Margin,
    decimal ChangePercent
    );