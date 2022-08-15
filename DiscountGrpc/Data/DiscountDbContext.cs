namespace DiscountGrpc.Data;

public class DiscountDbContext
{
    private readonly IEnumerable<Discount> _discounts;

    public DiscountDbContext()
    {
        _discounts = new List<Discount>(10);

        _discounts = DiscountMock.Seed();
    }
}