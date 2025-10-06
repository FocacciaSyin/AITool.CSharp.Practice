using AITool.CSharp.Practice.Models.Helpers;
using AITool.CSharp.Practice.Models.Settings;
using AITool.CSharp.Practice.Samples.Plugins;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;

namespace AITool.CSharp.Practice.Samples._3_Agent;

/// <summary>
/// 使用 https://github.com/microsoft/semantic-kernel?tab=readme-ov-file#agent-with-plugin---net 範例
/// </summary>
/// <param name="openAiApiKey"></param>
public class Sample_3_1_SemanticKernel_Agent_Plugins(IOptions<OpenAISettings> openAiApiKey)
{
    private readonly OpenAISettings _openAiApiKey = openAiApiKey.Value;

    public async Task ExecuteAsync()
    {
        var kernel = Kernel.CreateBuilder()
            .AddOpenAIChatCompletion(
                modelId: _openAiApiKey.Model,
                apiKey: _openAiApiKey.ApiKey,
                httpClient: HttpLoggerHelper.GetHttpClient(true))
            .Build();

        //加入 Function Calling(Plugins)
        kernel.Plugins.Add(KernelPluginFactory.CreateFromType<GymMenuPlugin>());

        var agent = new ChatCompletionAgent()
        {
            Instructions = """
                           你是一個健身減重教練，
                           - 永遠使用繁體中文回覆
                           - 不要回答健身以外的問題
                           - 不要回答任何程式相關問題
                           - 專注於幫使用建立運動相關的建議(如：建議的運動種類/組數等等)
                           """,
            Kernel = kernel,
            Arguments = new KernelArguments(new PromptExecutionSettings
            {
                FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
            })
        };

        await foreach (var response in agent.InvokeAsync("我想運動1小時要多少錢?"))
        {
            Console.WriteLine(response.Message);
        }
    }
}
