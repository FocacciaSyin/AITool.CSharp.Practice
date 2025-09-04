namespace AITool.CSharp.Practice.Models;

/// <summary>
/// GitHub 配置設定
/// </summary>
public class GitHubSettings
{
    /// <summary>
    /// GitHub API 金鑰
    /// </summary>
    public string ApiKey { get; set; } = string.Empty;
    
    /// <summary>
    /// GitHub 模型端點
    /// </summary>
    public required string EndPoint { get; set; } 
    
    /// <summary>
    /// 模型名稱
    /// </summary>
    public required string ModelId { get; set; } 
}