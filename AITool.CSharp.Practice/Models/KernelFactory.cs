using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;

namespace AITool.CSharp.Practice.Models;

public static class KernelFactory
{
    /// <summary>
    /// 建立並設定 Semantic Kernel 實例
    /// </summary>
    /// <returns>設定好的 Kernel 實例</returns>
    public static Kernel CreateKernel()
    {
        var builder = Kernel.CreateBuilder();

        // 讀取 OpenAI API 金鑰
        var openAiApiKey = Configuration.GetOpenAIApiKey();
        
        Console.WriteLine($"使用的 OpenAI 模型: {openAiApiKey.Model}");
        Console.WriteLine($"使用的 OpenAI 模型: {openAiApiKey.ApiKey}");
        
        // 設定 OpenAI 聊天完成服務
        builder.AddOpenAIChatCompletion(
            modelId: openAiApiKey.Model,
            apiKey: openAiApiKey.ApiKey
        );

        // 設定日誌
        builder.Services.AddLogging(loggingBuilder => { loggingBuilder.AddConsole().SetMinimumLevel(LogLevel.Warning); });

        return builder.Build();
    }
}
