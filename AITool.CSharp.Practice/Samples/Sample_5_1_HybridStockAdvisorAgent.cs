using AITool.CSharp.Practice.Models.Helpers;
using AITool.CSharp.Practice.Models.Settings;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using AITool.CSharp.Practice.Samples.Plugins;

namespace AITool.CSharp.Practice.Samples;

/// <summary>
/// 混合式股票顧問 Agent - 結合 MSSQL 數據與 RAG 策略
/// </summary>
public class Sample_5_1_HybridStockAdvisorAgent
{
    private readonly OpenAISettings _openAiSettings;
    private readonly QdrantSettings _qdrantSettings;
    private readonly DatabaseSettings _databaseSettings;

    public Sample_5_1_HybridStockAdvisorAgent(
        IOptions<OpenAISettings> openAiSettings,
        IOptions<QdrantSettings> qdrantSettings,
        IOptions<DatabaseSettings> databaseSettings)
    {
        _openAiSettings = openAiSettings.Value;
        _qdrantSettings = qdrantSettings.Value;
        _databaseSettings = databaseSettings.Value;
    }

    public async Task ExecuteAsync()
    {
        Console.WriteLine("🚀 混合式股票顧問 Agent 啟動中...\n");

        try
        {
            // 建立 Semantic Kernel
            var kernel = CreateKernelWithPlugins();

            // 建立混合式股票顧問 Agent
            var agent = CreateStockAdvisorAgent(kernel);

            Console.WriteLine("✅ 股票顧問 Agent 已準備就緒！\n");
            Console.WriteLine("💡 您可以詢問：");
            Console.WriteLine("  • 最近大盤走勢如何？");
            Console.WriteLine("  • 依照策略現在適合買入嗎？");
            Console.WriteLine("  • 布林帶策略怎麼看現在的市場？");
            Console.WriteLine("  • 移動平均線指標分析");
            Console.WriteLine("\n" + "=".PadRight(50, '=') + "\n");

            // 測試範例問題
            await TestSampleQuestions(agent);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ 初始化失敗：{ex.Message}");
            Console.WriteLine($"詳細錯誤：{ex}");
        }
    }

    private Kernel CreateKernelWithPlugins()
    {
        var kernel = Kernel.CreateBuilder()
            .AddOpenAIChatCompletion(
                modelId: _openAiSettings.Model,
                apiKey: _openAiSettings.ApiKey,
                httpClient: HttpLoggerHelper.GetHttpClient(true))
            .Build();

        // 加入股票資料庫 Plugin
        kernel.Plugins.Add(KernelPluginFactory.CreateFromObject(
            new StockDbPlugin(Microsoft.Extensions.Options.Options.Create(_databaseSettings)),
            "StockData"));

        // 加入投資策略知識庫 Plugin
        kernel.Plugins.Add(KernelPluginFactory.CreateFromObject(
            new InvestmentStrategyPlugin(),
            "InvestmentStrategy"));

        Console.WriteLine("✅ 已載入股票數據和投資策略插件");

        return kernel;
    }

    private ChatCompletionAgent CreateStockAdvisorAgent(Kernel kernel)
    {
        return new ChatCompletionAgent
        {
            Name = "台股顧問專家",
            Instructions = """
                           你是一位專業的台股大盤分析師和投資顧問，具備以下特性：
                           
                           📊 專業領域：
                           - 專精於台灣證券交易所大盤指數分析
                           - 只討論台股大盤走勢，不提供個股建議
                           - 不討論海外股市或其他金融商品
                           
                           🔍 分析方法：
                           - 結合即時股價數據進行技術分析
                           - 運用投資策略知識庫提供專業建議
                           - 整合量化指標與基本面分析
                           
                           💬 回覆風格：
                           - 永遠使用繁體中文回覆
                           - 提供客觀、專業的分析意見
                           - 明確標註分析依據和資料來源
                           - 適當提醒投資風險
                           
                           🚫 限制條件：
                           - 不提供個股推薦或買賣建議
                           - 不討論加密貨幣、期貨、選擇權等衍生性商品
                           - 不提供非台股相關的投資建議
                           - 所有建議僅供參考，不構成投資建議
                           
                           🛠️ 可用工具：
                           - StockData.GetLatestClosePrice：查詢最新大盤收盤價
                           - StockData.GetHistoryClosePrices：查詢歷史大盤收盤價
                           - InvestmentStrategy.GetBollingerBandsStrategy：布林帶策略分析
                           - InvestmentStrategy.GetMovingAverageStrategy：移動平均線策略
                           - InvestmentStrategy.GetRsiStrategy：RSI 相對強弱指標策略
                           - InvestmentStrategy.GetMarketSentimentStrategy：市場情緒分析策略
                           
                           當收到詢問時，請：
                           1. 先使用 StockData 功能查詢相關股價數據
                           2. 根據問題類型，使用相應的投資策略工具
                           3. 綜合數據和策略提供專業分析
                           4. 明確說明分析基礎和風險提醒
                           5. 結論部分要包含「僅供參考，不構成投資建議」
                           """,
            Kernel = kernel,
            Arguments = new KernelArguments(new PromptExecutionSettings
            {
                FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
            })
        };
    }

    private async Task TestSampleQuestions(ChatCompletionAgent agent)
    {
        var sampleQuestions = new[]
        {
            "最近大盤走勢如何？",
            "依照策略現在適合買入嗎？"
        };

        foreach (var question in sampleQuestions)
        {
            Console.WriteLine($"🤔 用戶提問：{question}");
            Console.WriteLine("🤖 股票顧問回覆：");
            Console.WriteLine("-".PadRight(40, '-'));

            await foreach (var response in agent.InvokeAsync(question))
            {
                Console.WriteLine(response.Message);
            }

            Console.WriteLine("\n" + "=".PadRight(50, '=') + "\n");
        }
    }
}