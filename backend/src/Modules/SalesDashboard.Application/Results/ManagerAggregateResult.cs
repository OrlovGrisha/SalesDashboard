namespace SalesDashboard.Application.Results;

public record ManagerAggregateResult(Guid ManagerId, decimal Revenue, decimal Cost, int Count);