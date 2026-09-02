using SalesDashboard.Application.Common;
using SalesDashboard.Application.Dtos;
using SalesDashboard.Application.Interfaces;

namespace SalesDashboard.Application.Services;

public class AnalyticsService : IAnalyticsService
{
    private readonly ISaleRepository _saleRepository;
    private readonly IManagerRepository _managerRepository;
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ICustomerRepository _customerRepository;
    public AnalyticsService(
        ISaleRepository saleRepository, 
        IManagerRepository managerRepository, 
        IProductRepository productRepository, 
        ICategoryRepository categoryRepository, 
        ICustomerRepository customerRepository)
    {
        _saleRepository = saleRepository;
        _managerRepository = managerRepository;
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _customerRepository = customerRepository;
    }

    public async Task<KpiSummaryDto> GetKpiSummaryAsync(DateRange period, CancellationToken cancellationToken)
    {
        var current = await _saleRepository.GetAggregateAsync(period, cancellationToken);
        var previous = await _saleRepository.GetAggregateAsync(period.PreviousPeriod,  cancellationToken);
        
        var grossProfit = current.Revenue - current.Cost;
        var averageCheck = current.Cost > 0 ? current.Revenue - current.Cost : 0;
        
        var prevGrossProfit = previous.Revenue - previous.Cost;
        var prevAverageCheck = previous.Cost > 0 ? previous.Revenue / previous.Cost : 0;

        var byManager = await _saleRepository.GetAggregateByManagerAsync(period, cancellationToken);
        string? topManagerName = null;
        if (byManager.Count > 0)
        {
            var topManagerId = byManager
                .OrderByDescending(m => m.Revenue - m.Cost)
                .First().ManagerId;
            var manager = await _managerRepository.GetByIdAsync(topManagerId, cancellationToken);
            topManagerName = manager?.FullName;
        }

        return new KpiSummaryDto(
            Revenue: current.Revenue,
            GrossProfit: grossProfit,
            Margin: current.Revenue > 0 ? grossProfit / current.Revenue : 0,
            SalesCount: current.Count,
            AverageCheck: averageCheck,
            TopManagerName: topManagerName,
            Comparison: new KpiComparisonDto(
                RevenueChangePercent: PercentChange(previous.Revenue, current.Revenue),
                GrossProfitChangePercent: PercentChange(prevGrossProfit, grossProfit),
                SalesCountChangePercent: PercentChange(previous.Count, current.Count),
                AverageCheckChangePercent: PercentChange(prevAverageCheck, averageCheck)
                )
            );
    }

    public async Task<IReadOnlyList<SalesTrendPointDto>> GetSalesTrendAsync(DateRange period, TrendGranularity granularity, CancellationToken cancellationToken)
    {
        var points = await _saleRepository.GetTrendAsync(period, granularity, cancellationToken);

        return points
            .Select(p => new SalesTrendPointDto(
                Revenue: p.Revenue,
                PeriodStart: p.PeriodStart,
                GrossProfit: p.Revenue - p.Cost,
                SalesCount: p.Count))
            .OrderBy(p => p.PeriodStart)
            .ToList();
    }

    public async Task<IReadOnlyList<CategoryBreakdownDto>> GetCategoryBreakdownAsync(DateRange period, CancellationToken ct)
    {
        var aggregates = await _saleRepository.GetAggregateByCategoryAsync(period, ct);
        var categories = await _categoryRepository.GetByIdsAsync(aggregates.Select(a => a.CategoryId), ct);
        
        return aggregates.Select(a => new CategoryBreakdownDto(
            CategoryId: a.CategoryId,
            CategoryName: categories.TryGetValue(a.CategoryId, out var c) ? c.Name : "Unknown",
            Revenue: a.Revenue,
            GrossProfit: a.Revenue - a.Cost,
            SalesCount: a.Count))
            .OrderBy(a => a.Revenue)
            .ToList();
    }

    public async Task<IReadOnlyList<ProductBreakdownDto>> GetTopProductsAsync(DateRange period, int take, CancellationToken ct)
    {
        var aggregates = await _saleRepository.GetTopProductsAsync(period, take, ct);
        var products = await _productRepository.GetAllWithCategoryAsync(ct);

        return aggregates
            .Select(a =>
            {
                products.TryGetValue(a.ProductId, out var product);
                return new ProductBreakdownDto(
                    ProductId: a.ProductId,
                    ProductName: product?.Name ?? "Unknown",
                    Revenue: a.Revenue,
                    GrossProfit: a.Revenue - a.Cost,
                    QuantitySold: a.QuantitySold);
            })
            .OrderByDescending(p => p.Revenue)
            .ToList();
    }

    public async Task<IReadOnlyList<RecentSaleDto>> GetRecentSalesAsync(DateRange period, int take, CancellationToken ct)
    {
        var sales = await _saleRepository.GetRecentAsync(period, take, ct);

        var managerIds = sales.Select(s => s.ManagerId).Distinct();
        var customerIds = sales.Select(s => s.CustomerId).Distinct();
        var productIds = sales.SelectMany(s => s.Items.Select(i => i.ProductId)).Distinct();

        var managers = await _managerRepository.GetByIdsAsync(managerIds, ct);
        var customers = await _customerRepository.GetByIdsAsync(customerIds, ct);
        var products = await _productRepository.GetByIdsAsync(productIds, ct);

        return sales
            .Select(s => new RecentSaleDto(
                SaleId: s.Id,
                SaleDate: s.SaleDate,
                ManagerName: managers.TryGetValue(s.ManagerId, out var m) ? m.FullName : "Unknown",
                CustomerName: customers.TryGetValue(s.CustomerId, out var cu) ? cu.Name : "Unknown",
                CustomerCompany: customers.TryGetValue(s.CustomerId, out var cu2) ? cu2.Company : "",
                StatusLabel: s.Status.ToString(),
                ProductNames: s.Items
                    .Select(i => products.TryGetValue(i.ProductId, out var p) ? p.Name : "Unknown")
                    .ToList(),
                Revenue: s.Revenue,
                GrossProfit: s.GrossProfit))
            .OrderByDescending(s => s.SaleDate)
            .ToList();
    }
    
    private static decimal PercentChange(decimal previous, decimal current) =>
        previous > 0 ? (current - previous) / previous * 100 : 0;

    private static decimal PercentChange(int previous, int current) =>
        previous > 0 ? (decimal)(current - previous) / previous * 100 : 0;
}