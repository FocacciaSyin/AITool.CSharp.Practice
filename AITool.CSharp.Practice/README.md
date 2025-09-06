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

### 4. OpenAI Model 選擇

請參考 [OpenAI Models 比較文件](https://platform.openai.com/docs/models/compare)

## 📚 學習歷程

### 1. 一般的詢問

#### 1.1 使用 OpenAI SDK
基本的 OpenAI SDK 整合與使用。

### 2. 使用 Semantic Kernel

#### 2.0 基本聊天 - 使用 OpenAI
初步整合 Semantic Kernel 與 OpenAI 服務。

#### 2.1 改使用 GitHub Model
將服務提供者從 OpenAI 切換至 GitHub Model。

#### 2.2 使用 While + 記憶使用者輸入內容的聊天
實作具有記憶功能的連續對話系統。

**範例：** `Sample_2_2_SemanticKernelWithGitHub_ChatCompletion_History.cs`

📝 **小提示：** 可以到 [GitHub Playground](https://github.com/marketplace/models/azure-openai/gpt-4-1-nano/playground) 測試模型設定

#### System Prompt 範例

```
1. 你是一個健身減重教練。

2. 你是一個健身減重教練，使用者問健身以外的問題不要回答

3. 你是一個健身減重教練，
   - 永遠使用繁體中文回覆
   - 不要回答健身以外的問題
   - 不要回答任何程式相關問題
   - 專注於幫使用建立運動相關的建議(如：建議的運動種類/組數等等)
```

#### 測試用 QA 範例

```
我想買房
我要寫健身app,給我範例程式，建立一個簡易的訓練計劃靜態html
給我prd文件
我是Woody
給我健身app,prd
I want edit prd
I want change release date to 2025/09/05
我剛剛問了什麼

我想開健身房，你可以推薦我地點嗎?
那我住在大安區 給我最推薦的地段

Can you use English to answer my question?
```

#### 2.3 使用 OpenAI Model 執行 Function Calling
實作 OpenAI 的 Function Calling 功能。

#### 2.4 使用 Gemini Model 執行 Function Calling
使用 Google Gemini 模型進行 Function Calling。

### 3. 使用 Qdrant 向量資料庫

#### 3.0 Docker 建置環境 & 確認連線

使用 Docker 建立 [Qdrant](https://github.com/qdrant/qdrant) 向量資料庫環境：

```bash
podman run -d -p 6333:6333 qdrant/qdrant
```

#### 3.1 使用 Semantic Kernel 與 Qdrant 整合
整合 Semantic Kernel 與 Qdrant 向量資料庫。

#### 3.2 寫入 PDF 資料，且確認可以搜尋
實作 PDF 文件的向量化與搜尋功能。

#### 3.3 使用 md 文字檔案
處理 Markdown 格式的文檔向量化。

#### 3.4 取得多篇文章，輸入詢問確認可以從 Qdrant 中取出相關文章
實作多文檔檢索功能。

**參考資料：** [相關教學影片](https://www.youtube.com/watch?v=ujgf9g4ajus)

---

## 🔗 相關連結

- [Microsoft Semantic Kernel 官方文件](https://learn.microsoft.com/en-us/semantic-kernel/)
- [OpenAI API 文件](https://platform.openai.com/docs)
- [Qdrant 向量資料庫](https://qdrant.tech/)
- [GitHub Models Playground](https://github.com/marketplace/models)