using AITool.CSharp.Practice.Models.Settings;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace AITool.CSharp.Practice.Samples;

/// <summary>
/// 使用 SemanticKernel + OpenAI API Key 處理 Json 相關的功能
/// </summary>
/// <param name="openAiApiKey">OpenAI 設定注入</param>
public class Sample_2_0_1_SemanticKernel_ChatCompletion_JsonFormat(IOptions<OpenAISettings> openAiApiKey)
{
    private readonly OpenAISettings _openAiApiKey = openAiApiKey.Value;

    public async Task ExecuteAsync()
    {
        // 1. 建立 Kernel
        var kernel = Kernel.CreateBuilder()
            .AddOpenAIChatCompletion(
                modelId: _openAiApiKey.Model,
                apiKey: _openAiApiKey.ApiKey)
            .Build();

        KernelArguments arguments = new(new OpenAIPromptExecutionSettings
        {
            ResponseFormat = "json_object",
            ChatSystemPrompt = """
                               1. 此工具用途為將使用者輸入的文字轉換為台灣地址格式
                               2. 請用 JSON 結構回傳結果
                               3. {{$topic}}
                               """
        })
        {
            { "topic", "資料請使用繁體中文回覆" }
        };
        Console.WriteLine(await kernel.InvokePromptAsync("我想要買 中和區 1200萬的房子", arguments));
    }
    
    //todo 建立 Model 且要用預期的結構回傳
}
