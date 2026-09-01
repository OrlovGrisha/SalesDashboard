namespace SalesDashboard.Domain.Catalog;

public class Category
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    
    private Category() { }

    private Category(string name)
    {
        Id = Guid.NewGuid();
        Name = name;
    }

    public static Category Create(string name)
    {
        return new Category(name);
    }
}