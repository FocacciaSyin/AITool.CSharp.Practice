using System.ClientModel;
using System.Text;
using AITool.CSharp.Practice.Models.Settings;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using OpenAI;

namespace AITool.CSharp.Practice.Samples._2_SemanticKernel;

/// <summary>
/// 2.2 使用 Semantic Kernel + GitHub Model 進行『聊天 + 紀錄對話歷史』的簡易範例。
/// 說明：
/// 1. 使用 GitHub Models （透過 OpenAI 相容端點）進行 Chat Completion。
/// 2. 使用 ChatHistory 保存所有對話輪次（System / User / Assistant）。
/// 3. 尚未做減量（Reducer）處理 — 純紀錄全部訊息的基本版。
/// 4. 可搭配 2.2.1 系列進一步示範截斷 / 摘要。
/// </summary>
/// <remarks>
/// GitHub Models 可讓你在不直接使用 OpenAI API Key 的情況下測試模型（需設定對應 Endpoint 與 Token）。
/// </remarks>
public class Sample_2_2_SemanticKernelWithGitHub_ChatCompletion_History(IOptions<GitHubSettings> githubSettings)
{
    private readonly GitHubSettings _gitHubSettings = githubSettings.Value;

    /// <summary>
    /// 執行主流程：讀取使用者輸入 → 呼叫模型 → 顯示並寫入歷史。
    /// 空行（Enter）離開迴圈。
    /// </summary>
    public async Task ExecuteAsync()
    {
        // 建立 OpenAI 相容 Client（使用 GitHub Models Token）
        var client = new OpenAIClient(
            credential: new ApiKeyCredential(_gitHubSettings.ApiKey),
            options: new OpenAIClientOptions { Endpoint = new Uri(_gitHubSettings.EndPoint) });

        // 建立 Kernel 並註冊 Chat Completion 服務
        var kernel = Kernel.CreateBuilder()
            .AddOpenAIChatCompletion(_gitHubSettings.ModelId, client)
            .Build();
        
        // 從 DI / Kernel 解析出聊天服務
        var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();
        
        // 建立聊天歷史（保存所有對話，用於提供上下文）
        var history = new ChatHistory();
        history.AddSystemMessage("""
                                 你是一個健身減重教練，使用者問健身以外的問題不要回答。
                                 """); // 系統訊息：定義角色/行為

        while (true)
        {
            Console.Write("User > ");
            var userInput = Console.ReadLine();
            
            if (string.IsNullOrWhiteSpace(userInput))
            {
                Console.WriteLine("結束對話。\n");
                break; // 直接離開迴圈
            }
            
            // 將使用者輸入加入歷史
            history.AddUserMessage(userInput);
            
            // 以 StringBuilder 收集串流回覆
            var sb = new StringBuilder();
            var responses = chatCompletionService.GetStreamingChatMessageContentsAsync(history);
            
            Console.Write("AI   > ");
            await foreach (var item in responses)
            {
                // item.Content 可能為 null（模型產生 token 中途），需判斷或直接輸出
                sb.Append(item.Content);
                Console.Write(item.Content);
            }
            Console.WriteLine();
        
            // 將模型回覆寫回歷史，成為下一輪上下文一部分
            history.AddAssistantMessage(sb.ToString());
        }
    }
}
