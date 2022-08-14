namespace ProductGrpcShared.Options;

public class WorkerServiceSetting
{
    public string ServiceName { get; set; } = null!;
    public int Interval { get; set; }
    public string ServerUrl { get; set; } = null!;
}