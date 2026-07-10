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
