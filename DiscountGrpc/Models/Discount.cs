namespace DiscountGrpc.Models;

public class Discount
{
    public string Code { get; set; } = null!;
    public int Amount { get; set; }
}