namespace SalesDashboard.Infrastructure.Seed;

public class SeedRandom
{
    private const int FixedSeed = 42;
    public Random Instance { get; } = new(FixedSeed);
}