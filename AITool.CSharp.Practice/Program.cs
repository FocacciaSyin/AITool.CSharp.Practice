using AITool.CSharp.Practice;
using AITool.CSharp.Practice.Models;
using Microsoft.SemanticKernel;

Console.WriteLine("=== 配置測試 ===");

// 測試配置讀取
var config = Configuration.GetConfiguration();
var apiKeyFromConfig = config["OpenAI:ApiKey"];
var modelFromConfig = config["OpenAI:Model"];

Console.WriteLine($"OpenAI:ApiKey: {(string.IsNullOrEmpty(apiKeyFromConfig) ? "(未設定)" : $"(已設定: {apiKeyFromConfig.Substring(0, Math.Min(20, apiKeyFromConfig.Length))}...)")}");
Console.WriteLine($"OpenAI:Model: {modelFromConfig ?? "(null)"}");

// 測試 API 金鑰取得
try 
{
    var apiKey = Configuration.GetOpenAIApiKey();
    Console.WriteLine($"✅ API 金鑰已設定並可取得: {apiKey.Substring(0, Math.Min(20, apiKey.Length))}...");
    
    // 如果成功，嘗試初始化 Semantic Kernel
    Console.WriteLine();
    Console.WriteLine("正在初始化 Semantic Kernel...");
    var kernel = KernelFactory.CreateKernel();
    Console.WriteLine("✅ Semantic Kernel 初始化成功！");
    
    var chatService = kernel.GetRequiredService<Microsoft.SemanticKernel.ChatCompletion.IChatCompletionService>();
    Console.WriteLine($"聊天完成服務：{chatService.GetType().Name}");
    
    Console.WriteLine("\n🎉 專案設定完成！您可以開始使用 Semantic Kernel 了。");
}
catch (Exception ex)
{
    Console.WriteLine($"❌ 失敗: {ex.Message}");
}
