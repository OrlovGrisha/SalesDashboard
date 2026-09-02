using SalesDashboard.Domain;
using SalesDashboard.Domain.Catalog;
using SalesDashboard.Domain.Customer;
using SalesDashboard.Domain.Entities;
using SalesDashboard.Domain.Managers;
using SalesDashboard.Infrastructure.Seed.Data;

namespace SalesDashboard.Infrastructure.Seed.Generators;

public class SaleGenerator
{
    private readonly SeedRandom _random;
    public SaleGenerator(SeedRandom random) => _random = random;

    // Множитель силы менеджера — влияет и на частоту сделок, и на их размер
    private static double StrengthMultiplier(ManagerStrength strength) => strength switch
    {
        ManagerStrength.Weak => 0.5,
        ManagerStrength.Average => 1.0,
        ManagerStrength.Strong => 1.6,
        ManagerStrength.Star => 2.3,
        _ => 1.0
    };

    // Сезонность: конец года — пик, лето — спад
    private static double SeasonMultiplier(int month) => month switch
    {
        11 or 12 => 1.35,
        6 or 7 or 8 => 0.7,
        _ => 1.0
    };

    public List<Sale> Generate(
        List<(Manager Manager, ManagerProfile Profile)> managers,
        List<Customer> customers,
        List<Product> products,
        DateTime periodStart,
        DateTime periodEnd,
        int targetCount)
    {
        var sales = new List<Sale>();
        var totalMonths = ((periodEnd.Year - periodStart.Year) * 12) + periodEnd.Month - periodStart.Month + 1;

        // Считаем "вес" каждого менеджера, чтобы распределить targetCount пропорционально силе
        var weights = managers.Select(m => StrengthMultiplier(m.Profile.Strength)).ToList();
        var totalWeight = weights.Sum();

        foreach (var (manager, profile) in managers)
        {
            var managerWeight = StrengthMultiplier(profile.Strength);
            var managerSalesCount = (int)(targetCount * (managerWeight / totalWeight));

            // Gap-период: случайное окно 4-6 недель без продаж где-то в середине периода
            DateTime? gapStart = null, gapEnd = null;
            if (profile.HasGapPeriod)
            {
                var gapWeeks = _random.Instance.Next(4, 7);
                var maxGapStartOffset = (periodEnd - periodStart).Days - (gapWeeks * 7) - 14;
                if (maxGapStartOffset > 0)
                {
                    var offset = _random.Instance.Next(14, maxGapStartOffset);
                    gapStart = periodStart.AddDays(offset);
                    gapEnd = gapStart.Value.AddDays(gapWeeks * 7);
                }
            }

            for (int i = 0; i < managerSalesCount; i++)
            {
                var saleDate = PickSaleDate(periodStart, periodEnd, gapStart, gapEnd);
                if (saleDate is null) continue; // не смогли подобрать дату вне gap — пропускаем

                var customer = customers[_random.Instance.Next(customers.Count)];
                var sale = Sale.Create(manager.Id, customer.Id, saleDate.Value);

                var itemCount = _random.Instance.Next(1, 4); // 1-3 позиции в продаже
                for (int j = 0; j < itemCount; j++)
                {
                    var product = products[_random.Instance.Next(products.Count)];
                    var quantity = _random.Instance.Next(1, 6);

                    // Сильные менеджеры продают крупнее — влияет и на цену (скидка/наценка ±15%), и на quantity
                    var priceVariance = 0.85 + (_random.Instance.NextDouble() * 0.3); // 0.85x - 1.15x
                    var unitPrice = Math.Round(product.BasePrice.Amount * (decimal)priceVariance, 2);
                    var unitCost = product.BaseCost.Amount; // себестоимость не варьируется, только цена продажи

                    sale.AddItem(SaleItem.Create(product.Id, quantity, Money.Of(unitPrice), Money.Of(unitCost)));
                }

                ApplyStatus(sale);
                sales.Add(sale);
            }
        }

        return sales;
    }

    private DateTime? PickSaleDate(DateTime periodStart, DateTime periodEnd, DateTime? gapStart, DateTime? gapEnd)
    {
        for (int attempt = 0; attempt < 10; attempt++)
        {
            var totalDays = (periodEnd - periodStart).Days;

            var randomDay = _random.Instance.Next(0, totalDays);
            var candidate = periodStart.AddDays(randomDay);

            var seasonRoll = _random.Instance.NextDouble();
            var seasonWeight = SeasonMultiplier(candidate.Month) / 1.35;
            if (seasonRoll > seasonWeight) continue;

            if (gapStart.HasValue && candidate >= gapStart.Value && candidate < gapEnd!.Value)
                continue;

            var hour = _random.Instance.Next(9, 19);
            var minute = _random.Instance.Next(0, 60);

            return new DateTime(candidate.Year, candidate.Month, candidate.Day, hour, minute, 0, DateTimeKind.Utc);
        }

        return null;
    }

    private void ApplyStatus(Sale sale)
    {
        var roll = _random.Instance.NextDouble();
        if (roll < 0.85)
        {
            // Paid — статус по умолчанию, ничего не делаем
        }
        else if (roll < 0.93)
        {
            sale.Cancel();
        }
        else
        {
            sale.Refund();
        }
    }
}