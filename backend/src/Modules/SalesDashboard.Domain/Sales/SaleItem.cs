namespace SalesDashboard.Domain.Entities;

public class SaleItem
{
    public Guid Id { get; private set; }
    public Guid SaleId { get; private set; }
    public Guid ProductId { get; private set; }
    public int Quantity { get; private set; }
    
    // зачем эти поля нужны?
    // разве UnitPrice назначается не в Product?
    public Money UnitPrice { get; private set; }
    public Money UnitCost { get; private set; }
    
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
    
    public void AssignToSale(Guid saleId) => SaleId = saleId;

    // Зачем нужный 2 метода
    public decimal LineRevenue => UnitPrice.Amount * Quantity;
    public decimal LineCost => UnitCost.Amount * Quantity;
}