# 🚀 混合式股票顧問 Agent

結合 MSSQL 數據存取與 RAG 投資策略知識庫的智能台股大盤分析系統。

## 📌 功能特色

### 🤖 智能股票顧問
- **專業角色**：台股大盤分析師和投資顧問
- **專業限制**：只討論台股大盤，不提供個股建議
- **分析方式**：結合即時數據 + 投資策略知識
- **風險提醒**：所有建議僅供參考，不構成投資建議

### 📊 數據來源整合
- **MSSQL Plugin**：查詢最新/歷史台股大盤收盤價
- **RAG 策略庫**：布林帶、移動平均線、RSI、市場情緒分析
- **混合式分析**：數據驅動 + 策略知識的專業建議

### 🛠️ 技術架構
- **.NET 8** + **Semantic Kernel** 
- **OpenAI API** 語言模型整合
- **MSSQL** 股價數據存取
- **Plugin 架構** 模組化功能擴展

## 🚀 快速開始

### 1. 環境需求

- .NET 8 SDK
- OpenAI API Key
- MSSQL Server (可選，目前使用模擬數據)
- Docker (可選，用於 Qdrant)

### 2. 設定 API Key

透過 Secret Manager 設定 OpenAI API 金鑰：

```bash
cd AITool.CSharp.Practice
dotnet user-secrets set "OpenAI:ApiKey" "your-openai-api-key"
```

### 3. 執行應用程式

```bash
cd AITool.CSharp.Practice
dotnet run
```

### 4. 可選：啟動 Qdrant 向量資料庫

```bash
# 使用 Docker Compose
docker-compose -f docker-compose.qdrant.yml up -d

# 或使用 Podman/Docker
podman run -d -p 6333:6333 qdrant/qdrant
```

## 💬 使用範例

### 範例對話

**用戶提問**：「最近大盤走勢如何？」

**系統回應**：
1. 🔍 自動呼叫 `StockData.GetLatestClosePrice` 查詢最新收盤價
2. 📈 使用 `InvestmentStrategy.GetMovingAverageStrategy` 取得技術分析
3. 📋 綜合數據與策略提供專業分析

**用戶提問**：「依照策略現在適合買入嗎？」

**系統回應**：
1. 📊 查詢近期歷史價格數據
2. 🧠 整合多種投資策略分析
3. ⚠️ 提供客觀建議並附風險提醒

## 🔧 架構設計

### Plugin 架構

```
🏗️ Semantic Kernel
├── 📊 StockDbPlugin
│   ├── GetLatestClosePrice()      # 最新收盤價
│   └── GetHistoryClosePrices()    # 歷史收盤價
└── 🧠 InvestmentStrategyPlugin
    ├── GetBollingerBandsStrategy()    # 布林帶策略
    ├── GetMovingAverageStrategy()     # 移動平均線
    ├── GetRsiStrategy()               # RSI 指標
    ├── GetMarketSentimentStrategy()   # 市場情緒
    └── SearchStrategy()               # 策略搜尋
```

### 系統流程

```
用戶提問 → Agent 分析 → 調用相關 Plugins → 整合結果 → 專業回覆
    ↓              ↓              ↓              ↓         ↓
輸入處理 → 意圖識別 → 數據查詢+策略檢索 → 結果合成 → 風險提醒
```

## 📊 支援的分析策略

### 1. 布林帶策略 (Bollinger Bands)
- **應用**：波動度分析、區間操作
- **信號**：上軌壓力、下軌支撐
- **特點**：適合盤整市況

### 2. 移動平均線策略 (Moving Average)
- **應用**：趨勢分析、交叉信號
- **信號**：黃金交叉、死亡交叉
- **特點**：多空趨勢判斷

### 3. RSI 相對強弱指標
- **應用**：動能分析、超買超賣
- **信號**：70以上過買、30以下過賣
- **特點**：震盪指標配合使用

### 4. 市場情緒分析
- **應用**：籌碼面分析、資金流向
- **信號**：恐懼貪婪指標、成交量
- **特點**：總體經濟配合分析

## ⚙️ 配置設定

### appsettings.json

```json
{
  "OpenAI": {
    "ApiKey": "透過 Secret Manager 設定",
    "Model": "gpt-4.1-nano"
  },
  "Database": {
    "ConnectionString": "Data Source=localhost;Initial Catalog=StockDB;Integrated Security=true;TrustServerCertificate=true;"
  },
  "Qdrant": {
    "Host": "localhost",
    "Port": 6333,
    "UseHttps": false,
    "CollectionName": "investment-strategy"
  }
}
```

## 🔒 安全與隱私

- **API Key 管理**：使用 Secret Manager 避免金鑰外洩
- **模擬數據**：目前使用模擬股價數據，避免實際資料風險
- **投資警語**：所有回覆均包含風險提醒聲明

## 🚧 未來擴展

### Phase 2: 真實數據整合
- [ ] 串接實際股價 API (如 Fugle API)
- [ ] MSSQL 資料庫實際連接
- [ ] 即時數據更新機制

### Phase 3: 進階功能
- [ ] Qdrant 向量資料庫完整整合
- [ ] PDF/Markdown 文件向量化
- [ ] 更多技術指標策略
- [ ] 歷史回測功能

### Phase 4: 使用者介面
- [ ] Web UI 介面
- [ ] 圖表視覺化
- [ ] 互動式分析

## ⚠️ 重要聲明

本系統提供的所有分析與建議僅供教育和研究目的，不構成任何投資建議。投資有風險，請謹慎評估自身風險承受能力，並諮詢專業投資顧問。

## 📚 相關資源

- [Microsoft Semantic Kernel 官方文件](https://learn.microsoft.com/en-us/semantic-kernel/)
- [OpenAI API 文件](https://platform.openai.com/docs)
- [Qdrant 向量資料庫](https://qdrant.tech/)
- [台灣證券交易所](https://www.twse.com.tw/)

---

*開發團隊：AITool.CSharp.Practice*  
*最後更新：2024年*