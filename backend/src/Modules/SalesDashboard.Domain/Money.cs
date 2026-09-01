namespace SalesDashboard.Domain;

public struct Money
{
    public decimal Amount { get; }
    public Money(decimal amount)
    {
        Amount = amount;
    }
    public static Money Zero => new Money(0);
    
    public static Money Of(decimal amount)
    {
        if (amount < 0) throw new DomainException("Money amount cannot be negative");
        return new Money(amount);
    }
    
    // операторы
}