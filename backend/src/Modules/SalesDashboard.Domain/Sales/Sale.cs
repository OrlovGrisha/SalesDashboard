using SalesDashboard.Domain.Enums;

namespace SalesDashboard.Domain.Entities;

public class Sale
{
    private readonly List<SaleItem> _items = new();
    
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public Guid ManagerId { get; set; }
    public DateTime SaleDate { get; set; }
    public SaleStatus Status { get; set; }
    public IReadOnlyCollection<SaleItem> Items => _items.AsReadOnly();

    private Sale() { }

    private Sale(Guid managerId, Guid customerId, DateTime saleDate)
    {
        Id = Guid.NewGuid();
        ManagerId = managerId;
        CustomerId = customerId;
        SaleDate = saleDate;
        Status = SaleStatus.Paid;
    }

    public static Sale Create(Guid managerId, Guid customerId, DateTime saleDate)
    {
        if (saleDate > DateTime.UtcNow)
            throw new DomainException("Sale date cannot be in the future");

        return new Sale(managerId, customerId, saleDate);
    }

    public void AddItem(SaleItem item)
    {
        if (Status != SaleStatus.Paid)
            throw new DomainException("Cannot add items to a sale that is not in Paid status");
        
        item.AssignToSale(Id);
        _items.Add(item);
    }

    public void Cancel()
    {
        // в чем смысл? а не нужна проверка на cancelled?
        if (Status == SaleStatus.Refunded)
            throw new DomainException("Cannot cancel a refunded sale");
        if (Status == SaleStatus.Cancelled)
            throw new DomainException("Sale is already cancelled");
        
        Status = SaleStatus.Cancelled;
    }

    public void Refund()
    {
        if (Status != SaleStatus.Paid)
            throw new DomainException("Cannot refund unpaid sale");
        
        
        Status = SaleStatus.Refunded;
    }
    
    // Business rule: Cancelled and Refunded both excluded from Revenue/GrossProfit.
    // Refund amount tracked separately for its own metric.
    public bool IsRevenueGenerating => Status == SaleStatus.Paid;
    
    public decimal Revenue => IsRevenueGenerating ? _items.Sum(i => i.LineRevenue) : 0m;
    public decimal Cost => IsRevenueGenerating ? _items.Sum(i => i.LineCost) : 0m;
    public decimal GrossProfit => Revenue - Cost;
    
    public decimal RefundedAmount => Status == SaleStatus.Refunded
        ? _items.Sum(i => i.LineRevenue)
        : 0m;
}