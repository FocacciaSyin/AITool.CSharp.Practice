using AITool.CSharp.Practice.Models;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;

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
        // Example 1. Invoke the kernel with a prompt and display the result
        // Console.WriteLine(await kernel.InvokePromptAsync("What color is the sky?"));
        // Console.WriteLine();

        // // Example 2. Invoke the kernel with a templated prompt and display the result
        // KernelArguments arguments = new() { { "topic", "sea" } };
        // Console.WriteLine(await kernel.InvokePromptAsync("What color is the {{$topic}}?", arguments));
        // Console.WriteLine();


        // // Example 3. Invoke the kernel with a templated prompt and stream the results to the display
        // await foreach (var update in kernel.InvokePromptStreamingAsync("What color is the {{$topic}}? Provide a detailed explanation.", arguments))
        // {
        //     Console.Write(update);
        // }
        // Console.WriteLine(string.Empty);

        // // Example 4. Invoke the kernel with a templated prompt and execution settings
        // arguments = new(new OpenAIPromptExecutionSettings { MaxTokens = 500, Temperature = 0.5 }) { { "topic", "dogs" } };
        // Console.WriteLine(await kernel.InvokePromptAsync("Tell me a story about {{$topic}}", arguments));

        // // Example 5. Invoke the kernel with a templated prompt and execution settings configured to return JSON
        // arguments = new(new OpenAIPromptExecutionSettings { ResponseFormat = "json_object" }) { { "topic", "chocolate" } };
        // Console.WriteLine(await kernel.InvokePromptAsync("Create a recipe for a {{$topic}} cake in JSON format", arguments));
    }
}
