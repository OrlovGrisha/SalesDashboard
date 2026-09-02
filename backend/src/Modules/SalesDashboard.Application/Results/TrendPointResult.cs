namespace SalesDashboard.Application.Results;

public record TrendPointResult(DateTime PeriodStart, decimal Revenue, decimal Cost, int Count);