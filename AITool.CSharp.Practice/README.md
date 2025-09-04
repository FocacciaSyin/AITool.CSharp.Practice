# AITool.CSharp.Practice - Semantic Kernel 專案

這個專案展示如何在 .NET 8 中使用 Microsoft Semantic Kernel 與 OpenAI 服務。

## 🚀 快速開始

### 1. 安裝相依套件

專案已包含以下套件：
- `Microsoft.SemanticKernel` (v1.15.0)
- `Microsoft.Extensions.Configuration` (v9.0.8)
- `Microsoft.Extensions.Configuration.Json` (v8.0.0)
- `Microsoft.Extensions.Configuration.EnvironmentVariables` (v8.0.0)
- `Microsoft.Extensions.Logging.Console` (v9.0.7)

### 2. 設定 OpenAI API 金鑰

#### appsettings.Development.json

修改 `appsettings.Development.json` 檔案，將 `ApiKey` 替換為你的 OpenAI API 金鑰。建議使用 Secret Manager 來管理敏感資訊。


```json
{
  "OpenAI": {
    "ApiKey": "透過 Secret Manager 設定",
    "Model": "gpt-3.5-turbo"
  }
}
```

### 3. 執行專案

```bash
dotnet run
```


### 代辦事項
1. 一般的詢問
2. 改使用 Github Model
3. 使用 While + 記憶使用者輸入內容的聊天
4. 使用 Function Calling
