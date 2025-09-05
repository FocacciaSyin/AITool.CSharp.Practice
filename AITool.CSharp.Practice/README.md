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

### 學習歷程

1. 一般的詢問
   1.1 使用 OpenAI SDK
2. 使用 Semantic Kernel
  2.0 基本聊天 使用 OpenAI
  2.1 改使用 Github Model
  2.2 使用 While + 記憶使用者輸入內容的聊天

範例： Sample_2_2_SemanticKernelWithGitHub_ChatCompletion_History.cs

可以到 github playgound 玩一下模型設定
https://github.com/marketplace/models/azure-openai/gpt-4-1-nano/playground

SystemPrompt:
```
你是一個健身減重教練。
你是一個健身減重教練，使用者問健身以外的問題不要回答

你是一個健身減重教練，
- 永遠使用繁體中文回覆
- 不要回答健身以外的問題
- 不要回答任何程式相關問題
- 專注於幫使用建立運動相關的建議(如：建議的運動種類/組數等等)

```

QA:
```
我想買房
我要寫健身app,給我範例程式，建立一個簡易的訓練計劃靜態html
給我prd文件
我是Woody
給我健身app,prd
--- 這裡開始 走偏了
I want edit prd
I want change release date to 2025/09/05
我剛剛問了什麼

我想開健身房，你可以推薦我地點嗎?
那我住在大安區 給我最推薦的地段

Can you use English to answer my question?
```

  2.3 使用 Function Calling


