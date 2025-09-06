using System.ComponentModel;
using Microsoft.SemanticKernel;

namespace AITool.CSharp.Practice.Models.AIPlugins;

public class TimeInformation
{
    // 定義一個可被 AI 呼叫的 C# 方法
    [KernelFunction]
    [Description("取得今天的日期")]
    public string GetCurrentTime() => DateTime.UtcNow.ToString("R");
}
