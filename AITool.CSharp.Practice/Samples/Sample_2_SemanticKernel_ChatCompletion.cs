using AITool.CSharp.Practice.Models;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;

namespace AITool.CSharp.Practice.Samples;

/// <summary>
/// 使用 SemanticKernel 來呼叫 OpenAI ChatCompletion 範例
/// </summary>
/// <param name="openAiApiKey"></param>
public class Sample_2_SemanticKernel_ChatCompletion(IOptions<OpenAISettings> openAiApiKey)
{
    private readonly OpenAISettings _openAiApiKey = openAiApiKey.Value;

    public async Task ExecuteAsync()
    {
        // 1. Kernel Build 基本範例
        var kernel = Kernel.CreateBuilder()
            .AddOpenAIChatCompletion(
                modelId: _openAiApiKey.Model,
                apiKey: _openAiApiKey.ApiKey)
            .Build();

        //基本詢問的功能
        Console.WriteLine(await kernel.InvokePromptAsync("你現在使用的模型是甚麼??"));
    }
}
