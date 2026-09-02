using Microsoft.AspNetCore.Mvc;
using SalesDashboard.Application.Common;
using SalesDashboard.Application.Dtos;
using SalesDashboard.Application.Services;

namespace SalesDashboard.Api.Controllers;

[ApiController]
[Route("api/managers")]
public class ManagersController : ControllerBase
{
    private readonly IManagerRankingService _ranking;
    private readonly PeriodResolver _periodResolver;

    public ManagersController(IManagerRankingService ranking, PeriodResolver periodResolver)
    {
        _ranking = ranking;
        _periodResolver = periodResolver;
    }

    [HttpGet("ranking")]
    public async Task<ActionResult<IReadOnlyList<ManagerRankingItemDto>>> GetRanking(
        [FromQuery] PeriodPreset preset = PeriodPreset.Last30Days,
        [FromQuery] DateTime? from = null,
        [FromQuery] DateTime? to = null,
        [FromQuery] RankingMode mode = RankingMode.GrossProfit,
        CancellationToken ct = default)
    {
        if (preset == PeriodPreset.Custom && (from is null || to is null))
            return BadRequest("Both 'from' and 'to' are required when preset is 'Custom'.");

        if (from is not null && to is not null && from > to)
            return BadRequest("'from' cannot be later than 'to'.");

        var period = _periodResolver.Resolve(preset, from, to);
        var result = await _ranking.GetRankingAsync(period, mode, ct);
        return Ok(result);
    }
}