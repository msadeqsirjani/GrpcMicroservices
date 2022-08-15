namespace DiscountGrpc.Data;

public class DiscountDbContext
{
    public DiscountDbContext()
    {
        Discounts = new List<Discount>(3);

        Discounts = DiscountMock.Seed();
    }

    public IEnumerable<Discount> Discounts { get; }
}