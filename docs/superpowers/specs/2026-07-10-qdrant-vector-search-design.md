# Qdrant 向量搜尋範例設計

## 目標

將目前未完成的 Qdrant 草稿遷移至 `Microsoft.SemanticKernel.Connectors.Qdrant 1.66.0-preview` 使用的 VectorData API，並提供一個能從互動選單執行的最小端到端範例。

## 範圍

- 使用本機 Qdrant 與 Gemini embedding 建立、寫入及查詢單一 collection。
- 在 `Program.cs` 的 `allSamples` 註冊一個 Qdrant sample。
- 保留現有 samples 行為，不導入文件解析、聊天記憶、混合搜尋或管理介面。
- 移除 csproj 對三個草稿 service 的暫時編譯排除；不沿用舊 `MemoryBuilder`/`ISemanticTextMemory` API。

## 元件

- `QdrantSettings`：保存可覆寫的 endpoint、API key、collection name、vector size；Gemini settings 增加可覆寫的 embedding model 設定。預設使用本機 Qdrant 與 `gemini-embedding-001`，向量維度固定為 768。
- 文件向量模型：包含 `Guid` key、文字內容、描述與 embedding，使用 VectorData attributes 描述欄位。
- Qdrant vector store service：封裝 collection 建立、upsert 與 vector search；直接使用新版 `QdrantCollection`/VectorData abstraction。
- Embedding service：使用目前 Google connector 提供的 embedding generator，不使用已淘汰的 `ITextEmbeddingGenerationService`。
- Sample：寫入少量固定文件，讀取查詢文字，產生 query embedding，輸出命中文件與相似度。

## 執行流程

1. 從 `appsettings.json` 與 User Secrets 載入 Qdrant、Gemini 設定。
2. 建立 embedding generator 與 Qdrant collection。
3. 確認 collection 存在，不存在時建立。
4. 將固定示範文件轉成 embeddings 並 upsert。
5. 讀取使用者查詢、產生 embedding、執行 top-k 搜尋。
6. 輸出內容、描述與相似度，回到既有互動選單。

## 錯誤處理

- 缺少 Gemini API key 或 embedding model 時，在呼叫外部服務前回報設定錯誤。
- Qdrant 無法連線時，顯示 endpoint 與 Podman 啟動提示，不吞掉例外後假裝無結果。
- embedding 維度與 collection 設定不一致時，明確回報並停止 sample，不自動刪除既有 collection。
- sample 不記錄 API key、完整 request headers 或其他機密資料。

## 驗證

- 先移除 csproj 的暫時編譯排除並執行 build，確認草稿 API 錯誤可重現。
- 執行 `dotnet restore` 與 `dotnet build --no-restore`，要求 0 errors。
- 使用 Podman 啟動本機 Qdrant，從 `AITool.CSharp.Practice/` 執行應用程式並選取新增 sample。
- 驗證 collection 建立、固定文件 upsert、查詢至少回傳一筆結果；再執行一次確認流程可重複執行。

## 非目標

- 不處理既有 OpenAI 套件版本警告或 OpenTelemetry 弱點警告。
- 不建立通用 repository、RAG pipeline、長短期記憶分層或 production retry policy。
- 不為尚未使用的進階 Qdrant 功能預先設計抽象層。
