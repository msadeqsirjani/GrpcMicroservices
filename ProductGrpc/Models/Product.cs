namespace ProductGrpc.Models;

public class Product : Entity
{
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public ProductStatus Status { get; set; }
}