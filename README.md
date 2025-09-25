# AITool.CSharp.Practice - Semantic Kernel 專案

這個專案展示如何在 .NET 8 中使用 Microsoft Semantic Kernel 與 OpenAI 服務。

## 🚀 快速開始

### 1. 安裝相依套件

專案已包含以下套件：

- `Microsoft.SemanticKernel` (v1.64.0)
- `Microsoft.SemanticKernel.Agents.Core` (v1.65.0)
- `Microsoft.SemanticKernel.Connectors.Google` (v1.64.0-alpha)
- `Microsoft.ML.Tokenizers` (v1.0.0)
- `Microsoft.ML.Tokenizers.Data.Cl100kBase` (v1.0.0)
- `Microsoft.ML.Tokenizers.Data.O200kBase` (v1.0.0)
- `Microsoft.Extensions.Configuration` (v9.0.8)
- `Microsoft.Extensions.Configuration.Json` (v9.0.8)
- `Microsoft.Extensions.Configuration.EnvironmentVariables` (v9.0.8)
- `Microsoft.Extensions.Configuration.UserSecrets` (v9.0.8)
- `Microsoft.Extensions.DependencyInjection` (v9.0.8)
- `Microsoft.Extensions.Hosting` (v9.0.8)
- `Microsoft.Extensions.Logging.Console` (v9.0.8)

### 2. 設定 OpenAI API 金鑰

#### appsettings.Development.json

修改 `appsettings.Development.json` 檔案，將 `ApiKey` 替換為你的 OpenAI API 金鑰。
建議使用 Secret Manager 來管理敏感資訊。

```json
{
  "OpenAI": {
    "ApiKey": "透過 Secret Manager 設定",
    "Model": "gpt-4.1-nano"
  }
}
```

### 3. 執行專案

```bash
dotnet run --project AITool.CSharp.Practice
```

### 4. OpenAI Model 選擇

請參考 [OpenAI Models 比較文件](https://platform.openai.com/docs/models/compare)

# 📝 TODO - Semantic Kernel 學習歷程規劃

## 1. 基礎

- [x] 1.1 使用 OpenAI SDK (熟悉 API 呼叫)
- [x] 1.2 建立簡單聊天範例
- [x] 1.3 使用 [CSnakes](https://github.com/tonybaloney/csnakes) 執行 [tiktoken](https://github.com/openai/tiktoken) 計算
  Token 數量
- [x] 1.4 計算 Token 數量 SK 官方使用 Microsoft.ML.Tokenizers (支援 GPT-4.1-nano, GPT-4, GPT-4o)
- [x] AITool.CSharp.Practice\Python\ 目錄下建立 Python 範例
    - [x] 建立 token_counter.py > count_tokens 函式
    - [x] 使用 sample_token_counter.py 測試

## 2. Semantic Kernel 基礎

- [x] 2.0 聊天整合 (OpenAI → GitHub Model)
  - [x] 2.0.1 基本聊天回傳必須使用自定義的 C# Model
- [x] 2.1 聊天 (Conversation)
- [x] 2.2 聊天 記憶歷史對話 (Conversation History)
    - [x] 2.2.1 Reducer (多輪對話總結 / 減量)
        - [x] 2.2.1.1 保留前 x 次對話 (Truncation)
        - [x] 2.2.1.2 摘要前 x 次對話 (Summarization)
- [x] 2.3 OpenAI Function Calling
- [X] 2.4 Gemini Function Calling

## 3. Agent 設計

- [ ] 3.1 基本 Agent
- [ ] 3.1 基本 Agent + Function Calling

## 5. 記憶 (Memory)

- [ ] 5.0 短期記憶 (會話上下文) - 要可以 Import / Export
- [ ] 5.1 長期記憶 (Qdrant / MSSQL)
- [ ] 5.2 與登入系統整合 (辨識使用者)

## 6. RAG (檔案 & 外部知識)

- [ ] 6.0 建立 Qdrant Docker 環境
- [ ] 6.1 整合 Semantic Kernel + Qdrant
- [ ] 6.2 PDF → 向量化 & 查詢
- [ ] 6.3 Markdown → 向量化
- [ ] 6.4 股票新聞 RAG 檢索

## 7. AutoGen 範例 (多 Agent 協作)

- [ ] 7.0 [AutoGen](https://microsoft.github.io/autogen/stable/user-guide/agentchat-user-guide/index.html) 基本範例，建立 Python 版本


## 8. 股票顧問應用

- [ ] 8.0 混合式 Agent
    - [ ] 股票顧問 Agent (讀取 MSSQL 大盤資料)
    - [ ] 新聞檢索 Agent (RAG + Qdrant)
    - [ ] 使用者對話 Agent (整合 system prompt + 記憶)
- [ ] 8.1 MSSQL → Agent 自動讀取每日收盤價
- [ ] 8.2 移動平均線策略 (回測)
- [ ] 8.3 布林帶策略 (回測)
- [ ] 8.4 混合式決策 Agent (技術指標 + 新聞情緒)

---

實作多文檔檢索功能。

**參考資料：** [相關教學影片](https://www.youtube.com/watch?v=ujgf9g4ajus)

---

## 🔗 相關連結

- [Microsoft Semantic Kernel 官方文件](https://learn.microsoft.com/en-us/semantic-kernel/)
- [OpenAI API 文件](https://platform.openai.com/docs)
- [Qdrant 向量資料庫](https://qdrant.tech/)
- [GitHub Models Playground](https://github.com/marketplace/models)
 