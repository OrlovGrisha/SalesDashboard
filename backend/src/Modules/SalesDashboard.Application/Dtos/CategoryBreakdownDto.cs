namespace SalesDashboard.Application.Dtos;

public record CategoryBreakdownDto(
    Guid CategoryId,
    string CategoryName,
    decimal Revenue,
    decimal GrossProfit,
    int SalesCount
    );