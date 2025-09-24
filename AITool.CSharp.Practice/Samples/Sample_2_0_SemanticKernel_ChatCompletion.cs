using AITool.CSharp.Practice.Models;
using AITool.CSharp.Practice.Models.Settings;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace AITool.CSharp.Practice.Samples;

/// <summary>
/// 使用 SemanticKernel + OpenAI API Key 進行最基本的聊天補全（Chat Completion）範例
/// </summary>
/// <param name="openAiApiKey">OpenAI 設定注入</param>
public class Sample_2_0_SemanticKernel_ChatCompletion(IOptions<OpenAISettings> openAiApiKey)
{
    private readonly OpenAISettings _openAiApiKey = openAiApiKey.Value;

    public async Task ExecuteAsync()
    {
        // 1. 建立 Kernel（最基本建置方式）
        var kernel = Kernel.CreateBuilder()
            .AddOpenAIChatCompletion(
                modelId: _openAiApiKey.Model,
                apiKey: _openAiApiKey.ApiKey)
            .Build();

        // 基本單輪詢問（可視需求取消註解）
        // Console.WriteLine(await kernel.InvokePromptAsync("你現在使用的模型是什麼？"));

        // 以下為 SemanticKernel 官方教學延伸練習
        // 參考範例：https://github.com/microsoft/semantic-kernel/blob/main/dotnet/samples/GettingStarted/Step1_Create_Kernel.cs

        // 範例 1：最基本的 Prompt 呼叫
        Console.WriteLine("---------------- 範例 1 開始 ----------------");
        Console.WriteLine(await kernel.InvokePromptAsync("天空是什麼顏色？"));

        // 範例 2：使用樣板（Template）參數替換
        Console.WriteLine("---------------- 範例 2 開始 ----------------");
        KernelArguments arguments = new() { { "topic", "海洋" } };
        Console.WriteLine(await kernel.InvokePromptAsync("{{$topic}} 的顏色是？", arguments));


        // 範例 3：串流輸出（Streaming）示範 - 逐步輸出補全結果
        Console.WriteLine("---------------- 範例 3 開始 ----------------");
        await foreach (var update in kernel.InvokePromptStreamingAsync("{{$topic}} 是什麼顏色？請提供詳細的說明。", arguments))
        {
            Console.Write(update);
        }

        Console.WriteLine();

        Console.WriteLine("---------------- 範例 4 開始 ----------------");
        // 範例 4：指定執行設定（MaxTokens / Temperature）並使用樣板
        arguments = new(new OpenAIPromptExecutionSettings
        {
            MaxTokens = 500,
            Temperature = 0.5
        })
        {
            { "topic", "狗" }
        };
        Console.WriteLine(await kernel.InvokePromptAsync("請說一個關於 {{$topic}} 的故事。", arguments));

        Console.WriteLine("---------------- 範例 5 開始 ----------------");
        // 範例 5：設定回傳為 JSON 格式（ResponseFormat = json_object）
        arguments = new(new OpenAIPromptExecutionSettings
        {
            ResponseFormat = "json_object"
        })
        {
            { "topic", "巧克力" }
        };
        Console.WriteLine(await kernel.InvokePromptAsync("請以 JSON 格式產生一份 {{$topic}} 蛋糕的食譜。", arguments));
    }
}
