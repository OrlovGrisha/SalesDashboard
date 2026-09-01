namespace SalesDashboard.Domain.Customer;

public class Customer
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
    public string Company { get; private set; } = null!;
    public string Segment { get; private set; } = null!;
    
    private Customer() { }
    
    private Customer(string name, string company, string segment)
    {
        Id = Guid.NewGuid();
        Name = name;
        Company = company;
        Segment = segment;
    }

    public static Customer Create(string name, string company, string segment)
    {
        // без domainException?
        
        return new Customer(name, company, segment);
    }
}