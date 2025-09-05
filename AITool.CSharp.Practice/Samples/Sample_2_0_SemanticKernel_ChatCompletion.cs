using AITool.CSharp.Practice.Models;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace AITool.CSharp.Practice.Samples;

/// <summary>
/// 使用 SemanticKernel + OpenAI APIKey 基本詢問
/// </summary>
/// <param name="openAiApiKey"></param>
public class Sample_2_0_SemanticKernel_ChatCompletion(IOptions<OpenAISettings> openAiApiKey)
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
        // Console.WriteLine(await kernel.InvokePromptAsync("你現在使用的模型是甚麼??"));

        // 以下為  SemanticKernel 教學範例
        // https://github.com/microsoft/semantic-kernel/blob/main/dotnet/samples/GettingStarted/Step1_Create_Kernel.cs
        
        // Example 1. Kernel 基本詢問
        Console.WriteLine(await kernel.InvokePromptAsync("天空是什麼顏色?"));
        Console.WriteLine("---------------- End Example 1 ----------------");

        // // Example 2. Kernel 使用 樣板(Template)
        KernelArguments arguments = new() { { "topic", "海洋" } };
        Console.WriteLine(await kernel.InvokePromptAsync("{{$topic}} 的顏色是?", arguments));
        Console.WriteLine("---------------- End Example 2 ----------------");


        // // Example 3. Invoke the kernel with a templated prompt and stream the results to the display
        await foreach (var update in kernel.InvokePromptStreamingAsync("What color is the {{$topic}}? Provide a detailed explanation.", arguments))
        {
            Console.Write(update);
        }
        Console.WriteLine("---------------- End Example 3 ----------------");

        // // Example 4. Invoke the kernel with a templated prompt and execution settings
        arguments = new(new OpenAIPromptExecutionSettings
        {
            MaxTokens = 500,
            Temperature = 0.5
        })
        {
            {
                "topic", "dogs"
            }
        };
        Console.WriteLine(await kernel.InvokePromptAsync("Tell me a story about {{$topic}}", arguments));

        // // Example 5. Invoke the kernel with a templated prompt and execution settings configured to return JSON
        arguments = new(new OpenAIPromptExecutionSettings
        {
            ResponseFormat = "json_object"
        })
        {
            {
                "topic", "chocolate"
            }
        };
        Console.WriteLine(await kernel.InvokePromptAsync("Create a recipe for a {{$topic}} cake in JSON format", arguments));
    }
}
