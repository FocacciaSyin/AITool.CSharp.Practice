using System.ComponentModel.DataAnnotations;

namespace AITool.CSharp.Practice.Models.Settings;

/// <summary>
/// OpenAI 配置設定
/// </summary>
public class OpenAISettings
{
    /// <summary>
    /// OpenAI API 金鑰
    /// </summary>
    [Required(ErrorMessage = "OpenAI API 金鑰為必填")]
    public string ApiKey { get; set; } = string.Empty;
    
    /// <summary>
    /// OpenAI 模型名稱
    /// </summary>
    public string Model { get; set; } = "gpt-3.5-turbo";
}