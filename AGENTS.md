# Repository Guidelines

## 專案結構與模組組織
方案檔 `AITool.CSharp.Practice.sln` 載入位於 `AITool.CSharp.Practice/` 的主控台應用程式，`Program.cs` 透過 .NET Hosting 與相依性注入初始化 Semantic Kernel 範例。功能導覽集中於 `Samples/`，依 GitHub OpenAI、函式呼叫與代理外掛等能力分類；共用的 DTO、協助類別與設定繫結則位於 `Models/`。手動測試所需的 HTTP 請求保存在 `HttpClients/`，可在 Visual Studio 或 Rider 直接執行。新增測試專案時，請在主專案旁建立同層級目錄以維持解決方案整潔。

## 建置、測試與開發指令
- `dotnet restore` — 下載 NuGet 相依套件，如 `Microsoft.SemanticKernel` 與記錄延伸模組。
- `dotnet build AITool.CSharp.Practice/AITool.CSharp.Practice.csproj` — 以 `net8.0` 建置並擷取編譯警告。
- `dotnet run --project AITool.CSharp.Practice/AITool.CSharp.Practice.csproj` — 執行目前在 `Program.cs` 切換的範例情境。
- `dotnet format` — 安裝全域工具 (`dotnet tool update -g dotnet-format`) 後套用 Roslyn 格式化規則。

## 程式風格與命名約定
使用四個空白縮排，公開成員採 PascalCase，區域變數與私有欄位維持 camelCase。依功能群組註冊 DI 服務並偏好使用非同步 API。範例檔名需具描述性（例如 `Sample_2_3_SemanticKernel_FunctionCalling.cs`），以便與 README 參照對應。專案已啟用可空參考型別與隱含 using，請勿停用。

## 測試指引
目前尚未內建自動化測試。若要新增覆蓋率，請建立目標為 `net8.0` 的 `AITool.CSharp.Practice.Tests` 專案，加入 `xunit` 或 `nunit`，並註冊進方案。測試類別命名應對應被驗證的情境，並於解決方案根目錄執行 `dotnet test`。在 PR 說明中記錄手動驗證步驟，直到回歸測試完善。

## 提交與 PR 指南
提交訊息遵循 Conventional Commits（如 `feat:`、`chore:`），內容精簡並使用祈使語氣。PR 需連結相關議題，說明設定變更（尤其是祕密金鑰），並附上展示新代理行為的主控台輸出或截圖。確保變更範圍單純；若功能仍在實驗階段，請分支處理後再發 PR。

## 安全與設定提示
透過 `dotnet user-secrets` 管理 `OpenAI`、`GitHub` 與 `Gemini` 金鑰，供 `Program.cs` 進行繫結。請勿提交真實密鑰或修改 `appsettings.json`，必要時改用 `.env` 或 user secrets。新增設定結構時於本文件或 README 補充，協助團隊安全複製環境。
