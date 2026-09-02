using SalesDashboard.Domain.Customer;
using SalesDashboard.Infrastructure.Seed.Data;

namespace SalesDashboard.Infrastructure.Seed.Generators;

public class CustomerGenerator
{
    private readonly SeedRandom _random;
    public CustomerGenerator(SeedRandom random) => _random = random;

    public List<Customer> Generate(int count)
    {
        var customers = new List<Customer>();

        for (int i = 0; i < count; i++)
        {
            var first = SeedNames.FirstNames[_random.Instance.Next(SeedNames.FirstNames.Length)];
            var last = SeedNames.LastNames[_random.Instance.Next(SeedNames.LastNames.Length)];
            var contactName = $"{first} {last}";

            var companyBase = SeedNames.CompanyNames[_random.Instance.Next(SeedNames.CompanyNames.Length)];
            var suffix = SeedNames.CompanySuffixes[_random.Instance.Next(SeedNames.CompanySuffixes.Length)];
            var company = $"{suffix} «{companyBase}»";

            var segment = SeedNames.CustomerSegments[_random.Instance.Next(SeedNames.CustomerSegments.Length)];

            customers.Add(Customer.Create(contactName, company, segment));
        }

        return customers;
    }
}