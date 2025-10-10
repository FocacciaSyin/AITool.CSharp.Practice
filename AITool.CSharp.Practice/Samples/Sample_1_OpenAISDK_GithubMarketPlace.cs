using System.ClientModel;
using AITool.CSharp.Practice.Models;
using AITool.CSharp.Practice.Models.Settings;
using OpenAI;
using OpenAI.Chat;

namespace AITool.CSharp.Practice.Samples;

/// <summary>
/// 使用 Github MarketPlace + OpenAI SDK 基本詢問
/// </summary>
public static class Sample_1_OpenAISDK_GithubMarketPlace
{
    /// <summary>
    /// GitHub MarketPlace OpenAI 範例
    /// https://github.com/marketplace
    /// https://github.com/marketplace/models/azure-openai/gpt-4-1/playground
    /// </summary>
    public static async Task RunAsync(GitHubSettings gitHubSettings)
    {
        await Task.Run(() =>
        {
            var client = new ChatClient(
                model: gitHubSettings.ModelId,
                credential: new ApiKeyCredential(gitHubSettings.ApiKey),
                options: new OpenAIClientOptions
                {
                    Endpoint = new Uri(gitHubSettings.EndPoint)
                });

            var messages = new List<ChatMessage>
            {
                new SystemChatMessage("你是一個健身減重教練"),
                new UserChatMessage("給我重訓菜單"),
            };

            var response = client.CompleteChat(
                messages,
                options: new ChatCompletionOptions
                {
                    Temperature = 1f,
                    TopP = 1f,
                });

            Console.WriteLine(response.Value.Content[0].Text);
        });
    }
}
