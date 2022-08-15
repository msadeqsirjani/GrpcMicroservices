namespace DiscountGrpc.Services;

public class DiscountService : Protos.DiscountService.DiscountServiceBase
{
    private readonly DiscountDbContext _context;

    public DiscountService(DiscountDbContext context)
    {
        _context = context;
    }

    public override Task<Empty> Available(Empty request, ServerCallContext context)
    {
        return Task.FromResult(new Empty());
    }

    public override Task<GetDiscountResponse?> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        var discount = _context.Discounts.FirstOrDefault(x => x.Code == request.Discount);

        var discountResponse = discount?.Adapt<GetDiscountResponse>();

        return Task.FromResult(discountResponse);
    }
}