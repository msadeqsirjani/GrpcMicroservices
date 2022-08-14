var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
builder.Services.AddDbContext<ProductDbContext>(options =>
    options.UseInMemoryDatabase("ProductDb"));

var app = builder.Build();

DatabaseSeed(app);

app.MapGrpcService<ProductService>();
app.MapGet("/", () => "Hello World!");

app.Run();

void DatabaseSeed(IHost webApplication)
{
    var scope = webApplication.Services.CreateScope();

    var context = scope.ServiceProvider.GetRequiredService<ProductDbContext>();

    ProductMock.Seed(context);
}