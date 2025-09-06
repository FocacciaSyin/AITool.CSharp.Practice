using AITool.CSharp.Practice.Models.AIPlugins;
using AITool.CSharp.Practice.Models.Helpers;
using AITool.CSharp.Practice.Models.Settings;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace AITool.CSharp.Practice.Samples;

/// <summary>
/// 2.2 使用 SemanticKernel + OpenAI API Key +  Function Calling 範例
/// </summary>
/// <remarks>
/// 展示如何讓 AI 透過 Function Calling 呼叫 C# 方法
/// </remarks>
public class Sample_2_3_SemanticKernel_FunctionCalling(IOptions<OpenAISettings> openAiSettings)
{
    private readonly OpenAISettings _openAiSettings = openAiSettings.Value;

    public async Task ExecuteAsync()
    {
        var kernelBuilder = Kernel.CreateBuilder()
            .AddOpenAIChatCompletion(
                modelId: _openAiSettings.Model,
                apiKey: _openAiSettings.ApiKey,
                httpClient: HttpLoggerHelper.GetHttpClient(false)//開啟後可以看到 Plugin 是不是有被使用
            );

        // 註冊可被 AI 呼叫的 C# 方法
        kernelBuilder.Plugins.AddFromType<TimeInformation>();

        var kernel = kernelBuilder.Build();
        var settings = new PromptExecutionSettings
        {
            FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
        };

        Console.WriteLine("Example 1: 詢問AI無法回答正確答案的問題");
        Console.WriteLine(await kernel.InvokePromptAsync("從現在開始到跨年還有幾天?"));

        //有被觸發是會進入中斷點的，所以可以 Debug
        Console.WriteLine("\nExample 2: 使用 templated prompts + 直接指定 KernelFunction");
        Console.WriteLine(await kernel.InvokePromptAsync("現在時間是 {{TimeInformation.GetCurrentTime}}.從現在開始到跨年還有幾天"));

        Console.WriteLine("\nExample 3: 讓AI自動調用可用的方法");
        Console.WriteLine(await kernel.InvokePromptAsync("從現在開始到跨年還有幾天? 併解釋你的思路", new(settings)));
    }
}
