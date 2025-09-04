using System.ClientModel;
using AITool.CSharp.Practice.Models;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;
using OpenAI;

namespace AITool.CSharp.Practice.Samples;

/// <summary>
/// 2.1 使用 SemanticKernel + GitHub OpenAI 基本詢問
/// </summary>
/// <remarks>
/// 可以使用 GitHub Model 內的模型，這樣就可以不用花錢買 OpenAI Token
/// </remarks>
/// <param name="githubSettings"></param>
public class Sample_2_1_SemanticKernelWithGitHub_ChatCompletion(IOptions<GitHubSettings> githubSettings)
{
    private readonly GitHubSettings _gitHubSettings = githubSettings.Value;

    public async Task ExecuteAsync()
    {
        var client = new OpenAIClient(
            credential: new ApiKeyCredential(_gitHubSettings.ApiKey),
            options: new OpenAIClientOptions
            {
                Endpoint = new Uri(_gitHubSettings.EndPoint)
            });

        // 1. Kernel Build 基本範例
        var kernel = Kernel.CreateBuilder()
            .AddOpenAIChatCompletion(_gitHubSettings.ModelId, client)
            .Build();

        //基本詢問的功能
        Console.WriteLine(await kernel.InvokePromptAsync("你現在使用的模型是甚麼??"));
    }
}
