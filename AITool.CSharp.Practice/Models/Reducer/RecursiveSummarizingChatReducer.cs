using System.Text;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace AITool.CSharp.Practice.Models.Reducer;

/// <summary>
/// 遞迴式聊天紀錄摘要壓縮器 (Recursive Summarizing Chat Reducer)
/// 功能：
/// 1. 當聊天紀錄總長度 (簡單以字元數估算) 超過 _maxTokens 時，
///    會取出最舊一批訊息 (預設 10 則，含使用者/助理) 生成摘要後移除原訊息。
/// 2. 摘要會以 System 訊息插入，保留語意但降低上下文長度。
/// 3. 可多次迭代直到長度低於門檻，以節省 Token / 增加後續可用上下文容量。
/// TODO：如需更精準可改為使用實際 Tokenizer (例如 tiktoken)。
/// </summary>
public class RecursiveSummarizingChatReducer : IChatHistoryReducer
{
    private readonly int _maxTokens;                 // 允許的最大字元（簡易估算）
    private readonly Kernel _kernel;                 // 供模型執行時可用的 Kernel (若有需要使用函式可擴充)
    private readonly IChatCompletionService _chatService; // LLM 聊天服務

    public RecursiveSummarizingChatReducer(int maxTokens, Kernel kernel, IChatCompletionService chatService)
    {
        _maxTokens = maxTokens;
        _kernel = kernel;
        _chatService = chatService;
    }

    /// <summary>
    /// 主流程：檢查並壓縮聊天紀錄
    /// </summary>
    public async Task<ChatHistory> ReduceAsync(ChatHistory history, CancellationToken ct = default)
    {
        // 迴圈：直到長度低於門檻才停止
        while (GetApproxLength(history) > _maxTokens)
        {
            // 取出最舊一批訊息（此處簡化：固定 10 則；可改成參數化）
            var oldMessages = history.Take(10).ToList();
            if (!oldMessages.Any()) break; // 保護性結束

            // 將要摘要的訊息組裝為純文字（也可改為更結構化格式）
            var textToSummarize = string.Join("\n", oldMessages.Select(m => $"{m.Role}: {m.Content}"));

            // 建立摘要提示詞（使用 string overload 以避免多載呼叫混淆）
            var prompt = new StringBuilder();
            prompt.AppendLine("請將以下對話壓縮成『摘要』，需：");
            prompt.AppendLine("- 保留關鍵需求 / 決策 / 結論");
            prompt.AppendLine("- 不需要逐字稿");
            prompt.AppendLine("- 控制在 150 字以內");
            prompt.AppendLine("=== 對話開始 ===");
            prompt.AppendLine(textToSummarize);
            prompt.AppendLine("=== 對話結束 ===");

            var summary = await _chatService.GetChatMessageContentAsync(
                prompt.ToString(),
                /* PromptExecutionSettings */ null,
                _kernel,
                ct
            );

            // 移除原始訊息（減少長度）
            foreach (var msg in oldMessages)
            {
                history.Remove(msg);
            }

            // 插入摘要（放在最前面，代表被壓縮的舊歷史）
            history.Insert(0, new ChatMessageContent(
                AuthorRole.System,
                $"[過往對話摘要] {summary.Content}"));
        }

        return history;
    }

    /// <summary>
    /// 簡單估算長度（以字元數計）— 可未來替換為 Token 計算以控制更準確。
    /// </summary>
    private int GetApproxLength(ChatHistory history)
        => history.Sum(m => m.Content?.Length ?? 0);

    /// <summary>
    /// IChatHistoryReducer 介面的另一個多載：輸入為唯讀訊息列表
    /// 轉成 ChatHistory 後重用主要 Reduce 邏輯。
    /// </summary>
    public async Task<IEnumerable<ChatMessageContent>?> ReduceAsync(IReadOnlyList<ChatMessageContent> chatHistory, CancellationToken cancellationToken = new())
    {
        var history = new ChatHistory();
        foreach (var m in chatHistory)
        {
            history.Add(m);
        }
        var reduced = await ReduceAsync(history, cancellationToken);
        return reduced; // ChatHistory 已實作 IEnumerable<ChatMessageContent>
    }
}
