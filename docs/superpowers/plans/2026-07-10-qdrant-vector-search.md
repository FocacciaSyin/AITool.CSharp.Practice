# Qdrant Vector Search Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** 將未完成的 Qdrant 草稿遷移至目前 VectorData API，並新增可從互動選單執行的最小向量搜尋範例。

**Architecture:** `EmbeddingService` 僅負責 Gemini embedding；`QdrantService` 僅封裝 `QdrantCollection<Guid, QdrantDocument>`；`VectorMemoryService` 組合兩者完成文件寫入與文字搜尋。Sample 建立服務、upsert 固定文件、讀取查詢並顯示結果。

**Tech Stack:** .NET 8、Semantic Kernel 1.65、Google connector 1.64.0-alpha、Qdrant connector 1.66.0-preview、Microsoft.Extensions.VectorData 9.7、Qdrant.Client 1.15.1、Podman。

## Global Constraints

- Qdrant gRPC endpoint 預設為 `http://localhost:6334`，允許設定覆寫。
- Embedding model 預設為 `gemini-embedding-001` 且允許設定覆寫；輸出維度固定為 768。
- API key 只由 `Gemini:ApiKey` User Secret 提供，不寫入 tracked 檔案。
- 不自動刪除或重建既有 collection。
- 不新增文件解析、聊天記憶、混合搜尋、重試框架或通用 repository。
- 識別字使用英文；新增註解使用繁體中文。
- 未經使用者明確要求，不建立 git commit。

---

### Task 1: 遷移設定、模型與核心服務

**Files:**
- Modify: `AITool.CSharp.Practice/AITool.CSharp.Practice.csproj`
- Modify: `AITool.CSharp.Practice/Models/Settings/GeminiSettings.cs`
- Modify: `AITool.CSharp.Practice/Models/Settings/QdrantSettings.cs`
- Create: `AITool.CSharp.Practice/Models/VectorStore/QdrantDocument.cs`
- Replace: `AITool.CSharp.Practice/Models/Services/EmbeddingService.cs`
- Replace: `AITool.CSharp.Practice/Models/Services/QdrantService.cs`
- Replace: `AITool.CSharp.Practice/Models/Services/VectorMemoryService.cs`

**Interfaces:**
- Produces: `EmbeddingService.GenerateEmbeddingAsync(string, CancellationToken) -> Task<ReadOnlyMemory<float>>`
- Produces: `QdrantService.EnsureCollectionExistsAsync(CancellationToken) -> Task`
- Produces: `QdrantService.UpsertAsync(IEnumerable<QdrantDocument>, CancellationToken) -> Task`
- Produces: `QdrantService.SearchAsync(ReadOnlyMemory<float>, CancellationToken) -> IAsyncEnumerable<VectorSearchResult<QdrantDocument>>`
- Produces: `VectorMemoryService.SaveDocumentsAsync(IEnumerable<(Guid Id, string Content, string Description)>, CancellationToken) -> Task`
- Produces: `VectorMemoryService.SearchAsync(string, CancellationToken) -> Task<IReadOnlyList<VectorSearchResult<QdrantDocument>>>`

- [ ] **Step 1: 建立 RED 編譯案例**

從 csproj 移除以下暫時排除，讓草稿重新參與編譯：

```xml
<Compile Remove="Models\Services\EmbeddingService.cs" />
<Compile Remove="Models\Services\QdrantService.cs" />
<Compile Remove="Models\Services\VectorMemoryService.cs" />
```

- [ ] **Step 2: 執行 build，確認因舊 API 失敗**

Run: `dotnet build AITool.CSharp.Practice/AITool.CSharp.Practice.csproj --no-restore`

Expected: FAIL，錯誤包含 `WithQdrantMemoryStore`、`GoogleAIGeniniEmbeddingGenerationService` 或舊 Qdrant client 呼叫。

- [ ] **Step 3: 更新設定模型**

`GeminiSettings` 新增：

```csharp
public string EmbeddingModelId { get; set; } = "gemini-embedding-001";
```

`QdrantSettings` 保留既有欄位，但將預設值調整為：

```csharp
public string Endpoint { get; set; } = "http://localhost:6334";
public string CollectionName { get; set; } = "semantic_kernel_demo";
public int VectorSize { get; set; } = 768;
public int SearchLimit { get; set; } = 3;
public int TimeoutSeconds { get; set; } = 30;
```

