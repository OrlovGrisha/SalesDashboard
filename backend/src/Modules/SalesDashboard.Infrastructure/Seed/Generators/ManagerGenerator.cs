using SalesDashboard.Domain.Managers;
using SalesDashboard.Infrastructure.Seed.Data;

namespace SalesDashboard.Infrastructure.Seed.Generators;

public class ManagerGenerator
{
    private readonly SeedRandom _random;
    public ManagerGenerator(SeedRandom random) => _random = random;

    public List<(Manager Manager, ManagerProfile Profile)> Generate(int count)
    {
        var result = new List<(Manager, ManagerProfile)>();
        var usedNames = new HashSet<string>();

        // Распределение силы: примерно 15% слабых, 50% средних, 25% сильных, 10% звёзд
        var strengthPool = new List<ManagerStrength>();
        strengthPool.AddRange(Enumerable.Repeat(ManagerStrength.Weak, (int)(count * 0.15)));
        strengthPool.AddRange(Enumerable.Repeat(ManagerStrength.Average, (int)(count * 0.50)));
        strengthPool.AddRange(Enumerable.Repeat(ManagerStrength.Strong, (int)(count * 0.25)));
        while (strengthPool.Count < count) strengthPool.Add(ManagerStrength.Star);

        for (int i = 0; i < count; i++)
        {
            string fullName;
            do
            {
                var first = SeedNames.FirstNames[_random.Instance.Next(SeedNames.FirstNames.Length)];
                var last = SeedNames.LastNames[_random.Instance.Next(SeedNames.LastNames.Length)];
                fullName = $"{first} {last}";
            } while (!usedNames.Add(fullName));

            var team = SeedNames.Teams[_random.Instance.Next(SeedNames.Teams.Length)];
            var strength = strengthPool[i];

            // 2-3 менеджера получают "провал" в продажах — edge case из задания
            var hasGap = i < 3 && _random.Instance.NextDouble() < 0.7;

            var manager = Manager.Create(fullName, team);
            var profile = new ManagerProfile(fullName, team, strength, hasGap);

            result.Add((manager, profile));
        }

        return result;
    }
}