namespace ShoppingCartGrpc.Models;

public class ShoppingCartItem : Entity
{
    public int Quantity { get; set; }
    public string Color { get; set; } = null!;
    public decimal Price { get; set; }
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = null!;
    public Guid ShoppingCartId { get; set; }
}