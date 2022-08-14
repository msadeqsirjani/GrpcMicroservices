namespace ProductGrpc.Services;

public class ProductService : ProductGrpc.ProductService.ProductServiceBase
{
    private readonly ProductDbContext _context;
    private readonly ILogger<ProductService> _logger;

    public ProductService(ProductDbContext context, ILogger<ProductService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public override Task<Empty> Available(Empty request, ServerCallContext context)
    {
        return Task.FromResult(new Empty());
    }

    public override async Task GetProducts(GetProductsRequest request, IServerStreamWriter<GetProductResponse> responseStream, ServerCallContext context)
    {
        if (request.PageNo < 0) request.PageNo = 1;
        if (request.PageSize < 0) request.PageSize = 10;

        TypeAdapterConfig<Product, GetProductResponse>
            .NewConfig()
            .Map(destination => destination.CreatedAt, src => Timestamp.FromDateTime(src.CreatedAt))
            .Map(destination => destination.ModifiedAt, src => Timestamp.FromDateTime(src.ModifiedAt));

        var products = _context
            .Products
            .Skip((request.PageNo - 1) * request.PageSize)
            .Take(request.PageSize)
            .ProjectToType<GetProductResponse>();

        foreach (var product in products)
        {
            await responseStream.WriteAsync(product);
        }
    }

    public override async Task<GetProductResponse?> GetProduct(GetProductRequest request, ServerCallContext context)
    {
        TypeAdapterConfig<Product, GetProductResponse>
            .NewConfig()
            .Map(destination => destination.CreatedAt, src => Timestamp.FromDateTime(src.CreatedAt))
            .Map(destination => destination.ModifiedAt, src => Timestamp.FromDateTime(src.ModifiedAt));

        var product = await _context
            .Products
            .ProjectToType<GetProductResponse>()
            .FirstOrDefaultAsync(x => x.Id.ToString() == request.Id);

        return product;
    }

    public override async Task<CreateProductResponse> CreateProduct(CreateProductRequest request, ServerCallContext context)
    {
        TypeAdapterConfig<CreateProductRequest, Product>
            .NewConfig()
            .Map(destination => destination.Id, src => Guid.NewGuid())
            .Map(destination => destination.CreatedAt, src => DateTime.UtcNow)
            .Map(destination => destination.ModifiedAt, src => DateTime.UtcNow);

        var product = request.Adapt<Product>();

        await _context.AddAsync(product);
        await _context.SaveChangesAsync();

        return new CreateProductResponse { Id = product.Id.ToString() };
    }

    public override async Task<CreateProductsResponse> CreateProducts(IAsyncStreamReader<CreateProductRequest> requestStream, ServerCallContext context)
    {
        TypeAdapterConfig<CreateProductRequest, Product>
            .NewConfig()
            .Map(destination => destination.Id, src => Guid.NewGuid())
            .Map(destination => destination.CreatedAt, src => DateTime.UtcNow)
            .Map(destination => destination.ModifiedAt, src => DateTime.UtcNow);

        await foreach (var productDto in requestStream.ReadAllAsync())
        {
            var product = productDto.Adapt<Product>();

            await _context.AddAsync(product);

            _logger.LogInformation(JsonSerializer.Serialize(product));
        }

        await _context.SaveChangesAsync();

        return new CreateProductsResponse { Message = "Product.AddRange.Success" };
    }

    public override async Task<UpdateProductResponse> UpdateProduct(UpdateProductRequest request, ServerCallContext context)
    {
        var product = await _context
            .Products
            .FirstOrDefaultAsync(x => x.Id.ToString() == request.Id);

        if (product == null)
            return new UpdateProductResponse { Message = "Product.Update.NotFound" };

        TypeAdapterConfig<UpdateProductRequest, Product>
            .NewConfig()
            .Ignore(destination => destination.Id)
            .Ignore(destination => destination.CreatedAt)
            .Ignore(destination => destination.ModifiedAt);

        product = request.Adapt(product);

        _context.Attach(product);
        await _context.SaveChangesAsync();

        return new UpdateProductResponse { Message = "Product.Update.Success" };
    }

    public override async Task<DeleteProductResponse> DeleteProduct(DeleteProductRequest request, ServerCallContext context)
    {
        var product = await _context
            .Products
            .FirstOrDefaultAsync(x => x.Id.ToString() == request.Id);

        if (product == null)
            return new DeleteProductResponse { Message = "Product.Delete.NotFound" };

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        return new DeleteProductResponse() { Message = "Product.Delete.Success" };
    }
}