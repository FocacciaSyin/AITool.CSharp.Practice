using System.ComponentModel;
using AITool.CSharp.Practice.Models.Helpers;
using AITool.CSharp.Practice.Models.Settings;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.ChatCompletion;

namespace AITool.CSharp.Practice.Samples;

/// <summary>
/// 使用 https://github.com/microsoft/semantic-kernel?tab=readme-ov-file#agent-with-plugin---net 範例
/// </summary>
/// <param name="openAiApiKey"></param>
public class Sample_3_1_SemanticKernel_Agent_Plugins(IOptions<OpenAISettings> openAiApiKey)
{
    private readonly OpenAISettings _openAiApiKey = openAiApiKey.Value;

    public async Task ExecuteAsync()
    {
        var kernel = Kernel.CreateBuilder()
            .AddOpenAIChatCompletion(
                modelId: "gpt-4o",
                apiKey: _openAiApiKey.ApiKey,
                httpClient: HttpLoggerHelper.GetHttpClient(true))
            .Build();

        kernel.Plugins.Add(KernelPluginFactory.CreateFromType<MenuPlugin>());

        ChatCompletionAgent agent =
            new()
            {
                Name = "SK-AGENT-健身專家",
                Instructions = """
                               你是一個健身減重教練，
                               - 永遠使用繁體中文回覆
                               - 不要回答健身以外的問題
                               - 不要回答任何程式相關問題
                               - 專注於幫使用建立運動相關的建議(如：建議的運動種類/組數等等)
                               """,
                Kernel = kernel,
                Arguments = new KernelArguments(new PromptExecutionSettings() { FunctionChoiceBehavior = FunctionChoiceBehavior.Auto() })
            };

        
        await foreach (AgentResponseItem<ChatMessageContent> response
                       in agent.InvokeAsync("我想運動1小時要多少錢?"))
        {
            Console.WriteLine(response.Message);
        }
    }

    sealed class MenuPlugin
    {
        [KernelFunction, Description("提供今日的健身課程推薦。")]
        public string GetWorkoutSpecials() =>
            """
            今日推薦教練課程:
            - 重量訓練: 全身肌群訓練
            - 有氧課程: 高強度間歇訓練 (HIIT)
            - 有氧運動: 5K慢跑基礎課程
            """;

        [KernelFunction, Description("提供指定健身課程的費用。")]
        public string GetCourseFee([Description("健身課程名稱。")] string courseName) =>
            courseName.ToLower() switch
            {
                "一小時自由使用" => "50",
                "兩小時自由使用" => "100",
                "三小時以上自由使用" => "150",
                "年費" => "10000",
                "HIIT" => "600",
                "5K慢跑基礎課程" => "600",
                "重量訓練" => "1000",
                "飛輪" => "500",
                _ => "查無此課程，請確認輸入名稱。"
            };
    }
}
