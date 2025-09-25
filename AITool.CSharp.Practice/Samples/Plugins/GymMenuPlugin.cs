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

    [KernelFunction, Description("取得所有可用的健身課程項目及其價格清單。")]
    public string GetAllCoursesWithFees()
    {
        var courseList = string.Join("\n", CourseFees.Select(kvp => $"- {kvp.Key}: NT$ {kvp.Value}"));
        return $"所有可用的健身課程及價格:\n{courseList}";
    }

    [KernelFunction, Description("查詢指定健身課程的費用。如果不確定課程名稱，請先使用 GetAllCoursesWithFees 查看所有可用課程。")]
    public string GetCourseFee([Description("健身課程名稱。")] string courseName)
    {
        if (CourseFees.TryGetValue(courseName, out var fee))
        {
            return $"{courseName} 的費用為 NT$ {fee}";
        }
        
        // 提供建議的課程名稱
        var availableCourses = string.Join(", ", CourseFees.Keys);
        return $"查無此課程「{courseName}」，請確認輸入名稱。\n可用的課程有: {availableCourses}";
    }
}
