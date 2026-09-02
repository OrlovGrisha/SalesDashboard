namespace SalesDashboard.Application.Results;

public record CategoryAggregateResult(Guid CategoryId, decimal Revenue, decimal Cost, int Count);