- [ ] **Step 4: 建立 VectorData record model**

新增 `QdrantDocument.cs`：

```csharp
using Microsoft.Extensions.VectorData;

namespace AITool.CSharp.Practice.Models.VectorStore;

public sealed class QdrantDocument
{
    public const int VectorDimensions = 768;

    [VectorStoreKey]
    public Guid Id { get; set; }

    [VectorStoreData]
    public string Content { get; set; } = string.Empty;

    [VectorStoreData]
    public string Description { get; set; } = string.Empty;

    [VectorStoreVector(VectorDimensions, DistanceFunction = DistanceFunction.CosineSimilarity)]
    public ReadOnlyMemory<float> Embedding { get; set; }
}
```

- [ ] **Step 5: 以目前 Google embedding generator 重寫 EmbeddingService**

實作以下核心內容，建構時檢查 API key、embedding model 與 dimensions；產生後再次確認向量維度：

```csharp
using AITool.CSharp.Practice.Models.Settings;
using Microsoft.Extensions.AI;
using Microsoft.SemanticKernel.Connectors.Google;

namespace AITool.CSharp.Practice.Models.Services;

public sealed class EmbeddingService : IDisposable
{
    private readonly GoogleAIEmbeddingGenerator _generator;
    private readonly int _dimensions;

    public EmbeddingService(GeminiSettings settings, int dimensions)
    {
        if (string.IsNullOrWhiteSpace(settings.ApiKey) || settings.ApiKey == "透過 Secret Manager 設定")
        {
            throw new ArgumentException("Gemini API key 未透過 User Secrets 設定。");
        }

        ArgumentException.ThrowIfNullOrWhiteSpace(settings.EmbeddingModelId);

        if (dimensions != QdrantDocument.VectorDimensions)
        {
            throw new ArgumentException($"Embedding 維度必須為 {QdrantDocument.VectorDimensions}。");
        }

        _dimensions = dimensions;
        _generator = new GoogleAIEmbeddingGenerator(
            settings.EmbeddingModelId,
            settings.ApiKey,
            GoogleAIVersion.V1_Beta,
            httpClient: null,
            loggerFactory: null,
            dimensions: dimensions);
    }

    public async Task<ReadOnlyMemory<float>> GenerateEmbeddingAsync(
        string text,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(text);
        var vector = await _generator.GenerateVectorAsync(text, cancellationToken: cancellationToken);

        if (vector.Length != _dimensions)
        {
            throw new InvalidOperationException($"Embedding 維度應為 {_dimensions}，實際為 {vector.Length}。");
        }

        return vector;
    }

    public void Dispose() => _generator.Dispose();
}
```

- [ ] **Step 6: 以 QdrantCollection 重寫 QdrantService**

保留 endpoint、collection name、search limit 與 timeout 驗證，實作：

```csharp
using AITool.CSharp.Practice.Models.Settings;
using AITool.CSharp.Practice.Models.VectorStore;
using Microsoft.Extensions.VectorData;
using Microsoft.SemanticKernel.Connectors.Qdrant;
using Qdrant.Client;

namespace AITool.CSharp.Practice.Models.Services;

public sealed class QdrantService : IDisposable
{
    private readonly QdrantClient _client;
    private readonly QdrantCollection<Guid, QdrantDocument> _collection;
    private readonly int _searchLimit;

    public QdrantService(QdrantSettings settings)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(settings.Endpoint);
        ArgumentException.ThrowIfNullOrWhiteSpace(settings.CollectionName);

        if (settings.VectorSize != QdrantDocument.VectorDimensions)
        {
            throw new ArgumentException($"Qdrant VectorSize 必須為 {QdrantDocument.VectorDimensions}。");
        }

        if (settings.SearchLimit <= 0 || settings.TimeoutSeconds <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(settings));
        }

        _searchLimit = settings.SearchLimit;
        _client = new QdrantClient(
            new Uri(settings.Endpoint),
            settings.ApiKey ?? string.Empty,
            TimeSpan.FromSeconds(settings.TimeoutSeconds),
            loggerFactory: null);
        _collection = new QdrantCollection<Guid, QdrantDocument>(
            _client,
            settings.CollectionName,
            ownsClient: false,
            options: null);
    }

    public async Task EnsureCollectionExistsAsync(CancellationToken cancellationToken = default)
    {
        await _client.HealthAsync(cancellationToken);
        await _collection.EnsureCollectionExistsAsync(cancellationToken);
    }

    public Task UpsertAsync(
        IEnumerable<QdrantDocument> documents,
        CancellationToken cancellationToken = default) =>
        _collection.UpsertAsync(documents, cancellationToken);

    public IAsyncEnumerable<VectorSearchResult<QdrantDocument>> SearchAsync(
        ReadOnlyMemory<float> vector,
        CancellationToken cancellationToken = default) =>
        _collection.SearchAsync(vector, _searchLimit, options: null, cancellationToken);

    public void Dispose()
    {
        _collection.Dispose();
        _client.Dispose();
    }
}
```

