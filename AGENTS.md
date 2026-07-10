# AGENTS.md

## 專案邊界

- Solution 只有 `AITool.CSharp.Practice/AITool.CSharp.Practice.csproj`：這是 `net8.0` 主控台程式；根目錄的空白 `Program.cs` 不會編譯，真正入口是 `AITool.CSharp.Practice/Program.cs`。
- 可執行範例以入口內的 `allSamples` 為準。程式不解析 sample 名稱 CLI 參數，而是啟動互動選單；新增範例時需同時在 `Samples/` 實作並註冊到此清單。
- `Samples/` 放單一情境範例；`Models/` 放 settings、共用 reducer/plugin/helper/service；`Infrastructure/` 放 Langfuse/OpenTelemetry DI；`HttpClients/` 是 IDE 手動 HTTP 測試。
- README 含尚未接上入口的規劃內容。不要只依 README 宣稱功能可用，先確認 csproj、實際檔案及 `allSamples`。

## 驗證與執行

- `global.json` 要求 SDK `9.0.0`，但 `rollForward: latestMajor` 允許使用更高 major；目標框架仍是 `net8.0`。
- 乾淨環境依序執行：`dotnet restore AITool.CSharp.Practice/AITool.CSharp.Practice.csproj`，再執行 `dotnet build AITool.CSharp.Practice/AITool.CSharp.Practice.csproj --no-restore`。
- 目前沒有 test project、CI workflow、lint 或 formatter 設定；修改 C# 後最低驗證是上述 build，不要聲稱有自動測試可跑。
- 執行時工作目錄必須是 `AITool.CSharp.Practice/`，再執行 `dotnet run`。入口用目前目錄尋找 `appsettings.json`、`Python/` 與 `Python/.venv`；從 repo root 只加 `--project` 會改變行為甚至在選單前失敗。
- 啟動後輸入選單編號執行範例，輸入 `exit` 結束；CLI 尾端參數不會選取範例。

## Python 與設定

- C# 的 CSnakes 執行期固定使用 `AITool.CSharp.Practice/Python/.venv` 與該目錄的 `requirements.txt`。根目錄 `pyproject.toml`、`uv.lock`、`.venv` 是另一套 Python 工作區，不能取代它。
- 需重建 CSnakes 環境時，在 `AITool.CSharp.Practice/Python/` 執行 `uv venv --seed .venv`，再執行 `uv pip install --python .venv -r requirements.txt --prerelease=allow`。
- `token_counter.py`、`my_autogen_agent_chat.py`、`agent_chat_with_dev_ui.py` 由 csproj 當作 Python AdditionalFiles；改模組名或函式簽章時，同步檢查產生的 C# 呼叫如 `TokenCounter()`。
- API keys 使用該 csproj 的 User Secrets：`OpenAI:ApiKey`、`GitHub:ApiKey`、`Gemini:ApiKey`、`Langfuse:PublicKey`、`Langfuse:SecretKey`。不要把真實值寫進 tracked `appsettings.json`、README 或 `.http` 檔。
- `AddLangfuseOpenTelemetry()` 每次啟動都會初始化 `appsettings.json` 指定的 Langfuse exporter，並啟用 Semantic Kernel sensitive telemetry；prompt/response 可能輸出至 console 並送往 Langfuse，勿以私密資料測試。

## 程式慣例

- 使用 C#；識別字使用英文，新增註解使用繁體中文。
- 保持 csproj 已啟用的 nullable reference types 與 implicit usings；每個 sample 維持單一示範目的與非同步 `RunAsync` 入口。
