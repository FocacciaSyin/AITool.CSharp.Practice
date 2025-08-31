# 學習使用

使用 Semantic kernal & [Microsoft Kernel Memory](https://github.com/microsoft/kernel-memory/tree/main) C# SDK 來建立智慧應用程式
先從基本範例開始建立，逐步加入更多功能

# 需求

1. 使用 Semantic kernal 建立 OpenAI Chat Completion
2. 使用 function Calling 來呼叫外部 API
   2.1 串接 fugle API 來取得股市資料
   2.2 取得個股資料
   2.3 使用者可以使用自然語言來查詢個股資料
3. 使用 PgVector 來儲存向量資料
4. 使用 PgVector 來記錄
   4.1 使用者輸入文字
   4.2 回傳內容
   4.3 system prompt
   4.4 function prompt
   4.5 評分(未來會做為強化學習的依據)

---

# 參考文章

https://ithelp.ithome.com.tw/articles/10367471

https://github.com/andrew0928/AndrewDemo.DevAIAPPs

---

# PgVector 設置與使用

## 啟動 PgVector 容器

```bash
# 使用 Podman 運行 PgVector 容器
podman run --name pgvector-db -e POSTGRES_PASSWORD=postgres -e POSTGRES_DB=vectordb -p 5432:5432 -d pgvector/pgvector:pg16

# 檢查容器狀態
podman ps

# 查看日誌
podman logs pgvector-db

# 停止容器
podman stop pgvector-db

# 重啟容器
podman start pgvector-db
```

## C# 連接範例

```csharp
using Npgsql;
using NpgsqlTypes;

string connectionString = "Host=localhost;Port=5432;Database=vectordb;Username=postgres;Password=postgres";
using var connection = new NpgsqlConnection(connectionString);
connection.Open();

// 啟用 pgvector 擴展
using var cmd = new NpgsqlCommand("CREATE EXTENSION IF NOT EXISTS vector", connection);
cmd.ExecuteNonQuery();
```