- [ ] **Step 7: 重寫 VectorMemoryService orchestration**

```csharp
using AITool.CSharp.Practice.Models.VectorStore;
using Microsoft.Extensions.VectorData;

namespace AITool.CSharp.Practice.Models.Services;

public sealed class VectorMemoryService(
    EmbeddingService embeddingService,
    QdrantService qdrantService)
{
    public Task InitializeAsync(CancellationToken cancellationToken = default) =>
        qdrantService.EnsureCollectionExistsAsync(cancellationToken);

    public async Task SaveDocumentsAsync(
        IEnumerable<(Guid Id, string Content, string Description)> documents,
        CancellationToken cancellationToken = default)
    {
        var records = new List<QdrantDocument>();
        foreach (var document in documents)
        {
            records.Add(new QdrantDocument
            {
                Id = document.Id,
                Content = document.Content,
                Description = document.Description,
                Embedding = await embeddingService.GenerateEmbeddingAsync(document.Content, cancellationToken)
            });
        }

        await qdrantService.UpsertAsync(records, cancellationToken);
    }

    public async Task<IReadOnlyList<VectorSearchResult<QdrantDocument>>> SearchAsync(
        string query,
        CancellationToken cancellationToken = default)
    {
        var vector = await embeddingService.GenerateEmbeddingAsync(query, cancellationToken);
        var results = new List<VectorSearchResult<QdrantDocument>>();

        await foreach (var result in qdrantService.SearchAsync(vector, cancellationToken))
        {
            results.Add(result);
        }

        return results;
    }
}
```

- [ ] **Step 8: 執行 build，修正僅限實際簽章差異**

Run: `dotnet build AITool.CSharp.Practice/AITool.CSharp.Practice.csproj --no-restore`

Expected: PASS，0 errors。若 preview 套件實際簽章與 XML 文件有差異，只做必要型別或參數調整，不加入額外抽象。

---

### Task 2: 加入最小互動 sample 與設定

**Files:**
- Create: `AITool.CSharp.Practice/Samples/6_Qdrant/Sample_6_0_Qdrant_VectorSearch.cs`
- Modify: `AITool.CSharp.Practice/appsettings.json`
- Modify: `AITool.CSharp.Practice/Program.cs`

**Interfaces:**
- Consumes: Task 1 的 `EmbeddingService`、`QdrantService`、`VectorMemoryService`。
- Produces: `Sample_6_0_Qdrant_VectorSearch.RunAsync(QdrantSettings, GeminiSettings) -> Task`。

- [ ] **Step 1: 建立 sample 並使用固定 ID 保持可重複執行**

