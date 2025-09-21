using AITool.CSharp.Practice.Models.Settings;
using AITool.CSharp.Practice.Samples.Plugins;
using Microsoft.Extensions.Options;

namespace AITool.CSharp.Practice.Samples;

/// <summary>
/// 混合式股票顧問 Agent Demo - 展示功能而不需要實際 API 連接
/// </summary>
public class Sample_5_2_HybridStockAdvisorDemo
{
    public async Task ExecuteAsync()
    {
        Console.WriteLine("🚀 混合式股票顧問 Agent Demo 啟動中...\n");

        // 建立 Plugins 實例
        var stockPlugin = new StockDbPlugin(Options.Create(new DatabaseSettings()));
        var strategyPlugin = new InvestmentStrategyPlugin();

        Console.WriteLine("✅ 股票顧問 Agent 已準備就緒！\n");
        Console.WriteLine("💡 以下是功能示範：");
        Console.WriteLine("=".PadRight(50, '=') + "\n");

        // Demo 1: 股價數據查詢
        Console.WriteLine("🤔 用戶提問：最近大盤走勢如何？");
        Console.WriteLine("🔍 系統動作：調用 StockData.GetLatestClosePrice()");
        Console.WriteLine("-".PadRight(40, '-'));
        
        var latestPrice = await stockPlugin.GetLatestClosePrice();
        Console.WriteLine($"📊 數據結果：{latestPrice}");
        Console.WriteLine();

        // Demo 2: 歷史數據查詢
        Console.WriteLine("🔍 系統動作：調用 StockData.GetHistoryClosePrices() 查詢近期趨勢");
        Console.WriteLine("-".PadRight(40, '-'));
        
        var endDate = DateTime.Now.ToString("yyyy-MM-dd");
        var startDate = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd");
        var historyPrices = await stockPlugin.GetHistoryClosePrices(startDate, endDate);
        Console.WriteLine($"📈 歷史數據：\n{historyPrices}");
        Console.WriteLine();

        // Demo 3: 策略分析
        Console.WriteLine("🔍 系統動作：調用 InvestmentStrategy.GetMovingAverageStrategy() 分析趨勢");
        Console.WriteLine("-".PadRight(40, '-'));
        
        var maStrategy = strategyPlugin.GetMovingAverageStrategy();
        Console.WriteLine($"🧠 策略建議：\n{maStrategy.Substring(0, Math.Min(200, maStrategy.Length))}...");
        Console.WriteLine();

        // Demo 4: 模擬完整回覆
        Console.WriteLine("🤖 股票顧問綜合回覆：");
        Console.WriteLine("-".PadRight(40, '-'));
        
        var response = GenerateMockResponse(latestPrice, historyPrices, maStrategy);
        Console.WriteLine(response);
        
        Console.WriteLine("\n" + "=".PadRight(50, '=') + "\n");

        // Demo 5: 第二個問題
        Console.WriteLine("🤔 用戶提問：依照策略現在適合買入嗎？");
        Console.WriteLine("🔍 系統動作：調用多個策略進行綜合分析");
        Console.WriteLine("-".PadRight(40, '-'));

        var rsiStrategy = strategyPlugin.GetRsiStrategy();
        var sentimentStrategy = strategyPlugin.GetMarketSentimentStrategy();
        
        var buyingAdvice = GenerateBuyingAdvice(latestPrice, rsiStrategy, sentimentStrategy);
        Console.WriteLine($"🤖 綜合建議：\n{buyingAdvice}");
        
        Console.WriteLine("\n" + "=".PadRight(50, '=') + "\n");
        Console.WriteLine("✅ Demo 完成！實際使用時將透過 OpenAI 自動調用相關功能並生成專業回覆。");
    }

    private string GenerateMockResponse(string latestPrice, string historyData, string strategy)
    {
        return $"""
               基於最新數據分析，目前台股大盤情況如下：

               📊 **當前市況**
               {latestPrice}

               📈 **近期走勢分析**
               根據過去一週的交易數據顯示，大盤呈現震盪格局。從歷史價格變化來看，
               市場在區間內整理，尚未出現明確的趨勢突破。

               🔍 **技術分析觀點**
               採用移動平均線策略分析：
               - 短期均線與長期均線呈現糾結狀態
               - 建議觀察是否出現黃金交叉或死亡交叉信號
               - 成交量配合度將是關鍵確認因素

               ⚠️ **風險提醒**
               以上分析僅供參考，不構成投資建議。投資有風險，請謹慎評估自身風險承受能力。
               """;
    }

    private string GenerateBuyingAdvice(string latestPrice, string rsiStrategy, string sentimentStrategy)
    {
        return $"""
               綜合多項投資策略分析，對於是否適合買入的評估如下：

               📊 **技術指標分析**
               • RSI 相對強弱指標：目前處於中性區間，未出現明顯過買或過賣信號
               • 建議等待 RSI 進入過賣區域（30以下）再考慮進場

               😰😤 **市場情緒評估**
               • 當前市場情緒偏向謹慎觀望
               • 成交量表現平淡，缺乏明確方向性
               • 外資態度中性，未見大幅買賣超

               💡 **綜合建議**
               基於當前分析：
               1. **不建議大舉買入**：技術指標未給出明確買進信號
               2. **可考慮分批布局**：若為長期投資者，可小額試單
               3. **密切關注**：重要支撐壓力位的突破情況

               🔍 **關鍵觀察點**
               • 是否跌破重要支撐位
               • 成交量是否出現放大
               • 國際股市走勢影響

               ⚠️ **重要聲明**
               本分析僅供參考，不構成任何投資建議。投資決策應基於您自身的財務狀況和風險承受能力。
               """;
    }
}