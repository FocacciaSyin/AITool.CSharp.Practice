# Semantic Kernel 進階學習藍圖

> 已具備 README 中的基礎練習（Chat Completion、Function Calling、PgVector）。
> 以下規劃聚焦進階主題與 MCP Context7 延伸資源，協助深化與整合能力。

## Day 0：現況盤點
- 快速複習 README 的既有樣本與程式架構，標記已完成與待補強的情境。
- 建立筆記：記錄當前使用的模型設定、Fugle API 參數與 PgVector schema。
- Context7 搜尋關鍵字：`semantic-kernel overview`、`hosting configuration`，確保對最新 API 版本差異有共識。

## Week 1：語意工作流程強化
- **Prompt 與上下文管理**：研究 Context7「Prompt templates」「ChatHistory」章節，為既有樣本加入系統化 Prompt 檔與多輪對話記錄策略。
- **Streaming 與 Content 封裝**：閱讀「Agent messaging」「StreamingKernelContent」文件，嘗試改寫 Console 範例支援串流回應與結構化輸出。
- **成果**：新增進階聊天樣本（`Sample_2_0_1_SemanticKernel_ChatCompletion_Streaming.cs`），並在 README 記錄差異與測試步驟。

## Week 1-2：Function Orchestration 與 Plugin 工程化
- **Planner 與決策流程**：參考 Context7「Stepwise planner」「Action planner」，比較選用情境，調整 Fugle API 案例為自動選擇指令模式。
- **Kernel Plugin 模組化**：整理 `Samples/FunctionCalling/`，將關聯功能抽成 Plugin 專案或資料夾，善用 `KernelPluginFactory`。
- **外部服務擴充**：挑選另一個 API（如新聞、財經指標）示範 Function Call 擴充與 DI 設計。
- **成果**：撰寫 `Sample_2_4_MultiSourceFunctionCalling.cs`，展示 Planner 決策與多 API 整合。

## Week 2：記憶與 RAG 升級
- **Kernel Memory 整合**：依 README 連結研究 Kernel Memory SDK，建立基本記憶管線（匯入、索引、召回）。
- **PgVector 進階操作**：閱讀 Context7「Vector store connectors」「Text search pagination」，實作批次 upsert、metadata 過濾與 Top/Skip 分頁。
- **混合檢索策略**：比較直接 Function Call 與向量記憶的互補性，設計 fallback 機制。
- **成果**：完成 `Sample_4_3_HybridMemoryAgent.cs`，記錄記憶結構與測試輸出。

## Week 3：Agent 與多代理協作
- **Agent Abstraction**：研究 Context7「Agent architecture」「Agent base classes」，嘗試以 YAML/JSON 宣告式設定建 Agent。
- **任務分工**：設計多代理情境（如「資料整理代理」「分析代理」），使用 `KernelProcess` 或 `AgentGroup` 協調任務。
- **監控與訊息追蹤**：導入 ChatHistory 保存與訊息標註，確保可追蹤決策過程。
- **成果**：新增 `Samples/Agents/Sample_5_1_MultiAgentWorkflow.cs`，示範跨代理協作。

## Week 4：品質、測試與營運化
- **測試策略**：依專案指南建立 `AITool.CSharp.Practice.Tests`，撰寫 Fugle Client、Planner 邏輯、向量檢索的單元或整合測試。
- **評估與監控**：參考 Context7「Evaluation」「Telemetry」章節，導入 logging、追蹤 request-id，並評估 LLM 回應品質。
- **部署準備**：撰寫操作文檔，描述 user-secrets 管理、環境變數與 PgVector 容器操作；規劃 CI 任務（`dotnet build`、`dotnet test`）。
- **成果**：形成一份進階 README 或 Wiki 條目，彙整測試、部署與監控實務。

## 持續深化（Long-term）
- 追蹤 Semantic Kernel 更新：訂閱官方 release note，定期檢視 API 變更。
- 探索自訂模型與服務：測試 Azure OpenAI、Azure AI Search、或本地模型的接入模式。
- 與團隊共享：定期分享學習成果，收集使用回饋，調整樣本與文件。

## 建議的下一步
1. 依 Day 0 任務整理現況與筆記，確定既有樣本的覆蓋範圍。
2. 從 Week 1 主題開始，先強化 Prompt 與聊天流程，再逐步進入 Planner、記憶與 Agent 領域。
