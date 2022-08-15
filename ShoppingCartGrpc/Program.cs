var builder = WebApplication.CreateBuilder(args);

var discountUri = builder.Configuration.GetValue<string>("DiscountUri");

builder.Services.AddGrpc();
builder.Services.AddGrpcClient<DiscountService.DiscountServiceClient>(options => options.Address = new Uri(discountUri));
builder.Services.AddTransient<DiscountFactory>();
builder.Services.AddDbContext<ShoppingCartDbContext>(options =>
    options.UseInMemoryDatabase("ShoppingCartDb"));

var app = builder.Build();

DatabaseSeed(app);

app.MapGrpcService<ShoppingCartService>();
app.MapGet("/", () => "Hello World!");

app.Run();

void DatabaseSeed(IHost webApplication)
{
    var scope = webApplication.Services.CreateScope();

    var context = scope.ServiceProvider.GetRequiredService<ShoppingCartDbContext>();

    ShoppingCartMock.Seed(context);
}