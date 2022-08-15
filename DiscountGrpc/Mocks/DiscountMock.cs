namespace DiscountGrpc.Mocks;

public class DiscountMock
{
    public static IEnumerable<Discount> Seed() => Generate();

    private static IEnumerable<Discount> Generate()
    {
        var discountCodes = new[]
        {
            "E3CE571D",
            "EE5ECF60",
            "8E8EEA24"
        };

        var faker = new Faker<Discount>();

        faker
            .RuleFor(x => x.Id, Guid.NewGuid)
            .RuleFor(x => x.Code, x => x.PickRandom(discountCodes))
            .RuleFor(x => x.Amount, x => x.Random.Int(100, 350))
            .RuleFor(x=>x.CreatedAt, DateTime.UtcNow)
            .RuleFor(x=>x.ModifiedAt, DateTime.UtcNow);

        var discounts = faker.Generate(3);

        return discounts;
    }
}