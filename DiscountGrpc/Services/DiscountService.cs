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

    public override Task<GetDiscountResponse> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        var discount = _context.Discounts.FirstOrDefault(x => x.Code == request.Discount);

        TypeAdapterConfig<Discount, GetDiscountResponse>
            .NewConfig()
            .Map(dest => dest.CreatedAt, src => Timestamp.FromDateTime(src.CreatedAt))
            .Map(dest => dest.ModifiedAt, src => Timestamp.FromDateTime(src.ModifiedAt));

        var discountResponse = (discount ?? new Discount()
        {
            Id = Guid.NewGuid(),
            Code = request.Discount,
            Amount = 0,
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow,
        }).Adapt<GetDiscountResponse>();

        return Task.FromResult(discountResponse);
    }
}