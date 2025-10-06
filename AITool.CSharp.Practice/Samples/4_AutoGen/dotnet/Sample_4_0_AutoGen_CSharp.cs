using Google.Protobuf;
using Microsoft.AutoGen.AgentChat.Abstractions;
using Microsoft.AutoGen.Contracts;
using Microsoft.AutoGen.Core;

namespace AITool.CSharp.Practice.Samples._4_AutoGen;

public class Sample_4_0_AutoGen_CSharp
{
    public async Task ExecuteAsync()
    {
    }
}

public class Modifier(AgentId id, IAgentRuntime runtime) : BaseAgent(id, runtime, "MyAgent", null)
{
}

public class CountMessage
{
    public int Content { get; set; }
}

public class CountUpdate
{
    public int NewCount { get; set; }
}
