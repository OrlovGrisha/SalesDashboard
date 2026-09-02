namespace SalesDashboard.Infrastructure.Seed.Data;

public enum ManagerStrength { Weak, Average, Strong, Star }

public record ManagerProfile(
    string FullName,
    string Team,
    ManagerStrength Strength,
    bool HasGapPeriod // для edge case "период без продаж у отдельного менеджера"
);