using System.ClientModel;
using AITool.CSharp.Practice.Models.Settings;
using Microsoft.Agents.AI;
using OpenAI;
using OpenAI.Chat;

namespace AITool.CSharp.Practice.Samples._5_Agent_Framework;

/// <summary>
/// 2025/10/12 跟 OpenAI Client 與 Opentelemetry 整合還不是那麼好
/// </summary>
public class Sample_5_1_AgentFramework
{
    public static async Task RunAsync(OpenAISettings openAISettings)
    {
        AIAgent agent = new OpenAIClient(openAISettings.ApiKey)
            .GetChatClient(openAISettings.Model)
            .CreateAIAgent(instructions: "You are good at telling jokes.", name: "Joker");

        UserChatMessage chatMessage = new("Tell me a joke about a pirate.");

        Console.WriteLine("Non-streaming response:");
        ChatCompletion chatCompletion = await agent.RunAsync([chatMessage]);
        Console.WriteLine(chatCompletion.Content.Last().Text);

        // Console.WriteLine("Streaming response:");
        // AsyncCollectionResult<StreamingChatCompletionUpdate> completionUpdates = agent.RunStreamingAsync([chatMessage]);
        // await foreach (StreamingChatCompletionUpdate completionUpdate in completionUpdates)
        // {
        //     if (completionUpdate.ContentUpdate.Count > 0)
        //     {
        //         Console.WriteLine(completionUpdate.ContentUpdate[0].Text);
        //     }
        // }
    }
}
