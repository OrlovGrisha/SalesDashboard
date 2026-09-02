namespace SalesDashboard.Application.Dtos;

public record KpiSummaryDto(
    decimal Revenue,
    decimal GrossProfit,
    decimal Margin,
    int SalesCount,
    decimal AverageCheck,
    string? TopManagerName,
    KpiComparisonDto Comparison 
    );