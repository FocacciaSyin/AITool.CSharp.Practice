# AITool.CSharp.Practice - Semantic Kernel 專案

這個專案展示如何在 .NET 8 中使用 Microsoft Semantic Kernel 與 OpenAI 服務。

## 🚀 快速開始

### 1. 安裝相依套件

專案已包含以下套件：
- `Microsoft.SemanticKernel` (v1.15.0)
- `Microsoft.Extensions.Configuration` (v9.0.8)
- `Microsoft.Extensions.Configuration.Json` (v8.0.0)
- `Microsoft.Extensions.Configuration.EnvironmentVariables` (v8.0.0)
- `Microsoft.Extensions.Logging.Console` (v9.0.7)

### 2. 設定 OpenAI API 金鑰

你可以使用以下任一方式來設定 API 金鑰：

#### 方法 1: 環境變數（推薦）

**Windows (PowerShell)**
```powershell
$env:OPENAI_API_KEY = "your_openai_api_key_here"
```

**Windows (命令提示字元)**
```cmd
set OPENAI_API_KEY=your_openai_api_key_here
```

**Linux/Mac**
```bash
export OPENAI_API_KEY=your_openai_api_key_here
```

#### 方法 2: launchSettings.json（VS/Rider 適用）

專案已包含 `Properties/launchSettings.json`，在其中更新 API 金鑰：

```json
{
  "profiles": {
    "AITool.CSharp.Practice": {
      "commandName": "Project",
      "environmentVariables": {
        "OPENAI_API_KEY": "your_openai_api_key_here",
        "DOTNET_ENVIRONMENT": "Development"
      }
    }
  }
}
```

#### 方法 3: appsettings.Development.json

修改 `appsettings.Development.json` 檔案：

```json
{
  "OpenAI": {
    "ApiKey": "your_openai_api_key_here",
    "Model": "gpt-3.5-turbo"
  }
}
```

**⚠️ 重要：此檔案已加入 `.gitignore`，不會被提交到版本控制系統。**

### 3. 執行專案

```bash
dotnet run
```

## 📁 專案結構

```
├── Program.cs              # 主程式入口點
├── KernelFactory.cs        # Semantic Kernel 工廠類別
├── Configuration.cs        # 設定管理類別
├── appsettings.json        # 基本設定檔
├── appsettings.Development.json  # 開發環境設定（包含 API 金鑰）
├── Properties/
│   └── launchSettings.json # 啟動設定（Visual Studio 使用）
└── README.md              # 專案說明文件
```

## 🔧 設定優先順序

系統會依以下順序尋找 API 金鑰：

1. **環境變數** (`OPENAI_API_KEY`)
2. **appsettings.{Environment}.json** (`OpenAI:ApiKey`)
3. **appsettings.json** (`OpenAI:ApiKey`)

## ⚙️ 自訂設定

### 變更 OpenAI 模型

在 `appsettings.json` 或 `appsettings.Development.json` 中修改：

```json
{
  "OpenAI": {
    "Model": "gpt-4"
  }
}
```

### 調整日誌層級

在 `appsettings.json` 中修改：

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning"
    }
  }
}
```

## 🛡️ 安全性注意事項

- ✅ `appsettings.Development.json` 已加入 `.gitignore`
- ✅ 不要在程式碼中硬編碼 API 金鑰
- ✅ 在生產環境中使用環境變數或安全的設定服務
- ✅ 定期輪換 API 金鑰

## 🎯 下一步

專案已準備好用於開發 Semantic Kernel 應用程式。你可以：

1. 新增 Semantic Functions
2. 建立 Native Functions
3. 實作對話管理
4. 整合向量資料庫（如 PgVector）

## 🔍 疑難排解

### 常見錯誤

**1. API 金鑰未設定**
```
請設定 OpenAI API 金鑰。可以使用以下任一方式：...
```
➡️ 按照上述方法設定 API 金鑰

**2. 套件版本衝突**
➡️ 執行 `dotnet restore` 重新還原套件

**3. 編譯錯誤**
➡️ 確認 TargetFramework 設定為 `net8.0`

### 檢查設定

執行專案時，系統會顯示：
- Semantic Kernel 版本
- API 金鑰設定狀態
- 初始化結果
