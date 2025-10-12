using AITool.CSharp.Practice.Models.Settings;
using Microsoft.AutoGen.Contracts;
using Microsoft.AutoGen.Core;

namespace AITool.CSharp.Practice.Samples._4_AutoGen.dotnet;

/// <summary>
/// 4.0 使用 Microsoft AutoGen Core 建立基本的 Agent 對話範例
/// 
/// 此範例展示:
/// 1. 如何定義 AutoGen Agent (繼承 BaseAgent)
/// 2. 如何實作訊息處理 (IHandle<T>)
/// 3. 如何使用 Topic 訂閱機制 (TypeSubscription)
/// 4. Agent 之間的訊息傳遞流程
/// 5. 使用 AgentsAppBuilder 設定 Runtime
/// 
/// 參考: https://microsoft.github.io/autogen/dotnet/dev/core/tutorial.html
/// </summary>
public static class Sample_4_0_AutoGen_CSharp
{
    /// <summary>
    /// 執行 AutoGen 基本範例 - 簡單對話系統
    /// </summary>
    public static async Task RunAsync(OpenAISettings openAISettings)
    {
        Console.WriteLine("=".PadRight(60, '='));
        Console.WriteLine("AutoGen .NET Core 基本範例 - 簡單對話系統");
        Console.WriteLine("=".PadRight(60, '='));
        Console.WriteLine();
        Console.WriteLine("此範例展示兩個 Agent 的對話:");
        Console.WriteLine("  - User Agent: 發送問候訊息");
        Console.WriteLine("  - Assistant Agent: 接收並回應訊息");
        Console.WriteLine();

        // 建立 AgentsAppBuilder 並設定 In-Process Runtime
        var appBuilder = new AgentsAppBuilder()
            .UseInProcessRuntime(deliverToSelf: true);

        // 註冊 Assistant Agent (負責回應問候)
        appBuilder.AddAgent<AssistantAgent>("AssistantAgent");

        // 註冊 User Agent (負責發送問候)
        appBuilder.AddAgent<UserAgent>("UserAgent");

        // 建置應用程式
        var app = await appBuilder.BuildAsync();

        Console.WriteLine("✓ AutoGen Runtime 已啟動");
        Console.WriteLine("✓ AssistantAgent 已註冊");
        Console.WriteLine("✓ UserAgent 已註冊");
        Console.WriteLine();

        // 發送初始問候訊息
        Console.WriteLine("【開始對話】");
        Console.WriteLine("-".PadRight(60, '-'));
        await app.PublishMessageAsync(
            new GreetingMessage { Content = "你好,很高興認識你!" },
            new TopicId("greetings"));

        // 等待一段時間讓 Agent 處理完成
        await Task.Delay(2000);

        Console.WriteLine();
        Console.WriteLine("=".PadRight(60, '='));
        Console.WriteLine("範例執行完成!");
        Console.WriteLine("=".PadRight(60, '='));
    }
}

/// <summary>
/// 問候訊息 - 用於傳遞問候內容
/// </summary>
public class GreetingMessage
{
    public required string Content { get; set; }
}

/// <summary>
/// 回應訊息 - 用於傳遞回應內容
/// </summary>
public class ResponseMessage
{
    public required string Content { get; set; }
}

/// <summary>
/// Assistant Agent - 負責回應問候
/// 
/// 功能:
/// - 訂閱 "greetings" Topic
/// - 接收 GreetingMessage
/// - 生成友善的回應
/// - 發布 ResponseMessage
/// </summary>
[TypeSubscription("greetings")]
public class AssistantAgent : BaseAgent, IHandle<GreetingMessage>
{
    public AssistantAgent(
        AgentId id,
        IAgentRuntime runtime)
        : base(id, runtime, "Assistant Agent", null)
    {
    }

    public async ValueTask HandleAsync(GreetingMessage item, MessageContext messageContext)
    {
        Console.WriteLine($"  [Assistant] 收到訊息: {item.Content}");
        
        // 生成回應
        var response = $"你好!我是 AI 助理,很高興能為你服務!你說:「{item.Content}」";
        
        Console.WriteLine($"  [Assistant] 回應: {response}");

        // 發布回應訊息
        await PublishMessageAsync(
            new ResponseMessage { Content = response },
            topic: new TopicId("responses"));
    }
}

/// <summary>
/// User Agent - 負責接收回應
/// 
/// 功能:
/// - 訂閱 "responses" Topic
/// - 接收 ResponseMessage
/// - 顯示收到的回應
/// </summary>
[TypeSubscription("responses")]
public class UserAgent : BaseAgent, IHandle<ResponseMessage>
{
    public UserAgent(
        AgentId id,
        IAgentRuntime runtime)
        : base(id, runtime, "User Agent", null)
    {
    }

    public async ValueTask HandleAsync(ResponseMessage item, MessageContext messageContext)
    {
        Console.WriteLine($"  [User] 收到回應: {item.Content}");
        Console.WriteLine($"  [User] 對話完成!");
        
        await Task.CompletedTask;
    }
}
