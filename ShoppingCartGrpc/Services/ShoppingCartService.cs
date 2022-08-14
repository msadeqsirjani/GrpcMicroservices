namespace ShoppingCartGrpc.Services;

public class ShoppingCartService : ShoppingCartGrpc.ShoppingCartService.ShoppingCartServiceBase
{
    private readonly ShoppingCartDbContext _context;
    private readonly ILogger<ShoppingCartService> _logger;

    public ShoppingCartService(ShoppingCartDbContext context, ILogger<ShoppingCartService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public override Task<Empty> Available(Empty request, ServerCallContext context)
    {
        return Task.FromResult(new Empty());
    }

    public override async Task<GetShoppingCartResponse?> GetShoppingCart(GetShoppingCartRequest request, ServerCallContext context)
    {
        var cart = await _context.ShoppingCarts
            .AsNoTrackingWithIdentityResolution()
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.Username == request.Username);

        TypeAdapterConfig<ShoppingCart, GetShoppingCartResponse>
            .NewConfig()
            .Map(destination => destination.CreatedAt, src => Timestamp.FromDateTime(src.CreatedAt))
            .Map(destination => destination.ModifiedAt, src => Timestamp.FromDateTime(src.ModifiedAt));

        TypeAdapterConfig<ShoppingCartItem, ShoppingCartItemResponse>
            .NewConfig()
            .Map(destination => destination.CreatedAt, src => Timestamp.FromDateTime(src.CreatedAt))
            .Map(destination => destination.ModifiedAt, src => Timestamp.FromDateTime(src.ModifiedAt));

        return cart?.Adapt<GetShoppingCartResponse>();
    }

    public override async Task<CreateShoppingCartResponse> CreateShoppingCart(CreateShoppingCartRequest request, ServerCallContext context)
    {
        var cart = await _context
            .ShoppingCarts
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.Username == request.Username);

        if (cart == null)
        {
            TypeAdapterConfig<CreateShoppingCartRequest, ShoppingCart>
                .NewConfig()
                .Map(destination => destination.Id, src => Guid.NewGuid());

            cart = request.Adapt<ShoppingCart>();

            await _context.ShoppingCarts.AddAsync(cart);
        }

        TypeAdapterConfig<ShoppingCartItemRequest, ShoppingCartItem>
                    .NewConfig()
                    .Map(destination => destination.Id, src => Guid.NewGuid());

        foreach (var itemDto in request.Items)
        {
            var existItem = cart.Items.FirstOrDefault(x => x.ProductId.ToString() == itemDto.ProductId);
            if (existItem != null)
            {
                existItem.Quantity++;

                _context.ShoppingCartItems.Attach(existItem);
            }
            else
            {
                var item = itemDto.Adapt<ShoppingCartItem>();

                cart.Items.Add(item);
            }
        }

        await _context.SaveChangesAsync();

        return new CreateShoppingCartResponse
        {
            Message = "ShoppingCart.Add.Success"
        };
    }

    public override async Task<CreateShoppingCartItemResponse> CreateShoppingCartItem(IAsyncStreamReader<CreateShoppingCartItemRequest> requestStream, ServerCallContext context)
    {
        TypeAdapterConfig<ShoppingCartItemRequest, ShoppingCartItem>
            .NewConfig()
            .Map(destination => destination.Id, src => Guid.NewGuid());

        ShoppingCart? cart = null;
        await foreach (var itemDto in requestStream.ReadAllAsync())
        {
            cart ??= await _context
                .ShoppingCarts
                .Include(x => x.Items)
                .FirstOrDefaultAsync(x => x.Username == itemDto.Username);

            if (cart == null)
                continue;

            var existItem = cart.Items.FirstOrDefault(x => x.ProductId.ToString() == itemDto.Item.ProductId);
            if (existItem != null)
            {
                existItem.Quantity++;

                _context.ShoppingCartItems.Attach(existItem);
            }
            else
            {
                // TODO : check discount & calculate the item last price -- call gRPC discount service

                var discount = 100;

                var item = itemDto.Adapt<ShoppingCartItem>();

                item.Price -= discount;

                cart.Items.Add(item);
            }
        }

        await _context.SaveChangesAsync();

        return new CreateShoppingCartItemResponse
        {
            Message = "ShoppingCartItem.Add.Success"
        };
    }

    public override async Task<DeleteShoppingCartItemResponse> DeleteShoppingCartItem(DeleteShoppingCartItemRequest request, ServerCallContext context)
    {
        var cart = await _context
            .ShoppingCarts
            .FirstOrDefaultAsync(x => x.Username == request.Username);

        if (cart == null)
            return new DeleteShoppingCartItemResponse
            {
                Message = "ShoppingCart.Delete.NotFound"
            };

        var item = await _context
            .ShoppingCartItems
            .FirstOrDefaultAsync(x => x.ProductId.ToString() == request.Item.ProductId);

        if (item == null)
            return new DeleteShoppingCartItemResponse
            {
                Message = "ShoppingCartItem.Delete.NotFound"
            };

        cart.Items.Remove(item);

        await _context.SaveChangesAsync();

        return new DeleteShoppingCartItemResponse
        {
            Message = "ShoppingCart.Delete.Success"
        };
    }
}