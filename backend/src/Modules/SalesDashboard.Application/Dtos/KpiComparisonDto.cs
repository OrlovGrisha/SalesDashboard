namespace SalesDashboard.Application.Dtos;

public record KpiComparisonDto(
    decimal RevenueChangePercent,
    decimal GrossProfitChangePercent,
    decimal SalesCountChangePercent,
    decimal AverageCheckChangePercent
    );