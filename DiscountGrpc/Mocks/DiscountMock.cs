namespace DiscountGrpc.Mocks;

public class DiscountMock
{
    public static IEnumerable<Discount> Seed() => Generate();

    private static IEnumerable<Discount> Generate()
    {
        var faker = new Faker<Discount>();

        faker
            .RuleFor(x => x.Id, Guid.NewGuid)
            .RuleFor(x => x.Code, x => x.Commerce.Ean8())
            .RuleFor(x => x.Amount, x => x.Random.Int(100, 350))
            .RuleFor(x=>x.CreatedAt, DateTime.UtcNow)
            .RuleFor(x=>x.ModifiedAt, DateTime.UtcNow);

        var discounts = faker.Generate(10);

        return discounts;
    }
}