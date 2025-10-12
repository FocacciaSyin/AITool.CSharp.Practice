using AITool.CSharp.Practice.Models.Settings;
using CSnakes.Runtime;

namespace AITool.CSharp.Practice.Samples._4_AutoGen;

public class Sample_4_1_CSnake_AutoGen
{
    public static async Task RunAsync(IPythonEnvironment pythonEnvironment, OpenAISettings openAISettings)
    {
        var autogenAgentChat = pythonEnvironment.MyAutogenAgentChat();
        await autogenAgentChat.MyAgentChat(openAISettings.ApiKey, openAISettings.Model);
    }
}
