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
            // create shopping cart ✅
            var cart = await CreateShoppingCart();

            Console.WriteLine(cart);
            
            // fetch products from product gRPC with server stream
            // add item into cart with client stream

            await Task.Delay(_options.Interval, stoppingToken);
        }
    }

    private async Task<GetShoppingCartResponse> CreateShoppingCart()
    {
        using var cartChannel = GrpcChannel.ForAddress(_options.ShoppingCartUrl);
        var cartClient = new ShoppingCartService.ShoppingCartServiceClient(cartChannel);

        await cartClient.CreateShoppingCartAsync(new CreateShoppingCartRequest()
        {
            Username = _options.Username
        });

        var cartResponse = await cartClient.GetShoppingCartAsync(new GetShoppingCartRequest()
        {
            Username = _options.Username
        });

        return cartResponse;
    }
}