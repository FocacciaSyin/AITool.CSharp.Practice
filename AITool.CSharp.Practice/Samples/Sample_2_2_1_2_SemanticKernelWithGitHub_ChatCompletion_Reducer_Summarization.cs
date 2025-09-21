using System.ClientModel;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using OpenAI;
using AITool.CSharp.Practice.Models.Settings;
using AITool.CSharp.Practice.Models.Reducer;

namespace AITool.CSharp.Practice.Samples;

/// <summary>
/// 2.2.1.2 Reducer 範例（Summarization 單一示例）
/// 說明：
/// 僅示範『摘要 (Summarization)』策略：當歷史過長時，將較舊訊息彙整為 System 摘要，以節省 Token 又保留語意。
/// 搭配自訂的 RecursiveSummarizingChatReducer。
/// 不包含截斷邏輯（若需截斷請參考 Sample_2_2_1_1_*_Reducer_Truncation）。
/// </summary>
public class Sample_2_2_1_2_SemanticKernelWithGitHub_ChatCompletion_Reducer_Summarization(IOptions<GitHubSettings> githubSettings)
{
    private readonly GitHubSettings _gitHubSettings = githubSettings.Value;

    public async Task ExecuteAsync(CancellationToken ct = default)
    {
        // 建立 GitHub Models 相容 OpenAIClient（使用 GitHub Token + Endpoint）
        var client = new OpenAIClient(
            credential: new ApiKeyCredential(_gitHubSettings.ApiKey),
            options: new OpenAIClientOptions { Endpoint = new Uri(_gitHubSettings.EndPoint) }
        );

        // 建立 Kernel 並註冊 ChatCompletion 服務
        var kernel = Kernel.CreateBuilder()
            .AddOpenAIChatCompletion(_gitHubSettings.ModelId, client)
            .Build();

        var chatService = kernel.GetRequiredService<IChatCompletionService>();

        Console.WriteLine("=== 摘要範例：將舊對話壓縮為摘要 ===\n");

        var history = new ChatHistory();
        history.AddSystemMessage("你是一個健身教練，只能回答健身與減重相關內容。");

        // 模擬較長歷史，便於觸發摘要
        for (int i = 1; i <= 12; i++)
        {
            history.AddUserMessage($"使用者：這是第 {i} 次提問，請給我一點健身/飲食建議 (編號 {i})");
            history.AddAssistantMessage($"助理：這是第 {i} 次回覆，提供一些訓練或營養面向的說明與細節，可能稍微冗長以增加內容長度。");
        }

        Console.WriteLine("[摘要前] 歷史訊息（部分可能很長）：");
        PrintHistory(history);

        // 建立摘要 Reducer（目前採用字元長度近似，可後續換成 Token 版）
        var reducer = new RecursiveSummarizingChatReducer(maxTokens: 500, kernel: kernel, chatService: chatService);

        Console.WriteLine("\n執行摘要 Reducer...\n");
        try
        {
            var reduced = await reducer.ReduceAsync(history, ct);
            Console.WriteLine("[摘要後] 歷史訊息（前方可能插入摘要 System 訊息）：");
            PrintHistory(reduced);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Reducer 呼叫失敗（可能是 API Key/網路問題）：{ex.Message}");
            Console.WriteLine("此情況下僅示範流程，實際摘要需模型請求成功。");
        }

        Console.WriteLine("\n=== 範例結束（Summarization）===");
    }

    // 移除截斷相關方法，僅保留輸出工具
    private static void PrintHistory(ChatHistory history)
    {
        foreach (var msg in history)
        {
            Console.WriteLine($"[{msg.Role}] {msg.Content}");
        }
    }
}
