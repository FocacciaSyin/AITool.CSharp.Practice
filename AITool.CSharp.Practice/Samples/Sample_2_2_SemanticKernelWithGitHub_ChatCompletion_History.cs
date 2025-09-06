using System.ClientModel;
using System.Text;
using AITool.CSharp.Practice.Models;
using AITool.CSharp.Practice.Models.Settings;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using OpenAI;

namespace AITool.CSharp.Practice.Samples;

/// <summary>
/// 2.2 使用 SemanticKernel + GitHub Model 聊天紀錄
/// </summary>
/// <remarks>
/// 可以使用 GitHub Model 內的模型，這樣就可以不用花錢買 OpenAI Token
/// </remarks>
/// <param name="githubSettings"></param>
public class Sample_2_2_SemanticKernelWithGitHub_ChatCompletion_History(IOptions<GitHubSettings> githubSettings)
{
    private readonly GitHubSettings _gitHubSettings = githubSettings.Value;

    public async Task ExecuteAsync()
    {
        var client = new OpenAIClient(
            credential: new ApiKeyCredential(_gitHubSettings.ApiKey),
            options: new OpenAIClientOptions
            {
                Endpoint = new Uri(_gitHubSettings.EndPoint)
            });

        // 1. Kernel Build 
        var kernel = Kernel.CreateBuilder()
            .AddOpenAIChatCompletion(_gitHubSettings.ModelId, client)
            .Build();
        
        // 2. 建立聊天紀錄
        var chat = kernel.GetRequiredService<IChatCompletionService>();
        
        // 會記錄所有聊天內容
        var history = new ChatHistory();
        history.AddSystemMessage("""
                                 你是一個健身減重教練，使用者問健身以外的問題不要回答。
                                 """); // 系統訊息，設定 AI 角色

        while (true)
        {
            Console.WriteLine("User:");
            var userInput = Console.ReadLine();
            
            if (string.IsNullOrEmpty(userInput))
            {
                break;
            }
            
            history.AddUserMessage(userInput); //使用者輸入
            
            var sb = new StringBuilder();
            var result = chat.GetStreamingChatMessageContentsAsync(history);
            
            Console.Write("AI: ");
            await foreach (var item in result)
            {
                sb.Append(item);
                Console.Write(item.Content);
            }
            Console.WriteLine();
        
            history.AddAssistantMessage(sb.ToString()); // AI 回覆
        }
    }
}
