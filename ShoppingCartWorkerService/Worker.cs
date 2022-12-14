namespace ShoppingCartWorkerService;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly WorkerServiceSetting _options;

    public Worker(ILogger<Worker> logger, IOptions<WorkerServiceSetting> options)
    {
        _logger = logger;
        _options = options.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var token = await GenerateTokenAsync(stoppingToken);

            const int maxRetryAttempts = 10;
            var pauseBetweenFailures = TimeSpan.FromSeconds(3);

            await Policy
                .Handle<RpcException>()
                .WaitAndRetryAsync(maxRetryAttempts, _ => pauseBetweenFailures, (exception, pause) =>
                {
                    Console.WriteLine($"{exception.Message} => {pause.TotalSeconds}");
                })
                .ExecuteAsync(async () => await Do(token, stoppingToken));

            await Task.Delay(_options.Interval, stoppingToken);
        }
    }

    private async Task Do(string token, CancellationToken stoppingToken)
    {
        using var cartChannel = GrpcChannel.ForAddress(_options.ShoppingCartUrl);
        var cartClient = new ShoppingCartService.ShoppingCartServiceClient(cartChannel);

        var headers = new Metadata { new("Authorization", $"Bearer {token}") };
        await CreateShoppingCart(cartClient, headers);

        using var cartStream = cartClient.CreateShoppingCartItem(headers);

        using var productChannel = GrpcChannel.ForAddress(_options.ProductUrl);
        var productClient = new ProductService.ProductServiceClient(productChannel);
        using var products = productClient.GetProducts(new GetProductsRequest
        {
            PageNo = 1,
            PageSize = 100
        });

        var discountCodes = new[]
        {
            "E3CE571D",
            "EE5ECF60",
            "8E8EEA24"
        };

        var colors = new[]
        {
            "Black",
            "White",
            "Red",
            "Green"
        };

        await foreach (var product in products.ResponseStream.ReadAllAsync(stoppingToken))
        {
            var item = new CreateShoppingCartItemRequest
            {
                Username = _options.Username,
                DiscountCode = discountCodes[Random.Shared.Next(discountCodes.Length)],
                Item = new ShoppingCartItemRequest
                {
                    ProductId = product.Id,
                    ProductName = product.Title ?? "Car",
                    Price = product.Price,
                    Color = colors[Random.Shared.Next(colors.Length)],
                    Quantity = 1,
                }
            };

            await cartStream.RequestStream.WriteAsync(item, stoppingToken);
        }

        await cartStream.RequestStream.CompleteAsync();

        var itemResponse = await cartStream;

        Console.WriteLine(itemResponse);
    }

    private async Task CreateShoppingCart(ShoppingCartService.ShoppingCartServiceClient client, Metadata headers)
    {
        await client.CreateShoppingCartAsync(new CreateShoppingCartRequest
        {
            Username = _options.Username
        }, headers);
    }

    private async Task<string> GenerateTokenAsync(CancellationToken stoppingToken)
    {
        var client = new HttpClient();
        var discovery = await client.GetDiscoveryDocumentAsync(_options.IdentityUrl, stoppingToken);
        if (discovery.IsError)
        {
            Console.WriteLine(discovery.Error);

            return string.Empty;
        }

        var token = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest()
        {
            Address = discovery.TokenEndpoint,
            ClientId = _options.ClientId,
            ClientSecret = _options.ClientSecret,
            Scope = _options.Scope
        }, stoppingToken);

        return token.AccessToken;
    }
}