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
        
        // 設定 OpenAI 聊天完成服務
        var modelId = Configuration.GetOpenAIModel();
        builder.AddOpenAIChatCompletion(
            modelId: modelId,
            apiKey: openAiApiKey
        );
        
        // 設定日誌
        builder.Services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.AddConsole().SetMinimumLevel(LogLevel.Warning);
        });
        
        return builder.Build();
    }
    
    /// <summary>
    /// 取得 Semantic Kernel 版本資訊
    /// </summary>
    /// <returns>版本資訊字串</returns>
    public static string GetKernelVersion()
    {
        var assembly = typeof(Kernel).Assembly;
        var version = assembly.GetName().Version;
        return $"Microsoft.SemanticKernel Version: {version}";
    }
}
