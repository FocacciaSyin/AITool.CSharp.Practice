using AITool.CSharp.Practice.Models;
using Microsoft.SemanticKernel.ChatCompletion;

Console.WriteLine("=== 配置測試 ===");

// 測試 API 金鑰取得
try
{
    Console.WriteLine("正在初始化 Semantic Kernel...");
    var kernel = KernelFactory.CreateKernel();
    Console.WriteLine("✅ Semantic Kernel 初始化成功！");

    var chatService = kernel.GetRequiredService<IChatCompletionService>();
    Console.WriteLine($"聊天完成服務：{chatService.GetType().Name}");

    Console.WriteLine("\n🎉 專案設定完成！您可以開始使用 Semantic Kernel 了。");
}
catch (Exception ex)
{
    Console.WriteLine($"❌ 失敗: {ex.Message}");
}
