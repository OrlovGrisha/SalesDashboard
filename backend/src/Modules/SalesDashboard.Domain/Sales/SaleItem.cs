namespace SalesDashboard.Domain.Entities;

public class SaleItem
{
    public Guid Id { get; set; }
    public Guid SaleId { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    
    // зачем эти поля нужны?
    // разве UnitPrice назначается не в Product?
    public Money UnitPrice { get; set; }
    public Money UnitCost { get; set; }
    
    private SaleItem() { }
    
    private SaleItem(Guid productId, int quantity, Money unitPrice, Money unitCost)
    {
        Id = Guid.NewGuid();
        ProductId = productId;
        Quantity = quantity;
        UnitPrice = unitPrice;
        UnitCost = unitCost;
    }
    
    public static SaleItem Create(Guid productId, int quantity, Money unitPrice, Money unitCost)
    {
        if (quantity <= 0)
            throw new DomainException("Sale item quantity must be positive");

        return new SaleItem(productId, quantity, unitPrice, unitCost);
    }
    
    // что за 3 поля?
    // почему internal? можно заменить на другой?
    internal void AssignToSale(Guid saleId) => SaleId = saleId;

    public decimal LineRevenue => UnitPrice.Amount * Quantity;
    public decimal LineCost => UnitCost.Amount * Quantity;
}