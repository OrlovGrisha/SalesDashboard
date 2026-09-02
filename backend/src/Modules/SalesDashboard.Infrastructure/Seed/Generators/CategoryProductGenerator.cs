using SalesDashboard.Domain;
using SalesDashboard.Domain.Catalog;
using SalesDashboard.Infrastructure.Seed.Data;

namespace SalesDashboard.Infrastructure.Seed.Generators;

public class CategoryProductGenerator
{
    private readonly SeedRandom _random;
    public CategoryProductGenerator(SeedRandom random) => _random = random;

    public (List<Category> Categories, List<Product> Products) Generate()
    {
        var categories = new List<Category>();
        var products = new List<Product>();

        foreach (var (categoryName, productNames) in SeedNames.Catalog)
        {
            var category = Category.Create(categoryName);
            categories.Add(category);

            foreach (var productName in productNames)
            {
                var basePrice = _random.Instance.Next(50, 3000); // разброс цен для разной маржинальности
                var marginPercent = _random.Instance.Next(15, 45); // 15-45% маржа
                var baseCost = basePrice * (100 - marginPercent) / 100m;

                products.Add(Product.Create(
                    productName,
                    category.Id,
                    Money.Of(basePrice),
                    Money.Of(Math.Round(baseCost, 2))));
            }
        }

        return (categories, products);
    }
}