using Microsoft.AspNetCore.Mvc;
using SalesDashboard.Application.Common;
using SalesDashboard.Application.Dtos;
using SalesDashboard.Application.Services;

namespace SalesDashboard.Api.Controllers;

[ApiController]
[Route("api/sales")]
public class SalesController : ControllerBase
{
    private readonly IAnalyticsService _analytics;
    private readonly PeriodResolver _periodResolver;

    public SalesController(IAnalyticsService analytics, PeriodResolver periodResolver)
    {
        _analytics = analytics;
        _periodResolver = periodResolver;
    }

    [HttpGet("recent")]
    public async Task<ActionResult<IReadOnlyList<RecentSaleDto>>> GetRecent(
        [FromQuery] PeriodPreset preset = PeriodPreset.Last30Days,
        [FromQuery] DateTime? from = null,
        [FromQuery] DateTime? to = null,
        [FromQuery] int take = 20,
        CancellationToken ct = default)
    {
        if (take is < 1 or > 200)
            return BadRequest("Parameter 'take' must be between 1 and 200.");

        if (preset == PeriodPreset.Custom && (from is null || to is null))
            return BadRequest("Both 'from' and 'to' are required when preset is 'Custom'.");

        if (from is not null && to is not null && from > to)
            return BadRequest("'from' cannot be later than 'to'.");

        var period = _periodResolver.Resolve(preset, from, to);
        var result = await _analytics.GetRecentSalesAsync(period, take, ct);
        return Ok(result);
    }
}