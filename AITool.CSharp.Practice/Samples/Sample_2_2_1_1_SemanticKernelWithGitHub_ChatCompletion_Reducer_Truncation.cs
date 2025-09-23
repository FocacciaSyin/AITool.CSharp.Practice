using System.ClientModel;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using OpenAI;
using AITool.CSharp.Practice.Models.Settings;

namespace AITool.CSharp.Practice.Samples;

/// <summary>
/// 2.2.1.1 Reducer 截斷 (Truncation) 策略示範
/// 說明：
/// 僅示範『截斷 (Truncation)』策略：
/// 使用 Microsoft.SemanticKernel.ChatCompletion 原生 ChatHistoryTruncationReducer 策略
/// </summary>
public class Sample_2_2_1_1_SemanticKernelWithGitHub_ChatCompletion_Reducer_Truncation(IOptions<GitHubSettings> githubSettings)
{
    // GitHub OpenAI 相容模型設定（由 DI 注入）
    private readonly GitHubSettings _gitHubSettings = githubSettings.Value;

    /// <summary>
    /// 執行截斷 (Truncation) Reducer 示範流程
    /// </summary>
    /// <param name="ct">取消作業用的 CancellationToken。</param>
    public async Task ExecuteAsync(CancellationToken ct = default)
    {
        // 1. 建立 GitHub Models 相容的 OpenAIClient（使用 GitHub Token + Endpoint）
        //    GitHub Models 對外提供 OpenAI 相容協議，因此可直接使用 OpenAIClient。
        var client = new OpenAIClient(
            credential: new ApiKeyCredential(_gitHubSettings.ApiKey),
            options: new OpenAIClientOptions { Endpoint = new Uri(_gitHubSettings.EndPoint) }
        );

        // 2. 建立 Kernel 並註冊 ChatCompletion 服務
        var kernel = Kernel.CreateBuilder()
            .AddOpenAIChatCompletion(_gitHubSettings.ModelId, client)
            .Build();

        // 3. 取得 ChatCompletion 服務實例
        var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

        // 4. 建立『截斷』Reducer，targetCount 表示要保留的最近 N 輪 , System 只會保留一個
        var reducer = new ChatHistoryTruncationReducer(targetCount: 5);

        int totalTokenCount = 0;

        // 5. 建立原始對話歷史
        var chatHistory = new ChatHistory();
        chatHistory.AddSystemMessage("你是一個簡短回覆的助理。(不會被截斷)");
        chatHistory.AddSystemMessage("這是第二段SystemPrompt(注意：會被截斷)");

        // 模擬多輪對話
        for (int i = 1; i <= 10; i++)
        {
            chatHistory.AddUserMessage($"使用者問題 #{i}: 請示範第 {i} 次問題");
            chatHistory.AddAssistantMessage($"助理回覆 #{i}: 這是回覆 {i}");
        }


        // 6. 進行截斷
        var reducedMessages = await reducer.ReduceAsync(chatHistory, ct);
        if (reducedMessages is not null)
        {
            // 以截斷後的訊息重新建立 ChatHistory，後續用於送出模型請求
            chatHistory = new ChatHistory(reducedMessages);
        }

        Console.WriteLine("=== 截斷後的對話歷史 ===");
        foreach (var reducedChatHistory in chatHistory)
        {
            Console.WriteLine($"{reducedChatHistory.Role}: {reducedChatHistory.Content}");
        }

        //呼叫 ChatCompletion 服務取得結果
        var response = await chatCompletionService.GetChatMessageContentsAsync(chatHistory);
        Console.WriteLine(response[0].Content);
    }
}
