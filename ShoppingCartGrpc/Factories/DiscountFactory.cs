namespace ShoppingCartGrpc.Factories;

public class DiscountFactory
{
    public async Task<int> GetDiscount(string discountCode)
    {
        using var channel = GrpcChannel.ForAddress("https://localhost:7091");
        var client = new DiscountService.DiscountServiceClient(channel);

        var discount = await client.GetDiscountAsync(new GetDiscountRequest() {Discount = discountCode});

        return discount.Amount;
    }
}