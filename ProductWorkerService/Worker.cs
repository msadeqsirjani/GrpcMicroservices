namespace ProductWorkerService;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly WorkerServiceSetting _options;
    private readonly ProductFactory _factory;

    public Worker(ILogger<Worker> logger, IOptions<WorkerServiceSetting> options, ProductFactory factory)
    {
        _logger = logger;
        _factory = factory;
        _options = options.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("{worker} running at: {time}", _options.ServiceName, DateTimeOffset.Now);

            await _factory.CreateProductAsync();

            await Task.Delay(_options.Interval, stoppingToken);
        }
    }
}