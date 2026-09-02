namespace SalesDashboard.Application.Results;

public record ProductAggregateResult(Guid ProductId, decimal Revenue, decimal Cost, int QuantitySold);