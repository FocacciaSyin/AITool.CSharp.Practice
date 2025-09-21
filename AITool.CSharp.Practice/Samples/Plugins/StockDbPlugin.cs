using System.ComponentModel;
using Microsoft.Data.SqlClient;
using Microsoft.SemanticKernel;
using AITool.CSharp.Practice.Models.Settings;
using Microsoft.Extensions.Options;

namespace AITool.CSharp.Practice.Samples.Plugins;

public class StockDbPlugin
{
    private readonly DatabaseSettings _databaseSettings;

    public StockDbPlugin(IOptions<DatabaseSettings> databaseSettings)
    {
        _databaseSettings = databaseSettings.Value;
    }

    [KernelFunction, Description("查詢台股大盤最新收盤價")]
    public async Task<string> GetLatestClosePrice()
    {
        try
        {
            // 模擬查詢最新收盤價，實際應該從 MSSQL 資料庫查詢
            // 這裡使用模擬數據，因為我們沒有實際的 MSSQL 資料庫
            var latestPrice = await SimulateLatestClosePriceQuery();
            
            return $"台股大盤最新收盤價：{latestPrice:F2} 點";
        }
        catch (Exception ex)
        {
            return $"查詢最新收盤價時發生錯誤：{ex.Message}";
        }
    }

    [KernelFunction, Description("查詢台股大盤指定日期區間的歷史收盤價")]
    public async Task<string> GetHistoryClosePrices(
        [Description("開始日期 (格式: yyyy-MM-dd)")] string startDate,
        [Description("結束日期 (格式: yyyy-MM-dd)")] string endDate)
    {
        try
        {
            if (!DateTime.TryParse(startDate, out var start) || !DateTime.TryParse(endDate, out var end))
            {
                return "日期格式錯誤，請使用 yyyy-MM-dd 格式";
            }

            if (start > end)
            {
                return "開始日期不能晚於結束日期";
            }

            // 模擬查詢歷史收盤價，實際應該從 MSSQL 資料庫查詢
            var historyPrices = await SimulateHistoryClosePricesQuery(start, end);
            
            return FormatHistoryPrices(historyPrices, start, end);
        }
        catch (Exception ex)
        {
            return $"查詢歷史收盤價時發生錯誤：{ex.Message}";
        }
    }

    private async Task<decimal> SimulateLatestClosePriceQuery()
    {
        // 模擬資料庫查詢延遲
        await Task.Delay(100);
        
        // 模擬台股大盤收盤價（約在 15000-18000 之間）
        var random = new Random();
        return 16000 + (decimal)(random.NextDouble() * 2000);
    }

    private async Task<List<(DateTime Date, decimal Price)>> SimulateHistoryClosePricesQuery(DateTime start, DateTime end)
    {
        // 模擬資料庫查詢延遲
        await Task.Delay(200);
        
        var prices = new List<(DateTime, decimal)>();
        var random = new Random();
        var currentPrice = 16000m;
        
        for (var date = start; date <= end; date = date.AddDays(1))
        {
            // 跳過週末
            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
                continue;
                
            // 模擬股價波動 (-2% 到 +2%)
            var change = (decimal)(random.NextDouble() * 0.04 - 0.02);
            currentPrice = currentPrice * (1 + change);
            prices.Add((date, currentPrice));
        }
        
        return prices;
    }

    private string FormatHistoryPrices(List<(DateTime Date, decimal Price)> prices, DateTime start, DateTime end)
    {
        if (!prices.Any())
        {
            return $"查詢期間 {start:yyyy-MM-dd} 至 {end:yyyy-MM-dd} 無交易數據";
        }

        var result = $"台股大盤歷史收盤價 ({start:yyyy-MM-dd} 至 {end:yyyy-MM-dd})：\n";
        
        // 顯示最多10筆最近的數據
        var recentPrices = prices.TakeLast(10).ToList();
        
        foreach (var (date, price) in recentPrices)
        {
            result += $"• {date:yyyy-MM-dd}: {price:F2} 點\n";
        }
        
        if (prices.Count > 10)
        {
            result += $"... (共 {prices.Count} 筆交易日數據)\n";
        }
        
        // 計算期間統計
        var firstPrice = prices.First().Price;
        var lastPrice = prices.Last().Price;
        var changePercent = ((lastPrice - firstPrice) / firstPrice) * 100;
        
        result += $"\n期間統計：\n";
        result += $"• 起始價格: {firstPrice:F2} 點\n";
        result += $"• 結束價格: {lastPrice:F2} 點\n";
        result += $"• 漲跌幅: {changePercent:+0.00;-0.00}%";
        
        return result;
    }

    // 這個方法是為了未來實際連接 MSSQL 資料庫時使用
    private async Task<string> ExecuteQueryAsync(string query, params SqlParameter[] parameters)
    {
        // 實際實作時，這裡會使用 _databaseSettings.ConnectionString 連接資料庫
        // 目前先返回模擬結果
        await Task.Delay(100);
        return "模擬查詢結果";
    }
}