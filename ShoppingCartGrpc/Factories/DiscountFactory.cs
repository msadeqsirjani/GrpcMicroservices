namespace ShoppingCartGrpc.Factories;

public class DiscountFactory
{
    private readonly DiscountService.DiscountServiceClient _client;

    public DiscountFactory(DiscountService.DiscountServiceClient client)
    {
        _client = client;
    }

    public async Task<int> GetDiscount(string discountCode)
    {
        var discount = await _client.GetDiscountAsync(new GetDiscountRequest() {Discount = discountCode});

        return discount.Amount;
    }
}