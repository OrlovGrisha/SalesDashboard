using Microsoft.AspNetCore.Mvc;
using SalesDashboard.Application.Common;
using SalesDashboard.Application.Dtos;
using SalesDashboard.Application.Services;

namespace SalesDashboard.Api.Controllers;

[ApiController]
[Route("api/analytics")]
public class AnalyticsController : ControllerBase
{
    private readonly IAnalyticsService _analytics;
    private readonly PeriodResolver _periodResolver;

    public AnalyticsController(IAnalyticsService analytics, PeriodResolver periodResolver)
    {
        _analytics = analytics;
        _periodResolver = periodResolver;
    }

    [HttpGet("kpi")]
    public async Task<ActionResult<KpiSummaryDto>> GetKpi(
        [FromQuery] PeriodPreset preset = PeriodPreset.Last30Days,
        [FromQuery] DateTime? from = null,
        [FromQuery] DateTime? to = null,
        CancellationToken ct = default)
    {
        var periodResult = TryResolvePeriod(preset, from, to, out var period);
        if (periodResult is not null) return periodResult;

        var result = await _analytics.GetKpiSummaryAsync(period, ct);
        return Ok(result);
    }

    [HttpGet("trend")]
    public async Task<ActionResult<IReadOnlyList<SalesTrendPointDto>>> GetTrend(
        [FromQuery] PeriodPreset preset = PeriodPreset.Last30Days,
        [FromQuery] DateTime? from = null,
        [FromQuery] DateTime? to = null,
        [FromQuery] TrendGranularity granularity = TrendGranularity.Day,
        CancellationToken ct = default)
    {
        var periodResult = TryResolvePeriod(preset, from, to, out var period);
        if (periodResult is not null) return periodResult;

        var result = await _analytics.GetSalesTrendAsync(period, granularity, ct);
        return Ok(result);
    }

    [HttpGet("categories")]
    public async Task<ActionResult<IReadOnlyList<CategoryBreakdownDto>>> GetCategories(
        [FromQuery] PeriodPreset preset = PeriodPreset.Last30Days,
        [FromQuery] DateTime? from = null,
        [FromQuery] DateTime? to = null,
        CancellationToken ct = default)
    {
        var periodResult = TryResolvePeriod(preset, from, to, out var period);
        if (periodResult is not null) return periodResult;

        var result = await _analytics.GetCategoryBreakdownAsync(period, ct);
        return Ok(result);
    }

    [HttpGet("products/top")]
    public async Task<ActionResult<IReadOnlyList<ProductBreakdownDto>>> GetTopProducts(
        [FromQuery] PeriodPreset preset = PeriodPreset.Last30Days,
        [FromQuery] DateTime? from = null,
        [FromQuery] DateTime? to = null,
        [FromQuery] int take = 10,
        CancellationToken ct = default)
    {
        if (take is < 1 or > 100)
            return BadRequest("Parameter 'take' must be between 1 and 100.");

        var periodResult = TryResolvePeriod(preset, from, to, out var period);
        if (periodResult is not null) return periodResult;

        var result = await _analytics.GetTopProductsAsync(period, take, ct);
        return Ok(result);
    }
    
    private ActionResult? TryResolvePeriod(
        PeriodPreset preset, DateTime? from, DateTime? to, out DateRange period)
    {
        period = default;

        if (preset == PeriodPreset.Custom && (from is null || to is null))
        {
            return BadRequest("Both 'from' and 'to' are required when preset is 'Custom'.");
        }

        if (from is not null && to is not null && from > to)
        {
            return BadRequest("'from' cannot be later than 'to'.");
        }

        period = _periodResolver.Resolve(preset, from, to);
        return null;
    }
}