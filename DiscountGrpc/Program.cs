var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton(new DiscountDbContext());
builder.Services.AddGrpc();

var app = builder.Build();

app.MapGrpcService<DiscountService>();
app.MapGet("/", () => "Hello World!");

app.Run();
