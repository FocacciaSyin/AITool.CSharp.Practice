using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace AITool.CSharp.Practice.Models.Reducer;

public class RecursiveSummarizingChatReducer:IChatHistoryReducer
{
    private readonly int _maxTokens;
    private readonly Kernel _kernel;
    private readonly IChatCompletionService _chatService;

    public RecursiveSummarizingChatReducer(int maxTokens, Kernel kernel, IChatCompletionService chatService)
    {
        _maxTokens = maxTokens;
        _kernel = kernel;
        _chatService = chatService;
    }

    public async Task<ChatHistory> ReduceAsync(ChatHistory history, CancellationToken ct = default)
    {
        // 遞迴檢查長度
        while (GetApproxLength(history) > _maxTokens)
        {
            // 取出最舊的一批訊息（例如前 10 筆）
            var oldMessages = history.Take(10).ToList();
            if (!oldMessages.Any()) break;

            var textToSummarize = string.Join("\n", oldMessages.Select(m => $"{m.Role}: {m.Content}"));

            // 呼叫 LLM 生成摘要
            var chatHistory = new ChatHistory();
            chatHistory.AddSystemMessage("請將以下對話壓縮成摘要，保留關鍵需求、決策與結論，限制 150 字內：");
            chatHistory.AddUserMessage(textToSummarize);
            
            var summary = await _chatService.GetChatMessageContentAsync(
                chatHistory,
                executionSettings: null,
                kernel: _kernel,
                cancellationToken: ct
            );

            // 移除舊訊息
            foreach (var msg in oldMessages)
            {
                history.Remove(msg);
            }

            // 新增摘要（標記為系統訊息，避免和用戶/助理混淆）
            history.Insert(0, new ChatMessageContent(
                AuthorRole.System,
                $"[過往對話摘要]: {summary.Content}"
            ));
        }

        return history;
    }

    /// <summary>
    /// 這裡用字數估算，可換成 tokenizer
    /// </summary>
    /// <param name="history"></param>
    /// <returns></returns>
    private int GetApproxLength(ChatHistory history)
    {
        return history.Sum(m => m.Content?.Length ?? 0);
    }

    public async Task<IEnumerable<ChatMessageContent>?> ReduceAsync(IReadOnlyList<ChatMessageContent> chatHistory, CancellationToken cancellationToken = new ())
    {
        var history = new ChatHistory(chatHistory);
        var reduced = await ReduceAsync(history, cancellationToken);
        return reduced;
    }
}
