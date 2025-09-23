using System.ClientModel;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using OpenAI;
using AITool.CSharp.Practice.Models.Settings;
using AITool.CSharp.Practice.Models.Reducer;

namespace AITool.CSharp.Practice.Samples;

/// <summary>
/// 2.2.1.2 Reducer 總結 (Summarization) 策略示範
/// 說明：
/// 僅示範『總結 (Summarization)』策略：
/// 使用 Microsoft.SemanticKernel.ChatCompletion 原生 ChatHistorySummarizationReducer 策略
/// </summary>
public class Sample_2_2_1_2_SemanticKernelWithGitHub_ChatCompletion_Reducer_Summarization(IOptions<GitHubSettings> githubSettings)
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

        // 4. 建立『總結』 ，targetCount 表示要保留的最近 N 輪 ，
        // thresholdCount 表示超過多少則訊息才會進行總結
        
        var reducer_2_10 = new ChatHistorySummarizationReducer(chatCompletionService, targetCount: 2, thresholdCount: 10); // 超過總訊息數會回傳 null
        
        // targetCount = 3 > System(1) + Assistant(1) + User(2) 
        var reducer_3_5 = new ChatHistorySummarizationReducer(chatCompletionService, targetCount: 3, thresholdCount: 5);
        // var reducer_10 = new ChatHistorySummarizationReducer(chatCompletionService, targetCount: 3, thresholdCount: 9);

        int totalTokenCount = 0;

        // 5. 建立原始對話歷史
        var chatHistory = new ChatHistory();
        chatHistory.AddSystemMessage("你是一個氣象專家。"); //壓縮後只會保留第一個
        chatHistory.AddSystemMessage("不能回答程式相關的問題，不知道就直接說不知道，總結使用繁體中文");

        chatHistory.AddUserMessage("哈囉，我是 秋雨新.");
        chatHistory.AddUserMessage("今天天氣怎樣？");
        chatHistory.AddUserMessage("我需要帶雨傘嗎？");
        chatHistory.AddUserMessage("幫我推薦三家附近的咖啡廳。");
        chatHistory.AddUserMessage("哪一家有 Wi-Fi？");
        chatHistory.AddUserMessage("我想買新北市的房子，請問房價怎麼樣？");
        chatHistory.AddUserMessage("高雄的交通方便嗎");
        chatHistory.AddAssistantMessage("你好！請問有什麼我可以幫助你的嗎？");
        

        // 5. 進行總結
        var reducedMessages_2_10 = await reducer_2_10.ReduceAsync(chatHistory, ct);
        var reducedMessages_3_5 = await reducer_3_5.ReduceAsync(chatHistory, ct);

        if (reducedMessages_2_10 is not null)
        {
            foreach (var msg in reducedMessages_2_10)
            {
                Console.WriteLine($"[{msg.Role}] {msg.Content}");
            }
        }

        foreach (var msg in reducedMessages_3_5)
        {
            Console.WriteLine($"[{msg.Role}] {msg.Content}");
        }
    }
}
