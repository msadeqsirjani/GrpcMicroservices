var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton(new DiscountDbContext());
builder.Services.AddGrpc();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
