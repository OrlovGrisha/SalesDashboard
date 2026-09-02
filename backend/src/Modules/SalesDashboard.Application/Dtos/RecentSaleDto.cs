namespace SalesDashboard.Application.Dtos;

public record RecentSaleDto(
    Guid SaleId,
    DateTime SaleDate,
    string ManagerName,
    string CustomerName,
    string CustomerCompany,
    string StatusLabel,
    IReadOnlyList<string> ProductNames,
    decimal Revenue,
    decimal GrossProfit
    );