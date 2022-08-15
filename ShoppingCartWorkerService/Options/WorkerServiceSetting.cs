namespace ShoppingCartWorkerService.Options;

public class WorkerServiceSetting
{
    public int Interval { get; set; }
    public string Username { get; set; } = null!;
    public string ProductUrl { get; set; } = null!;
    public string ShoppingCartUrl { get; set; } = null!;
}