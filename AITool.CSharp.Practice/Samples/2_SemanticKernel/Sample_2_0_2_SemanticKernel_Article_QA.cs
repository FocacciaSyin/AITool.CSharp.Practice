using System.Text.Json;
using System.Text.Json.Serialization;
using AITool.CSharp.Practice.Models.Settings;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace AITool.CSharp.Practice.Samples._2_SemanticKernel;

/// <summary>
/// 使用 SemanticKernel + OpenAI API Key 讀取文章並由 LLM 生成 10 筆 Q&A
/// </summary>
/// <param name="openAiApiKey">OpenAI 設定注入</param>
public class Sample_2_0_2_SemanticKernel_Article_QA(IOptions<OpenAISettings> openAiApiKey)
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

        // 2. 定義文章內容（可視需求從檔案讀取，此處硬編碼範例）
        var article = """
                      Semantic Kernel 是微軟開發的一個開源框架，用於整合大型語言模型（LLM）到應用程式中。
                      它提供了一個統一的介面，讓開發者能夠輕鬆地使用不同的 AI 模型，如 OpenAI 的 GPT 系列。
                      Semantic Kernel 的核心概念包括 Kernel、Plugins 和 Planners。
                      Kernel 是框架的核心，負責管理 AI 服務和記憶體。
                      Plugins 允許開發者將自訂功能整合到 AI 工作流程中。
                      Planners 則是用來自動化複雜任務的執行順序。
                      這個框架支援多種程式語言，包括 C#、Python 和 Java。
                      開發者可以使用 Semantic Kernel 來建構聊天機器人、自動化工具和智慧代理。
                      它強調模組化和可擴展性，讓 AI 應用開發更加高效。
                      """;

        // 3. 設定提示執行參數（要求回傳符合 QAModel 的結構）
        var settings = new OpenAIPromptExecutionSettings
        {
            ResponseFormat = typeof(QAList),
            ChatSystemPrompt = """
                               請基於提供的文章內容，生成 10 筆問題與答案（Q&A）。
                               每個 Q&A 應涵蓋文章中的關鍵資訊。
                               問題應簡潔明確，答案應基於文章內容並使用繁體中文。
                               請以 JSON 物件格式返回，包含 'items' 欄位，該欄位是一個陣列，包含 10 個物件，每個物件有 'question' 和 'answer' 欄位。
                               """
        };

        var chat = kernel.GetRequiredService<IChatCompletionService>();

        // 4. 呼叫模型取得結構化 Q&A 結果
        var rawMessage = await chat.GetChatMessageContentAsync(article, settings);
        var rawText = rawMessage.ToString();

        QAList? qaList = null;
        try
        {
            // 嘗試反序列化為 QAModel
            qaList = JsonSerializer.Deserialize<QAList>(rawText);
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"反序列化失敗: {ex.Message}");
            Console.WriteLine($"原始回傳: {rawText}");
            return;
        }

        // 5. 輸出 Q&A 結果
        if (qaList?.Items.Any() is true)
        {
            int index = 1;
            foreach (var qa in qaList.Items)
            {
                Console.WriteLine($"{index++}. 問題: {qa.Question}");
                Console.WriteLine($"   答案: {qa.Answer}");
                Console.WriteLine();
            }
        }
        else
        {
            Console.WriteLine("未能生成有效的 Q&A 列表。");
        }
        
        Console.ReadLine(); // 暫停以便把紀錄被 Openteleator 擷取
    }

    /// <summary>
    /// Q&A 列表
    /// </summary>
    public class QAList
    {
        /// <summary>
        /// Q&A 項目列表
        /// </summary>
        [JsonPropertyName("items")]
        public required List<QAItem> Items { get; set; }
    }

    /// <summary>
    /// 單筆 Q&A 項目
    /// </summary>
    public class QAItem
    {
        /// <summary>
        /// 問題
        /// </summary>
        [JsonPropertyName("question")]
        public required string Question { get; set; }

        /// <summary>
        /// 答案
        /// </summary>
        [JsonPropertyName("answer")]
        public required string Answer { get; set; }
    }
}
