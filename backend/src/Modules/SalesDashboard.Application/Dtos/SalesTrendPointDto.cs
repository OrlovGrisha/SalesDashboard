namespace SalesDashboard.Application.Dtos;

public record SalesTrendPointDto(
    DateTime PeriodStart,
    decimal Revenue,
    decimal GrossProfit,
    int SalesCount
    );