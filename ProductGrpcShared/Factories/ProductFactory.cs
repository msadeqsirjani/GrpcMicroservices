namespace ProductGrpcShared.Factories;

public class ProductFactory
{
    private readonly string _serverUrl;

    public ProductFactory(string serverUrl)
    {
        _serverUrl = serverUrl;
    }

    public async Task GetProductsAsync()
    {
        using var channel = GrpcChannel.ForAddress(_serverUrl);
        var client = new ProductService.ProductServiceClient(channel);

        const int maxRetryAttempts = 10;
        var pauseBetweenFailures = TimeSpan.FromSeconds(3);

        await Policy
            .Handle<RpcException>()
            .WaitAndRetryAsync(maxRetryAttempts, _ => pauseBetweenFailures, (exception, pause) =>
            {
                Console.WriteLine($"{exception.Message} => {pause.TotalSeconds}");
            })
            .ExecuteAsync(async () =>
            {
                using var call = client.GetProducts(new GetProductsRequest()
                {
                    PageNo = 1,
                    PageSize = 10
                });

                await foreach (var product in call.ResponseStream.ReadAllAsync())
                {
                    Console.WriteLine(product);
                }
            });
    }

    public async Task GetProductAsync()
    {
        using var channel = GrpcChannel.ForAddress(_serverUrl);
        var client = new ProductService.ProductServiceClient(channel);

        const int maxRetryAttempts = 10;
        var pauseBetweenFailures = TimeSpan.FromSeconds(3);

        await Policy
            .Handle<RpcException>()
            .WaitAndRetryAsync(maxRetryAttempts, _ => pauseBetweenFailures, (exception, pause) =>
            {
                Console.WriteLine($"{exception.Message} => {pause.TotalSeconds}");
            })
            .ExecuteAsync(async () =>
            {
                var product = await client.GetProductAsync(new GetProductRequest
                {
                    Id = "89769350-a864-4939-9d58-6302513b7250"
                });

                Console.WriteLine(product);
            });
    }

    public async Task CreateProductAsync()
    {
        using var channel = GrpcChannel.ForAddress(_serverUrl);
        var client = new ProductService.ProductServiceClient(channel);

        const int maxRetryAttempts = 10;
        var pauseBetweenFailures = TimeSpan.FromSeconds(3);

        await Policy
            .Handle<RpcException>()
            .WaitAndRetryAsync(maxRetryAttempts, _ => pauseBetweenFailures, (exception, pause) =>
            {
                Console.WriteLine($"{exception.Message} => {pause.TotalSeconds}");
            })
            .ExecuteAsync(async () =>
            {
                var product = ProductMock.SeedSingle();
                var productRequest = product.Adapt<CreateProductRequest>();
                var response = await client.CreateProductAsync(productRequest);

                Console.WriteLine(response);
            });
    }

    public async Task CreateProductsAsync()
    {
        using var channel = GrpcChannel.ForAddress(_serverUrl);
        var client = new ProductService.ProductServiceClient(channel);

        const int maxRetryAttempts = 10;
        var pauseBetweenFailures = TimeSpan.FromSeconds(3);

        await Policy
            .Handle<RpcException>()
            .WaitAndRetryAsync(maxRetryAttempts, _ => pauseBetweenFailures, (exception, pause) =>
            {
                Console.WriteLine($"{exception.Message} => {pause.TotalSeconds}");
            })
            .ExecuteAsync(async () =>
            {
                var products = ProductMock.Seed();

                using var call = client.CreateProducts();

                foreach (var product in products)
                {
                    await call.RequestStream.WriteAsync(product.Adapt<CreateProductRequest>());
                }

                await call.RequestStream.CompleteAsync();
                var response = await call;

                Console.WriteLine(response);
            });
    }

    public async Task UpdateProductAsync()
    {
        using var channel = GrpcChannel.ForAddress(_serverUrl);
        var client = new ProductService.ProductServiceClient(channel);

        const int maxRetryAttempts = 10;
        var pauseBetweenFailures = TimeSpan.FromSeconds(3);

        await Policy
            .Handle<RpcException>()
            .WaitAndRetryAsync(maxRetryAttempts, _ => pauseBetweenFailures, (exception, pause) =>
            {
                Console.WriteLine($"{exception.Message} => {pause.TotalSeconds}");
            })
            .ExecuteAsync(async () =>
            {
                var response = await client.UpdateProductAsync(new UpdateProductRequest
                {
                    Id = "89769350-a864-4939-9d58-6302513b7250",
                    Title = "Samsung S21 FE",
                    Description =
                        "The Samsung Galaxy S21 FE comes with 6.4-inch AMOLED display with 120Hz refresh rate and Qualcomm Snapdragon 888 processor. Specs also include 4500mAh battery ...",
                    Price = 751,
                    Status = ProductStatus.None
                });

                Console.WriteLine(response);
            });
    }

    public async Task DeleteProductAsync()
    {
        using var channel = GrpcChannel.ForAddress(_serverUrl);
        var client = new ProductService.ProductServiceClient(channel);

        const int maxRetryAttempts = 10;
        var pauseBetweenFailures = TimeSpan.FromSeconds(3);

        await Policy
            .Handle<RpcException>()
            .WaitAndRetryAsync(maxRetryAttempts, _ => pauseBetweenFailures, (exception, pause) =>
            {
                Console.WriteLine($"{exception.Message} => {pause.TotalSeconds}");
            })
            .ExecuteAsync(async () =>
            {
                var response = await client.DeleteProductAsync(new DeleteProductRequest
                {
                    Id = "D81438AB-CAE1-457C-B588-60BC0D2E57C5",
                });

                Console.WriteLine(response);
            });
    }
}