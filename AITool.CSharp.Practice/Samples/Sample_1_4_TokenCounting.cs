using Microsoft.ML.Tokenizers;

namespace AITool.CSharp.Practice.Samples;

/// <summary>
/// 1.4 使用 Microsoft.ML.Tokenizers 計算 Token 數量的範例
/// 說明：
/// 1. 使用 Semantic Kernel 官方推薦的 Microsoft.ML.Tokenizers 套件
/// 2. 示範如何計算文字的 Token 數量
/// 3. 比較不同模型的 Token 計算結果
/// </summary>
public class Sample_1_4_TokenCounting
{
    /// <summary>
    /// 執行 Token 計算範例
    /// </summary>
    public async Task ExecuteAsync()
    {
        Console.WriteLine("=== 1.4 Token 計算範例 ===\n");

        // 測試文字
        var testTexts = new[]
        {
            "Hello, world!",
            "這是一個中文句子，用來測試 Token 計算。",
            "The quick brown fox jumps over the lazy dog. This is a longer sentence to test tokenization.",
            "Semantic Kernel 是微軟開發的 AI 框架，用於構建智慧應用程式。"
        };

        // 使用不同的 Tokenizer
        var tokenizers = new Dictionary<string, Tokenizer>
        {
            ["GPT-4.1-nano"] = TiktokenTokenizer.CreateForEncoding("o200k_base"), // GPT-4.1 系列預計使用 o200k_base
            ["GPT-4"] = TiktokenTokenizer.CreateForModel("gpt-4"),
            ["GPT-4o"] = TiktokenTokenizer.CreateForModel("gpt-4o")
        };

        foreach (var text in testTexts)
        {
            Console.WriteLine($"文字: {text}");
            Console.WriteLine("Token 計算結果:");

            foreach (var (model, tokenizer) in tokenizers)
            {
                try
                {
                    var tokenCount = tokenizer.CountTokens(text);
                    Console.WriteLine($"  {model}: {tokenCount} tokens");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"  {model}: 錯誤 - {ex.Message}");
                }
            }
        }
    }
}
