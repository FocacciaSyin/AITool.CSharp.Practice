using System.ClientModel;
using AITool.CSharp.Practice.Models;
using Microsoft.Extensions.Options;
using OpenAI;
using OpenAI.Chat;

namespace AITool.CSharp.Practice.Samples;

public class Sample_1_GitHubOpenAI
{
    private readonly GitHubSettings _gitHubSettings;

    public Sample_1_GitHubOpenAI(IOptions<GitHubSettings> githubSettings)
    {
        _gitHubSettings = githubSettings.Value;
    }

    /// <summary>
    /// GitHub MarketPlace OpenAI 範例
    /// </summary>
    public void Execute()
    {
        var client = new ChatClient(
            model: _gitHubSettings.ModelId,
            credential: new ApiKeyCredential(_gitHubSettings.ApiKey),
            options: new OpenAIClientOptions
            {
                Endpoint = new Uri(_gitHubSettings.EndPoint)
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
    }
}
