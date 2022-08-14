namespace ShoppingCartGrpc.Models;

public class ShoppingCart : Entity
{
    public ShoppingCart()
    {
        Items = new List<ShoppingCartItem>();
    }

    public ShoppingCart(string username) : this()
    {
        Username = username;
    }

    public string Username { get; set; } = null!;

    public decimal TotalPrice
    {
        get
        {
            return !Items.Any() ? 0 : Items.Sum(item => item.Price * item.Quantity);
        }
    }

    public List<ShoppingCartItem> Items { get; set; }
}