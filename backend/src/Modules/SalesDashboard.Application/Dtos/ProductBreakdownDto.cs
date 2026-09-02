namespace SalesDashboard.Application.Dtos;

public record ProductBreakdownDto(
    Guid ProductId,
    string ProductName,
    string CategoryName,
    decimal Revenue,
    decimal GrossProfit,
    int QuantitySold
    );