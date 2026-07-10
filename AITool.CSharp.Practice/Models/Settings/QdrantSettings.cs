using System.ComponentModel.DataAnnotations;

namespace AITool.CSharp.Practice.Models.Settings;

/// <summary>
/// Qdrant 向量資料庫設定
/// </summary>
public class QdrantSettings
{
    /// <summary>
    /// Qdrant 伺服器端點
    /// </summary>
    [Required(ErrorMessage = "Qdrant 伺服器端點為必填")]
    public string Endpoint { get; set; } = "http://localhost:6334";

    /// <summary>
    /// Qdrant API 金鑰（可選）
    /// </summary>
    public string? ApiKey { get; set; }

    /// <summary>
    /// 預設集合名稱
    /// </summary>
    [Required(ErrorMessage = "集合名稱為必填")]
    public string CollectionName { get; set; } = "semantic_kernel_demo";

    /// <summary>
    /// 向量維度（根據使用的嵌入模型決定）
    /// </summary>
    public int VectorSize { get; set; } = 768;

    /// <summary>
    /// 搜尋結果限制數量
    /// </summary>
    public int SearchLimit { get; set; } = 3;

    /// <summary>
    /// 連線逾時時間（秒）
    /// </summary>
    public int TimeoutSeconds { get; set; } = 30;

    /// <summary>
    /// 是否在啟動時檢查連線
    /// </summary>
    public bool HealthCheckOnStartup { get; set; } = true;
}
