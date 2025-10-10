using CSnakes.Runtime;

namespace AITool.CSharp.Practice.Samples;

/// <summary>
/// 1.3 使用 CSnakes 執行 tiktoken 計算 Token 數量的範例
/// 說明：
/// 1. 使用 CSnakes 呼叫 Python 的 tiktoken 套件
/// 2. 示範如何在 .NET 中執行 Python 代碼進行 Token 計算
/// 3. 比較不同模型的 Token 計算結果
/// </summary>
public class Sample_1_3_CSnakes_TokenCounting(IPythonEnvironment pythonEnvironment)
{
    /// <summary>
    /// 執行 Token 計算範例
    /// </summary>
    public async Task RunAsync()
    {
        Console.WriteLine("=== 1.3 使用 CSnakes + tiktoken 計算 Token 數量範例 ===\n");

        // 測試文字
        var testTexts = new[]
        {
            "Hello, world!",
            "這是一個中文句子，用來測試 Token 計算。",
            "The quick brown fox jumps over the lazy dog. This is a longer sentence to test tokenization.",
            "Semantic Kernel 是微軟開發的 AI 框架，用於構建智慧應用程式。"
        };

        // 使用不同的模型編碼
        var encodings = new[] { "cl100k_base", "p50k_base", "r50k_base" };
        var modelNames = new[] { "GPT-4", "GPT-3.5-turbo", "GPT-3" };

        // 載入我們的 Python 模組
        var tokenCounter = pythonEnvironment.TokenCounter();

        foreach (var text in testTexts)
        {
            Console.WriteLine($"文字: {text}");
            Console.WriteLine("Token 計算結果:");

            for (int i = 0; i < encodings.Length; i++)
            {
                try
                {
                    // 使用我們的 Python 函數計算 token 數量
                    var tokenCount = tokenCounter.CountTokens(text, encodings[i]);

                    Console.WriteLine($"  {modelNames[i]} ({encodings[i]}): {tokenCount} tokens");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"  {modelNames[i]} ({encodings[i]}): 錯誤 - {ex.Message}");
                }
            }

            Console.WriteLine();
        }
    }
}
