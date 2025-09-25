using System.Text.Json;
using System.Text.Json.Serialization;
using AITool.CSharp.Practice.Models.Settings;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace AITool.CSharp.Practice.Samples;

/// <summary>
/// 使用 SemanticKernel + OpenAI API Key 處理 聊天回傳格式為特定Model
/// </summary>
/// <param name="openAiApiKey">OpenAI 設定注入</param>
public class Sample_2_0_1_SemanticKernel_ChatCompletion_ResponseFormat(IOptions<OpenAISettings> openAiApiKey)
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

        // 2. 設定提示執行參數（要求回傳符合 AddressModel 的結構）
        var settings = new OpenAIPromptExecutionSettings
        {
            ResponseFormat = typeof(AddressModel), 
            ChatSystemPrompt = """
                               重要：你是房仲人員，不回答與房地產無關的問題。
                               1. 此工具用途為將使用者輸入的文字轉換為台灣地址格式
                               2. 使用繁體中文回覆結果
                               3. 若無法判斷則輸出可判斷之欄位，其餘為 null
                               4. 若問題與房地產、地址、房屋買賣租賃無關，請設定：
                                  - isRefused: true
                                  - refusedReason: "此問題與房地產無關"
                                  - city, district 設為空字串
                               5. 若是房地產相關問題，請設定 isRefused: false
                               """
        };

        // 3. 多組使用者輸入（問題）
        var questions = new[]
        {
            "我想要買 中和區 1200萬的房子",
            "台北市大安區 信義路三段 附近的公寓",
            "想找 高雄 三民區 透天",
            "桃園 中壢 區域房價資訊",
            "新北市板橋區文化路一段 25 號",
            "今天天氣如何?",
            "高雄三民區有甚麼美食?",
        };

        var chat = kernel.GetRequiredService<IChatCompletionService>();


        // 4. 逐筆處理問題並解析為 AddressModel
        foreach (var question in questions)
        {
            // 呼叫模型取得聊天/結構化結果
            var rawMessage = await chat.GetChatMessageContentAsync(question, settings);
            var rawText = rawMessage.ToString();

            AddressModel? address = null;
            try
            {
                // 嘗試反序列化為 AddressModel
                address = JsonSerializer.Deserialize<AddressModel>(rawText);
            }
            catch (JsonException)
            {
                // 失敗時保留 null（可視需求後續再補強容錯）
            }

            // 即時輸出單筆結果（根據是否拒絕回答調整顯示）
            if (address?.IsRefused == true)
            {
                Console.WriteLine($"=== 單筆結果 ===\n問題: {question}\n模型回傳: {rawText}\n狀態: 已拒絕回答 - {address.RefusedReason}\n");
            }
            else
            {
                Console.WriteLine($"=== 單筆結果 ===\n問題: {question}\n模型回傳: {rawText}\n解析: {address?.City}{address?.District}{address?.Street}{(address?.No != null ? address.No + "號" : string.Empty)}\n");
            }
        }
    }

    /// <summary>
    /// 地址模型
    /// </summary>
    public class AddressModel
    {
        /// <summary>
        /// 縣市
        /// </summary>
        [JsonPropertyName("city")]
        public required string City { get; set; }

        /// <summary>
        /// 行政區
        /// </summary>
        [JsonPropertyName("district")]
        public required string District { get; set; }

        /// <summary>
        /// 路名
        /// </summary>
        [JsonPropertyName("street")]
        public string? Street { get; set; }

        /// <summary>
        /// 號
        /// </summary>
        [JsonPropertyName("no")]
        public string? No { get; set; }
        
        //---- 以下為 讓 LLM 分析使用 ----

        /// <summary>
        /// 是否拒絕回答（非房地產相關問題）
        /// </summary>
        [JsonPropertyName("isRefused")]
        public bool IsRefused { get; set; }

        /// <summary>
        /// 拒絕原因（當 IsRefused 為 true 時）
        /// </summary>
        [JsonPropertyName("refusedReason")]
        public string? RefusedReason { get; set; }
    }
}
