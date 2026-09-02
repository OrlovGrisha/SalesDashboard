namespace SalesDashboard.Application.Common;

public class PeriodResolver
{
    public DateRange Resolve(PeriodPreset preset, DateTime? customFrom, DateTime? customTo)
    {
        var now = DateTime.UtcNow;
        var today = now.Date;
        
        return preset switch
        {
            PeriodPreset.Today => new DateRange(today, today.AddDays(1)),
            PeriodPreset.Last7Days => new DateRange(today.AddDays(-7), today.AddDays(1)),
            PeriodPreset.Last30Days => new DateRange(today.AddDays(-30), today.AddDays(1)),
            PeriodPreset.ThisMonth => new DateRange(new DateTime(today.Year, today.Month, 1), today.AddDays(1)),
            PeriodPreset.LastMonth => GetLastMonth(today),
            
            PeriodPreset.Custom => new DateRange(
                customFrom ?? throw new ArgumentNullException(nameof(customFrom)),
                customTo ?? throw new ArgumentNullException(nameof(customTo))),
            _ => throw new ArgumentOutOfRangeException(nameof(preset))
        };
    }

    private static DateRange GetLastMonth(DateTime today)
    {
        var firstOfThisMonth = new DateTime(today.Year, today.Month, 1);
        var firstOfLastMonth = firstOfThisMonth.AddMonths(-1);
        return new DateRange(firstOfLastMonth, firstOfThisMonth);
    }
}