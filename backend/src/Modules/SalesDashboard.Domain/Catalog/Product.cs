namespace SalesDashboard.Domain.Catalog;

public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid CategoryId { get; set; }
    public Money BasePrice { get; private set; }
    public Money BaseCost { get; private set; }
    
    private Product() { }

    private Product(string name, Guid categoryId, Money basePrice, Money baseCost)
    {
        Id = Guid.NewGuid();
        Name = name;
        CategoryId = categoryId;
        BasePrice = basePrice;
        BaseCost = baseCost;
    }
    
    public static Product Create(string name, Guid categoryId, Money basePrice, Money baseCost)
    {
        if (baseCost.Amount > basePrice.Amount)
            throw new DomainException("Product cost cannot exceed its price");

        return new Product(name, categoryId, basePrice, baseCost);
    }
}