namespace Common.Kestrel;

public class KestrelOptions
{
    internal const string ConfigurationSection = "Kestrel";
    
    public int ApiPort { get; init; }

    public int GRpcPort { get; init; } = 0;
    
    public string? LocalNetworkIp { get; init; }
}
