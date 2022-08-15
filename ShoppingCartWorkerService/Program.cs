var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        var configuration = context.Configuration;
        var workerServiceSetting = new WorkerServiceSetting();
        var configurationSection = configuration.GetSection(nameof(WorkerServiceSetting));

        configurationSection.Bind(workerServiceSetting);

        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
