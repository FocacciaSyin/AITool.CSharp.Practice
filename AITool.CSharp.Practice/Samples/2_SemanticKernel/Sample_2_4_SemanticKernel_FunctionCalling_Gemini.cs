using AITool.CSharp.Practice.Models.AIPlugins;
using AITool.CSharp.Practice.Models.Helpers;
using AITool.CSharp.Practice.Models.Settings;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;

namespace AITool.CSharp.Practice.Samples._2_SemanticKernel;

/// <summary>
/// 2.4 使用 SemanticKernel + Gemini API Key +  Function Calling 範例
/// </summary>
/// <remarks>
/// 展示如何讓 AI 透過 Function Calling 呼叫 C# 方法
/// </remarks>
public class Sample_2_4_SemanticKernel_FunctionCalling_Gemini(IOptions<GeminiSettings> geminiSettings)
{
    private readonly GeminiSettings _geminiSettings = geminiSettings.Value;

    public async Task ExecuteAsync()
    {
        //與 2.3 範例幾乎一樣，差別在於這邊使用 Gemini 的擴充方法
        var kernelBuilder = Kernel.CreateBuilder()
            .AddGoogleAIGeminiChatCompletion(
                modelId: _geminiSettings.ModelId,
                apiKey: _geminiSettings.ApiKey,
                httpClient: HttpLoggerHelper.GetHttpClient(true)
            );

        kernelBuilder.Plugins.AddFromType<TimeInformation>();

        var kernel = kernelBuilder.Build();
        var settings = new PromptExecutionSettings
        {
            FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
        };

        Console.WriteLine("Example 1: 詢問AI無法回答正確答案的問題");
        Console.WriteLine(await kernel.InvokePromptAsync("從現在開始到跨年還有幾天?"));

        Console.WriteLine("\nExample 2: 使用 templated prompts + 直接指定 KernelFunction");
        Console.WriteLine(await kernel.InvokePromptAsync("現在時間是 {{TimeInformation.GetCurrentTime}}.從現在開始到跨年還有幾天"));

        Console.WriteLine("\nExample 3: 讓AI自動調用可用的方法");
        Console.WriteLine(await kernel.InvokePromptAsync("從現在開始到跨年還有幾天? 併解釋你的思路", new(settings)));
    }
}