```csharp
using AITool.CSharp.Practice.Models.Services;
using AITool.CSharp.Practice.Models.Settings;

namespace AITool.CSharp.Practice.Samples._6_Qdrant;

public static class Sample_6_0_Qdrant_VectorSearch
{
    public static async Task RunAsync(QdrantSettings qdrantSettings, GeminiSettings geminiSettings)
    {
        using var embeddingService = new EmbeddingService(geminiSettings, qdrantSettings.VectorSize);
        using var qdrantService = new QdrantService(qdrantSettings);
        var memoryService = new VectorMemoryService(embeddingService, qdrantService);

        try
        {
            await memoryService.InitializeAsync();
            await memoryService.SaveDocumentsAsync(
            [
                (Guid.Parse("11111111-1111-1111-1111-111111111111"), "Semantic Kernel 是微軟提供的 AI 應用程式開發 SDK。", "Semantic Kernel"),
                (Guid.Parse("22222222-2222-2222-2222-222222222222"), "Qdrant 是支援向量相似度搜尋的向量資料庫。", "Qdrant"),
                (Guid.Parse("33333333-3333-3333-3333-333333333333"), "Podman 可以在本機執行 OCI 容器。", "Podman")
            ]);

            Console.WriteLine("輸入向量搜尋問題（直接 Enter 使用預設問題）：");
            var query = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(query))
            {
                query = "哪一個工具可以搜尋語意相近的資料？";
            }

            var results = await memoryService.SearchAsync(query);
            Console.WriteLine($"\n搜尋：{query}");

            foreach (var result in results)
            {
                Console.WriteLine($"- {result.Record.Description} | 分數：{result.Score:F4}");
                Console.WriteLine($"  {result.Record.Content}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Qdrant 範例執行失敗：{ex.Message}");
            Console.WriteLine($"請確認 Qdrant 已在 {qdrantSettings.Endpoint} 啟動，且 Gemini API key 已設定。");
        }
    }
}
```

- [ ] **Step 2: 補上 tracked 非機密設定**

在 `Gemini` section 新增：

```json
"EmbeddingModelId": "gemini-embedding-001"
```

新增：

```json
"Qdrant": {
  "Endpoint": "http://localhost:6334",
  "ApiKey": "",
  "CollectionName": "semantic_kernel_demo",
  "VectorSize": 768,
  "SearchLimit": 3,
  "TimeoutSeconds": 30,
  "HealthCheckOnStartup": true
}
```

- [ ] **Step 3: 註冊 settings 與 sample**

`Program.cs` 加入 namespace：

```csharp
using AITool.CSharp.Practice.Samples._6_Qdrant;
```

設定讀取加入：

```csharp
var qdrantSettings = configuration.GetSection("Qdrant").Get<QdrantSettings>()!;
```

`allSamples` 尾端加入：

```csharp
("6.0 [Qdrant] Vector Search", async () => await Sample_6_0_Qdrant_VectorSearch.RunAsync(qdrantSettings, geminiSettings))
```

- [ ] **Step 4: 執行 build**

Run: `dotnet build AITool.CSharp.Practice/AITool.CSharp.Practice.csproj --no-restore`

Expected: PASS，0 errors。

---

### Task 3: 本機端到端驗證與文件同步

**Files:**
- Modify only if needed: `README.md`
- Modify: `AGENTS.md` only if execution guidance changed

**Interfaces:**
- Consumes: Task 2 的互動 sample。
- Produces: 可重複執行的本機 Qdrant 向量搜尋流程。

- [ ] **Step 1: 確認或啟動 Qdrant 容器**

Run: `podman ps --filter name=qdrant`

若沒有執行中的容器：

Run: `podman run -d --name qdrant -p 6333:6333 -p 6334:6334 -v qdrant_storage:/qdrant/storage qdrant/qdrant:latest`

Expected: container 狀態為 running，REST dashboard 位於 `http://localhost:6333/dashboard`，gRPC 位於 `localhost:6334`。

- [ ] **Step 2: 確認 User Secret 存在**

Run: `dotnet user-secrets list --project AITool.CSharp.Practice/AITool.CSharp.Practice.csproj`

Expected: 列出 `Gemini:ApiKey`；不得把其值複製到任何 tracked 檔案或輸出摘要。

- [ ] **Step 3: 從正確工作目錄執行 sample**

在 `AITool.CSharp.Practice/` 執行 `dotnet run`，輸入新增 sample 的選單編號，再輸入查詢文字。

Expected: 顯示至少一筆結果，且查詢「哪一個工具可以搜尋語意相近的資料？」時 Qdrant 文件應位於前列。

- [ ] **Step 4: 再執行一次確認 idempotency**

重複 Task 3 Step 3。

Expected: collection 已存在時不刪除重建，固定 ID 被更新且搜尋仍成功。

- [ ] **Step 5: 最終驗證**

Run: `dotnet restore AITool.CSharp.Practice/AITool.CSharp.Practice.csproj`

Run: `dotnet build AITool.CSharp.Practice/AITool.CSharp.Practice.csproj --no-restore`

Expected: 0 errors；只回報實際存在的 warnings。檢查 `git diff`，不得包含 API key、`.env`、User Secrets 或無關格式化。
