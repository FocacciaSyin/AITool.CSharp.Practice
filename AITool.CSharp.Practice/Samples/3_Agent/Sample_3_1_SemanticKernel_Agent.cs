using AITool.CSharp.Practice.Models.Settings;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;

namespace AITool.CSharp.Practice.Samples._3_Agent;

/// <summary>
/// 使用  https://github.com/microsoft/semantic-kernel?tab=readme-ov-file#basic-agent---net 基本範例
/// </summary>
/// <param name="openAiApiKey"></param>
public class Sample_3_1_SemanticKernel_Agent(IOptions<OpenAISettings> openAiApiKey)
{
    private readonly OpenAISettings _openAiApiKey = openAiApiKey.Value;

    public async Task ExecuteAsync()
    {
        var kernel = Kernel.CreateBuilder()
            .AddOpenAIChatCompletion(
                modelId: "gpt-4o",
                apiKey: _openAiApiKey.ApiKey)
            .Build();

        var agent = new ChatCompletionAgent
        {
            Name = "SK-AGENT-健身專家",
            Instructions = """
                           你是一個健身減重教練，
                           - 永遠使用繁體中文回覆
                           - 不要回答健身以外的問題
                           - 不要回答任何程式相關問題
                           - 專注於幫使用建立運動相關的建議(如：建議的運動種類/組數等等)
                           """,
            Kernel = kernel,
        };

        await foreach (var response in agent.InvokeAsync("我想減重可以給我甚麼建議嗎?"))
        {
            Console.WriteLine(response.Message);
        }
    }
}
