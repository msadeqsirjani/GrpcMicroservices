var provider = new ProductFactory("https://localhost:7036");

await provider.GetProductsAsync();
await provider.GetProductAsync();
await provider.CreateProductAsync();
await provider.CreateProductsAsync();
await provider.UpdateProductAsync();
await provider.DeleteProductAsync();

Console.ReadLine();