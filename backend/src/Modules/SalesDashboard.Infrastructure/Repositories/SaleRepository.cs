using Microsoft.EntityFrameworkCore;
using SalesDashboard.Application.Common;
using SalesDashboard.Application.Interfaces;
using SalesDashboard.Application.Results;
using SalesDashboard.Domain.Entities;
using SalesDashboard.Domain.Enums;

namespace SalesDashboard.Infrastructure.Repositories;

public class SaleRepository : ISaleRepository
{
    private readonly AppDbContext _db;
    public SaleRepository(AppDbContext db) => _db = db;
    
    public async Task<SalesAggregateResult> GetAggregateAsync(DateRange period, CancellationToken cancellationToken)
    {
        var query = _db.Sales
            .Where(s => s.Status == SaleStatus.Paid && period.From <= s.SaleDate && period.To > s.SaleDate)
            .SelectMany(s => s.Items)
            .GroupBy(_ => 1)
            .Select(g => new SalesAggregateResult(
                g.Sum(s => s.Quantity * s.UnitPrice.Amount),
                g.Sum(s => s.Quantity * s.UnitCost.Amount),
                g.Select(s => s.SaleId).Distinct().Count()));
        
        Console.WriteLine(query.ToQueryString());

        return await query.FirstOrDefaultAsync(cancellationToken) ?? new SalesAggregateResult(0, 0 , 0);
    }

    /*public async Task<IReadOnlyList<ManagerAggregateResult>> GetAggregateByManagerAsync(
        DateRange period, CancellationToken ct)
    {
        return await _db.Sales
            .Where(s => s.SaleDate >= period.From && s.SaleDate < period.To && s.Status == SaleStatus.Paid)
            .GroupBy(s => s.ManagerId)
            .Select(g => new ManagerAggregateResult(
                g.Key,
                g.Sum(s => s.Items.Sum(i => i.UnitPrice.Amount * i.Quantity)),
                g.Sum(s => s.Items.Sum(i => i.UnitCost.Amount * i.Quantity)),
                g.Count()))
            .ToListAsync(ct);
    }*/

    public async Task<IReadOnlyList<TrendPointResult>> GetTrendAsync(
        DateRange period, TrendGranularity granularity, CancellationToken ct)
    {
        // EF Core/Npgsql can't translate a GroupBy over a computed date-bucket key combined
        // with a nested Sum over the Items navigation in the same query, so the per-sale
        // revenue/cost is aggregated in SQL first and the date-bucket grouping happens in memory.
        var perSale = await _db.Sales
            .Where(s => s.SaleDate >= period.From && s.SaleDate < period.To && s.Status == SaleStatus.Paid)
            .Select(s => new
            {
                s.SaleDate,
                Revenue = s.Items.Sum(i => i.UnitPrice.Amount * i.Quantity),
                Cost = s.Items.Sum(i => i.UnitCost.Amount * i.Quantity),
            })
            .ToListAsync(ct);

        return perSale
            .GroupBy(s => granularity == TrendGranularity.Month
                ? new DateTime(s.SaleDate.Year, s.SaleDate.Month, 1)
                : granularity == TrendGranularity.Week
                    ? s.SaleDate.Date.AddDays(-(((int)s.SaleDate.DayOfWeek + 6) % 7))
                    : s.SaleDate.Date)
            .Select(g => new TrendPointResult(g.Key, g.Sum(s => s.Revenue), g.Sum(s => s.Cost), g.Count()))
            .OrderBy(p => p.PeriodStart)
            .ToList();
    }

    public async Task<IReadOnlyList<CategoryAggregateResult>> GetAggregateByCategoryAsync(
        DateRange period, CancellationToken ct)
    {

        return await (
            from s in _db.Sales
            where s.SaleDate >= period.From && s.SaleDate < period.To && s.Status == SaleStatus.Paid
            from i in s.Items
            join p in _db.Products on i.ProductId equals p.Id
            group i by p.CategoryId into g
            select new CategoryAggregateResult(
                g.Key,
                g.Sum(i => i.UnitPrice.Amount * i.Quantity),
                g.Sum(i => i.UnitCost.Amount * i.Quantity),
                g.Count()))
            .ToListAsync(ct);
    }

    /*public async Task<IReadOnlyList<ProductAggregateResult>> GetTopProductsAsync(
        DateRange period, int take, CancellationToken ct)
    {
        return await (
            from s in _db.Sales
            where s.SaleDate >= period.From && s.SaleDate < period.To && s.Status == SaleStatus.Paid
            from i in s.Items
            group i by i.ProductId into g
            orderby g.Sum(i => i.UnitPrice.Amount * i.Quantity) descending
            select new ProductAggregateResult(
                g.Key,
                g.Sum(i => i.UnitPrice.Amount * i.Quantity),
                g.Sum(i => i.UnitCost.Amount * i.Quantity),
                g.Sum(i => i.Quantity)))
            .Take(take)
            .ToListAsync(ct);
    }*/

    public async Task<IReadOnlyList<ProductAggregateResult>> GetTopProductsAsync(DateRange period, int take, CancellationToken ct)
    {
        const string sql = """
                           SELECT si."ProductId"                            AS "ProductId",
                                  SUM(si.unit_price * si."Quantity")        AS "Revenue",
                                  SUM(si.unit_cost  * si."Quantity")        AS "Cost",
                                  SUM(si."Quantity")                        AS "QuantitySold"
                           FROM sale_items AS si
                           JOIN sales AS s ON s."Id" = si."SaleId"
                           WHERE s."Status" = 'Paid'
                             AND s."SaleDate" >= {0}
                             AND s."SaleDate" <  {1}
                           GROUP BY si."ProductId"
                           ORDER BY SUM(si.unit_price * si."Quantity") DESC
                           LIMIT {2}
                           """;
        
        var result = await _db.Database.SqlQueryRaw<ProductAggregateResult>(sql, period.From, period.To, take)
            .ToListAsync(ct);

        return result;
    }

    public async Task<IReadOnlyList<Sale>> GetRecentAsync(DateRange period, int take, CancellationToken ct)
    {
        return await _db.Sales
            .Include(s => s.Items)
            .Where(s => s.SaleDate >= period.From && s.SaleDate < period.To)
            .OrderByDescending(s => s.SaleDate)
            .Take(take)
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task AddAsync(Sale sale, CancellationToken ct) =>
        await _db.Sales.AddAsync(sale, ct);


    public async Task<IReadOnlyList<ManagerAggregateResult>> GetAggregateByManagerAsync(DateRange period, CancellationToken ct)
    {
        var result = await _db.Sales.Where(s => s.SaleDate >= period.From && s.SaleDate < period.To)
            .GroupBy(s => s.ManagerId)
            .Select(g => new ManagerAggregateResult(
                g.Key, 
                g.Sum(s => s.Items.Sum(item => item.UnitPrice.Amount * item.Quantity)), 
                g.Sum(s => s.Items.Sum(i => i.UnitCost.Amount * i.Quantity)),
                g.Count())
            ).ToListAsync(ct);
            
        return result.AsReadOnly();
    }
}