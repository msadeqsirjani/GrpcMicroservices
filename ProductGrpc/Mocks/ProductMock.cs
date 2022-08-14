namespace ProductGrpc.Mocks;

public class ProductMock
{
    public static void Seed(ProductDbContext context)
    {
        if (context.Products.Any()) return;

        var products = GenerateProductMock();
        context.Products.AddRange(products);
        context.SaveChanges();
    }

    private static IEnumerable<Product> GenerateProductMock()
    {
        var faker = new Faker<Product>();
        var statuses = new[] { ProductStatus.InStock, ProductStatus.Low, ProductStatus.None };

        faker
            .RuleFor(x => x.Id, Guid.NewGuid)
            .RuleFor(x => x.Title, x => x.Commerce.ProductName())
            .RuleFor(x => x.Description, x => x.Commerce.ProductDescription())
            .RuleFor(x => x.Price, x => x.Random.Int(10, 4500))
            .RuleFor(x => x.Status, x => x.PickRandom(statuses))
            .RuleFor(x => x.CreatedAt, DateTime.UtcNow)
            .RuleFor(x => x.ModifiedAt, DateTime.UtcNow);

        var products = faker.Generate(50);

        var ids = new[]
        {
            new Guid("0283B3AE-9000-428D-801A-2BF153DCA26B"), 
            new Guid("9EA60AD7-10D2-49AD-AF3E-DA2E77E05C92"),
            new Guid("9A95CE1E-E5EC-4792-821D-D2D85584DAB6"),
            new Guid("81F78902-E384-4E1D-9E3B-D4D720320138"),
            new Guid("A30ED444-2872-4B56-ABBA-147E48622377"),
            new Guid("89769350-A864-4939-9D58-6302513B7250")
        };

        for (var i = 0; i < 6; i++)
            products[i].Id = ids[i];

        return products;
    }
}