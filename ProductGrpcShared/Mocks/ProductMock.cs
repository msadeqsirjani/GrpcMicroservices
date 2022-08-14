using ProductGrpcShared.Models;

namespace ProductGrpcShared.Mocks;

public class ProductMock
{
    public static IEnumerable<Product> Seed() => GenerateProductMock(50);
    public static IEnumerable<Product> SeedSingle() => GenerateProductMock(1);

    private static IEnumerable<Product> GenerateProductMock(int count)
    {
        var faker = new Faker<Product>();
        var statuses = new[] { ProductStatus.InStock, ProductStatus.Low, ProductStatus.None };

        faker
            .RuleFor(x => x.Title, x => x.Commerce.ProductName())
            .RuleFor(x => x.Description, x => x.Commerce.ProductDescription())
            .RuleFor(x => x.Price, x => x.Random.Int(10, 4500))
            .RuleFor(x => x.Status, x => x.PickRandom(statuses));

        var products = faker.Generate(count);

        return products;
    }
}