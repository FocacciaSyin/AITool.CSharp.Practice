using System.ClientModel;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using OpenAI;
using AITool.CSharp.Practice.Models.Settings;

namespace AITool.CSharp.Practice.Samples;

/// <summary>
/// 2.2.1.1 Reducer 範例（Truncation 單一示例）
/// 說明：
/// 僅示範『截斷 (Truncation)』策略：保留最近 N 輪 (User + Assistant) 對話，System 訊息全部保留。
/// 不包含摘要邏輯（若需摘要請參考 Sample_2_2_1_2_*_Reducer_Summarization）。
/// </summary>
public class Sample_2_2_1_1_SemanticKernelWithGitHub_ChatCompletion_Reducer_Truncation(IOptions<GitHubSettings> githubSettings)
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

        Console.WriteLine("=== 截斷範例：僅保留最近 N 輪對話 ===\n");
        
        var truncHistory = new ChatHistory();
        truncHistory.AddSystemMessage("你是一個簡短回覆的助理。此範例僅示範『截斷』策略，不做摘要。");

        for (int i = 1; i <= 6; i++)
        {
            truncHistory.AddUserMessage($"使用者問題 #{i}: 請示範第 {i} 次問題");
            truncHistory.AddAssistantMessage($"助理回覆 #{i}: 這是回覆 {i}");
        }

        Console.WriteLine("[截斷前] 完整歷史：");
        PrintHistory(truncHistory);

        int roundsToKeep = 2; // 要保留的最近輪數
        TruncateHistory(truncHistory, roundsToKeep);

        Console.WriteLine($"\n[截斷後] 僅保留最近 {roundsToKeep} 輪：");
        PrintHistory(truncHistory);

        Console.WriteLine("\n=== 範例結束（Truncation）===");
    }

    /// <summary>
    /// 截斷策略：僅保留最近 N 輪 (User+Assistant) 對話；System 訊息全部保留。
    /// 為簡化：假設對話歷史是按時間順序 append。
    /// </summary>
    private static void TruncateHistory(ChatHistory history, int roundsToKeep)
    {
        if (roundsToKeep <= 0) { history.Clear(); return; }
        // 取出 System 與非 System 訊息（避免把角色定義刪掉）
        var systemMessages = history.Where(m => m.Role == AuthorRole.System).ToList();
        var nonSystem = history.Where(m => m.Role != AuthorRole.System).ToList();

        // 每輪 2 則（User + Assistant），計算要保留的總數
        int keepCount = Math.Min(nonSystem.Count, roundsToKeep * 2);
        var toKeep = nonSystem.Skip(Math.Max(0, nonSystem.Count - keepCount)).ToList();

        history.Clear();
        // 先放回 System，再放最近對話
        foreach (var s in systemMessages) history.Add(s);
        foreach (var m in toKeep) history.Add(m);
    }

    /// <summary>
    /// 輸出目前 ChatHistory 內容（依順序）。
    /// </summary>
    private static void PrintHistory(ChatHistory history)
    {
        foreach (var msg in history)
        {
            Console.WriteLine($"[{msg.Role}] {msg.Content}");
        }
    }
}
