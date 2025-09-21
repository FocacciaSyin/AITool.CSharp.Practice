using System.ComponentModel;
using Microsoft.SemanticKernel;

namespace AITool.CSharp.Practice.Samples.Plugins;

/// <summary>
/// 投資策略知識庫 Plugin - 提供各種技術分析策略
/// </summary>
public class InvestmentStrategyPlugin
{
    [KernelFunction, Description("取得布林帶策略分析方法和應用原則")]
    public string GetBollingerBandsStrategy()
    {
        return """
               布林帶策略 (Bollinger Bands)：
               
               📊 組成結構：
               布林帶是由三條線組成的技術指標：
               1. 中線：20日移動平均線
               2. 上軌：中線 + (2 × 標準差)
               3. 下軌：中線 - (2 × 標準差)
               
               📈 應用原則：
               - 當股價觸及上軌時，可能出現回檔壓力
               - 當股價觸及下軌時，可能出現反彈支撐
               - 布林帶收縮表示波動度降低，可能醞釀大行情
               - 布林帶擴張表示波動度增加，趨勢可能加速
               
               ⏰ 適用時機：
               - 盤整市況中的區間操作
               - 趨勢確認和轉折點判斷
               - 波動度分析和風險控制
               
               ⚠️ 注意事項：
               - 強勢趨勢中可能出現「沿軌道走」現象
               - 需搭配其他指標確認信號
               - 假突破時要注意停損
               """;
    }

    [KernelFunction, Description("取得移動平均線策略分析方法")]
    public string GetMovingAverageStrategy()
    {
        return """
               移動平均線策略 (Moving Average)：
               
               📊 常用組合：
               移動平均線是技術分析的基礎工具：
               1. 短期：5日、10日移動平均線
               2. 中期：20日、60日移動平均線
               3. 長期：120日、240日移動平均線
               
               🎯 黃金交叉與死亡交叉：
               - 黃金交叉：短期均線向上突破長期均線，買進信號
               - 死亡交叉：短期均線向下跌破長期均線，賣出信號
               
               📈 多頭排列與空頭排列：
               - 多頭排列：短 > 中 > 長期均線，趨勢向上
               - 空頭排列：短 < 中 < 長期均線，趨勢向下
               
               🛡️ 支撐與壓力：
               - 移動平均線可作為動態支撐或壓力線
               - 量價配合度是成功率的關鍵因素
               - 均線密集處往往形成重要關卡
               
               💡 實戰技巧：
               - 均線糾結後往往有大行情
               - 注意均線的斜率變化
               - 結合成交量確認趨勢強度
               """;
    }

    [KernelFunction, Description("取得RSI相對強弱指標策略")]
    public string GetRsiStrategy()
    {
        return """
               RSI 相對強弱指標策略：
               
               📊 指標說明：
               RSI (Relative Strength Index) 是衡量股價動能的震盪指標
               
               🧮 計算方式：
               RSI = 100 - (100 / (1 + RS))
               RS = 平均上漲幅度 / 平均下跌幅度
               
               📈 應用原則：
               - RSI > 70：過買區域，注意回檔風險
               - RSI < 30：過賣區域，注意反彈機會
               - RSI 50：多空分界線
               - RSI 80/20：極端過買過賣區域
               
               🔍 進階應用：
               - 背離現象：價格創新高但 RSI 未創新高（頂背離）
               - 價格創新低但 RSI 未創新低（底背離）
               - RSI 突破下降趨勢線：反轉信號
               
               ⚠️ 使用注意：
               - 強勢趨勢中 RSI 可能長期維持在高檔
               - 結合其他指標使用效果更佳
               - 注意背離信號的確認
               """;
    }

    [KernelFunction, Description("取得市場情緒分析策略")]
    public string GetMarketSentimentStrategy()
    {
        return """
               市場情緒分析策略：
               
               😰😤 恐懼與貪婪指標：
               - 極度恐懼（0-25）：通常是買進機會
               - 恐懼（26-45）：可考慮分批進場
               - 中性（46-54）：觀望為主
               - 貪婪（55-75）：注意風險控制
               - 極度貪婪（76-100）：通常是減碼時機
               
               📊 成交量分析：
               - 價漲量增：健康上漲趨勢，可持續關注
               - 價漲量縮：上漲力道不足，注意轉折
               - 價跌量增：賣壓沉重，下跌可能持續
               - 價跌量縮：下跌力道減弱，可能築底
               
               💰 資金流向分析：
               - 外資買賣超：國際資金動向
               - 投信買賣超：法人機構態度
               - 融資融券餘額：散戶情緒指標
               - 大戶持股比例：主力動向
               
               🌍 總體經濟指標：
               - 央行利率政策：資金成本影響
               - 通膨預期：實質報酬率考量
               - GDP 成長率：經濟基本面
               - 國際情勢：風險偏好變化
               
               💡 綜合判斷：
               - 多重指標交叉驗證
               - 注意市場主流情緒
               - 逆向思考的重要性
               """;
    }

    [KernelFunction, Description("根據關鍵字搜尋相關投資策略")]
    public string SearchStrategy([Description("搜尋關鍵字，如：布林帶、均線、RSI、情緒等")] string keyword)
    {
        var lowerKeyword = keyword.ToLowerInvariant();
        
        return lowerKeyword switch
        {
            var k when k.Contains("布林") || k.Contains("bollinger") => GetBollingerBandsStrategy(),
            var k when k.Contains("均線") || k.Contains("移動平均") || k.Contains("ma") || k.Contains("moving") => GetMovingAverageStrategy(),
            var k when k.Contains("rsi") || k.Contains("相對強弱") => GetRsiStrategy(),
            var k when k.Contains("情緒") || k.Contains("恐懼") || k.Contains("貪婪") || k.Contains("sentiment") => GetMarketSentimentStrategy(),
            _ => """
                 📚 可用的投資策略包括：
                 
                 1. 布林帶策略 (Bollinger Bands) - 波動度分析
                 2. 移動平均線策略 (Moving Average) - 趨勢分析  
                 3. RSI 相對強弱指標 - 動能分析
                 4. 市場情緒分析 - 籌碼面分析
                 
                 請使用更具體的關鍵字搜尋，或直接詢問特定策略。
                 """
        };
    }

    [KernelFunction, Description("取得技術分析的基本原則和風險提醒")]
    public string GetTechnicalAnalysisPrinciples()
    {
        return """
               📊 技術分析基本原則：
               
               🎯 三大假設：
               1. 市場行為包含一切資訊
               2. 價格以趨勢方式演變
               3. 歷史會重演
               
               📈 分析層次：
               1. 趨勢分析：主要趨勢、次要趨勢、短期波動
               2. 型態分析：頭部、底部、整理型態
               3. 指標分析：超買超賣、動能、量價關係
               
               ⚠️ 風險提醒：
               - 技術分析非萬能，有失效可能
               - 需要搭配基本面分析
               - 風險控制永遠是第一考量
               - 不同市況適用不同策略
               - 避免過度依賴單一指標
               
               💡 實戰建議：
               - 多重時間框架分析
               - 注意關鍵支撐壓力位
               - 量價配合確認信號
               - 設定停損保護資金
               - 保持客觀理性分析
               """;
    }
}