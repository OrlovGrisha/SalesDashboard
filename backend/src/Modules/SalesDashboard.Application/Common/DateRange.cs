namespace SalesDashboard.Application.Common;

public struct DateRange
{
    public DateTime From { get; private set; }
    public DateTime To { get; private set; }
    
    public DateRange(DateTime from, DateTime to)
    {
        From = from;
        To = to;
    }
    
    public TimeSpan Duration => To - From;
    public DateRange PreviousPeriod => new DateRange(From - Duration, From);
}