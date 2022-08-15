namespace ShoppingCartGrpc.Mocks;

public class ShoppingCartMock
{
    public static void Seed(ShoppingCartDbContext context)
    {
        if (context.ShoppingCarts.Any()) return;

        var carts = Generate();
        context.ShoppingCarts.AddRange(carts);
        context.SaveChanges();
    }

    public static IEnumerable<ShoppingCart> Generate()
    {
        var ids = new[]
        {
            new Guid("0283B3AE-9000-428D-801A-2BF153DCA26B"),
            new Guid("9EA60AD7-10D2-49AD-AF3E-DA2E77E05C92"),
            new Guid("9A95CE1E-E5EC-4792-821D-D2D85584DAB6"),
            new Guid("81F78902-E384-4E1D-9E3B-D4D720320138"),
            new Guid("A30ED444-2872-4B56-ABBA-147E48622377"),
            new Guid("89769350-A864-4939-9D58-6302513B7250")
        };

        var faker = new Faker<ShoppingCart>();

        faker
            .RuleFor(x => x.Id, Guid.NewGuid)
            .RuleFor(x => x.Username, x => x.Person.UserName)
            .RuleFor(x => x.CreatedAt, DateTime.UtcNow)
            .RuleFor(x => x.ModifiedAt, DateTime.UtcNow)
            .RuleFor(x => x.Items, x => x.Make(3, () => new ShoppingCartItem()
            {
                Id = Guid.NewGuid(),
                Quantity = x.Random.Int(1, 50),
                Color = x.Commerce.Color(),
                Price = x.Random.Int(10, 4500),
                ProductId = x.PickRandom(ids),
                ProductName = x.Commerce.ProductName(),
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = DateTime.UtcNow
            }));

        var carts = faker.Generate(50);

        faker
            .RuleFor(x => x.Id, Guid.NewGuid)
            .RuleFor(x => x.Username, x => "msadeqsirjani")
            .RuleFor(x => x.CreatedAt, DateTime.UtcNow)
            .RuleFor(x => x.ModifiedAt, DateTime.UtcNow)
            .RuleFor(x=>x.Items, new List<ShoppingCartItem>());

        var oneCart = faker.Generate(1).First();

        carts.Add(oneCart);

        return carts;
    }
}