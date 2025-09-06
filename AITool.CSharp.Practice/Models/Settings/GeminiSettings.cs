namespace AITool.CSharp.Practice.Models.Settings;

/// <summary>
/// Gemini 配置設定
/// </summary>
public class GeminiSettings
{
    /// <summary>
    /// GitHub API 金鑰
    /// </summary>
    public string ApiKey { get; set; } = string.Empty;

    /// <summary>
    /// 模型名稱
    /// </summary>
    public required string ModelId { get; set; }
}
