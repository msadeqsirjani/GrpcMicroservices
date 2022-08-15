namespace DiscountGrpc.Models;

public class Discount : Entity
{
    public string Code { get; set; } = null!;
    public int Amount { get; set; }
}