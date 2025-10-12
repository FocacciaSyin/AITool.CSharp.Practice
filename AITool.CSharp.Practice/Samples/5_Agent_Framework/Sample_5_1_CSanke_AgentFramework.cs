using AITool.CSharp.Practice.Models.Settings;
using CSnakes.Runtime;

namespace AITool.CSharp.Practice.Samples._5_Agent_Framework;

public class Sample_5_1_CSanke_AgentFramework
{
    public static async Task RunAsync(IPythonEnvironment pythonEnvironment, OpenAISettings openAISettings)
    {
        //要使用 CSnake 啟動 DevUI還是有點困難，是preview版，等dotnet 出 devUI再說
        var agentChatWithDevUi = pythonEnvironment.AgentChatWithDevUi();
        agentChatWithDevUi.ChatAgent(openAISettings.ApiKey);
    }
}
