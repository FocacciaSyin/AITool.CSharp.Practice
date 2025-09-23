# Copilot Coding Agent Instructions for AITool.CSharp.Practice

## 專案架構與核心原則
- 本專案為 .NET 8 主控台應用，聚焦於 Microsoft Semantic Kernel、OpenAI、Gemini 等 AI 能力整合。
- 主要進入點為 `AITool.CSharp.Practice/Program.cs`，所有範例與功能導覽集中於 `Samples/` 目錄，依功能分類（如 Function Calling、Agent Plugins）。
- 共用 DTO、協助類別、設定繫結等集中於 `Models/`，請勿將業務邏輯分散於多處。
- 手動 HTTP 測試腳本存放於 `HttpClients/`，可直接於 IDE 執行。

## 開發與建置流程
- 邏輯分層明確：範例（`Samples/`）僅示範單一情境，勿混用多重責任。
- 建置：`dotnet build AITool.CSharp.Practice/AITool.CSharp.Practice.csproj`（預設 net8.0）。
- 執行：`dotnet run --project AITool.CSharp.Practice/AITool.CSharp.Practice.csproj`，會根據 `Program.cs` 切換範例。
- 格式化：`dotnet format`（需先安裝 dotnet-format 全域工具）。
- 測試：如需自動化測試，請於專案根目錄旁建立 `AITool.CSharp.Practice.Tests`，目標 net8.0，使用 xunit。

## 命名與程式風格
- 四空白縮排，公開成員 PascalCase，私有欄位與區域變數 camelCase。
- 範例檔名需具描述性（如 `Sample_2_3_SemanticKernel_FunctionCalling.cs`），方便 README 對應。
- DI 註冊依功能群組，偏好非同步 API。
- 已啟用可空參考型別與隱含 using，請勿停用。

## 整合與設定
- 敏感金鑰（OpenAI、GitHub、Gemini）請用 `dotnet user-secrets` 管理，勿提交至版本庫。
- 設定檔如需擴充，請同步於 `AGENTS.md` 或 README 補充。
- 外部依賴如 PgVector，請參考 `README.md` 之容器啟動與連線範例。

## PR 與提交規範
- Commit 訊息遵循 Conventional Commits（如 `feat:`、`chore:`），內容精簡。
- PR 需連結議題，說明設定變更與代理行為，附主控台輸出或截圖。
- 功能實驗請分支處理，確保主分支穩定。

## 參考檔案
- `AGENTS.md`：完整開發規範與安全建議
- `Samples/`：各情境範例
- `Models/`：共用資料結構與協助類
- `HttpClients/`：API 測試腳本
- `README.md`、`AITool.CSharp.Practice/README.md`：快速入門、外部依賴說明

---
如遇不明確規範，請優先參考 `AGENTS.md` 並於 PR 說明中主動記錄決策依據。