namespace SalesDashboard.Application.Dtos;

public record ProductBreakdownDto(
    Guid ProductId,
    string ProductName,
    decimal Revenue,
    decimal GrossProfit,
    int QuantitySold
    );