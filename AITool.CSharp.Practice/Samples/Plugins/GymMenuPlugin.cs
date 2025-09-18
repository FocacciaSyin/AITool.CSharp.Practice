using System.ComponentModel;
using Microsoft.SemanticKernel;

namespace AITool.CSharp.Practice.Samples.Plugins;

public class GymMenuPlugin
{
    private static readonly Dictionary<string, string> CourseFees = new(StringComparer.OrdinalIgnoreCase)
    {
        { "一小時自由使用", "50" },
        { "兩小時自由使用", "100" },
        { "三小時以上自由使用", "150" },
        { "年費", "10000" },
        { "HIIT", "600" },
        { "5K慢跑基礎課程", "600" },
        { "重量訓練", "1000" },
        { "飛輪", "500" }
    };

    [KernelFunction, Description("提供今日的健身課程推薦。")]
    public string GetWorkoutSpecials() =>
        """
        今日推薦教練課程:
        - 重量訓練: 全身肌群訓練
        - 有氧課程: 高強度間歇訓練 (HIIT)
        - 有氧運動: 5K慢跑基礎課程
        """;

    [KernelFunction, Description("提供指定健身課程的費用。")]
    public string GetCourseFee([Description("健身課程名稱。")] string courseName)
    {
        return CourseFees.TryGetValue(courseName, out var fee) ? fee : "查無此課程，請確認輸入名稱。";
    }
}
