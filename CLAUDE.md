# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## 專案概述

這是一個 .NET 8 控制台應用程式，展示 Microsoft Semantic Kernel 與各種 AI 服務（OpenAI、GitHub Models、Gemini）的整合。此專案作為 AI/ML 概念的學習平台，包括聊天補全、函式呼叫、代理程式和可觀察性。

## 建置與執行指令

```bash
# 建置專案
dotnet build

# 執行主應用程式（互動式範例選擇）
dotnet run --project AITool.CSharp.Practice

# 還原相依性
dotnet restore
```

## 設定

專案使用 ASP.NET Core 設定與 User Secrets 管理敏感資料：

- **OpenAI 設定**: 模型 `gpt-4.1-nano`，API 金鑰透過 User Secrets 設定
- **GitHub Models**: 端點位於 `https://models.github.ai/inference`
- **Gemini**: 模型 `gemini-2.5-flash`
- **Langfuse**: 可觀察性平台位於 `http://localhost:3000`

設定機密資料：
```bash
dotnet user-secrets set "OpenAI:ApiKey" "your-api-key"
dotnet user-secrets set "GitHub:ApiKey" "your-github-token"
dotnet user-secrets set "Gemini:ApiKey" "your-gemini-key"
```

## 架構

### 核心結構
- **Program.cs**: 具有編號範例選擇的互動式控制台
- **Samples/**: 依功能組織（SemanticKernel、Agent、AutoGen）
- **Models/**: 設定類別和輔助工具
- **Infrastructure/**: Langfuse 和 OpenTelemetry 的服務擴充
- **Python/**: 使用 CSnakes 執行時期的 Token 計數工具

### 核心元件
- **Semantic Kernel 整合**: 聊天補全、函式呼叫、對話歷史
- **代理程式框架**: 具有外掛程式支援的基本代理程式
- **縮減器模式**: 對話截斷和摘要，用於 Token 管理
- **可觀察性**: OpenTelemetry 追蹤與 Langfuse 整合
- **Python 整合**: CSnakes 用於基於 tiktoken 的 Token 計數

### 範例分類
- **2.x 範例**: Semantic Kernel 基礎（聊天、歷史、函式呼叫）
- **3.x 範例**: 具有外掛程式的代理程式實作
- **4.x 範例**: AutoGen 多代理程式協作（開發中）

## 相依性

主要套件：
- Microsoft.SemanticKernel (v1.65.0)
- Microsoft.SemanticKernel.Agents.Core
- Microsoft.SemanticKernel.Connectors.Google
- Microsoft.ML.Tokenizers 含資料模型
- CSnakes.Runtime 用於 Python 互通性
- OpenTelemetry 匯出器用於可觀察性

## 開發注意事項

- 應用程式提供互動式選單用於測試不同的 AI 情境
- 設定遵循「透過 Secret Manager 設定」模式確保安全性
- Python 環境設定在 `Python/` 目錄，支援虛擬環境
- 每個範例都是獨立的，展示特定的 Semantic Kernel 功能