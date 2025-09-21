namespace AITool.CSharp.Practice.Models.Settings;

public class QdrantSettings
{
    public string Host { get; set; } = "localhost";
    public int Port { get; set; } = 6333;
    public string? ApiKey { get; set; }
    public bool UseHttps { get; set; } = false;
    public string CollectionName { get; set; } = "investment-strategy";
}