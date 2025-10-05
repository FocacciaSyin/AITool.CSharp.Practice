using System.Text.Encodings.Web;
using AITool.CSharp.Practice.Infrastructure;
using AITool.CSharp.Practice.Models.Settings;
using AITool.CSharp.Practice.Samples;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CSnakes.Runtime;
using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = Host.CreateApplicationBuilder(args);
builder.Configuration.AddUserSecrets<Program>();

// Register configuration settings
builder.Services.Configure<OpenAISettings>(builder.Configuration.GetSection("OpenAI"));
builder.Services.Configure<GitHubSettings>(builder.Configuration.GetSection("GitHub"));
builder.Services.Configure<GeminiSettings>(builder.Configuration.GetSection("Gemini"));
builder.Services.Configure<LangfuseSettings>(builder.Configuration.GetSection("Langfuse"));

// 設定 CSnakes Python 環境
var home = Path.Join(Directory.GetCurrentDirectory(), "Python");
builder.Services
    .WithPython()
    .WithHome(home)
    .WithVirtualEnvironment(Path.Combine(home, ".venv"))
    .FromRedistributable();

// Register your services
builder.Services.AddSingleton<Sample_1_GitHubOpenAI>();
builder.Services.AddSingleton<Sample_1_3_CSnakes_TokenCounting>();
builder.Services.AddSingleton<Sample_1_4_TokenCounting>();
builder.Services.AddSingleton<Sample_2_0_SemanticKernel_ChatCompletion>();
builder.Services.AddSingleton<Sample_2_0_1_SemanticKernel_ChatCompletion_ResponseFormat>();
builder.Services.AddSingleton<Sample_2_0_2_SemanticKernel_Article_QA>();
builder.Services.AddSingleton<Sample_2_1_SemanticKernelWithGitHub_ChatCompletion>();
builder.Services.AddSingleton<Sample_2_2_1_1_SemanticKernelWithGitHub_ChatCompletion_Reducer_Truncation>();
builder.Services.AddSingleton<Sample_2_2_1_2_SemanticKernelWithGitHub_ChatCompletion_Reducer_Summarization>();
builder.Services.AddSingleton<Sample_2_2_SemanticKernelWithGitHub_ChatCompletion_History>();
builder.Services.AddSingleton<Sample_2_3_SemanticKernel_FunctionCalling>();
builder.Services.AddSingleton<Sample_2_4_SemanticKernel_FunctionCalling_Gemini>();
builder.Services.AddSingleton<Sample_3_1_SemanticKernel_Agent>();
builder.Services.AddSingleton<Sample_3_1_SemanticKernel_Agent_Plugins>();

// builder.Services.AddCustomOpenTelemetry();
builder.Services.AddLangfuseOpenTelemetry();

var build = builder.Build();

// 1. 使用 OpenAI SDK 基本詢問
// build.Services
//     .GetRequiredService<Sample_1_GitHubOpenAI>()
//     .Execute();

// 1.3 使用 CSnakes + tiktoken 計算 Token 數量
// await build.Services
//     .GetRequiredService<Sample_1_3_CSnakes_TokenCounting>()
//     .ExecuteAsync();

// 1.4 使用 Microsoft.ML.Tokenizers 計算 Token 數量
// await build.Services
//     .GetRequiredService<Sample_1_4_TokenCounting>()
//     .ExecuteAsync();

// 2. 使用 SemanticKernel 範例
// 2.0 使用 SemanticKernel + OpenAI APIKey 基本詢問
// await build.Services
//     .GetRequiredService<Sample_2_0_SemanticKernel_ChatCompletion>()
//     .ExecuteAsync();

// 2.0.1 使用 SemanticKernel + OpenAI APIKey 回傳指定 Model
// await build.Services
//     .GetRequiredService<Sample_2_0_1_SemanticKernel_ChatCompletion_ResponseFormat>()
//     .ExecuteAsync();

// 2.0.2 使用 SemanticKernel + OpenAI APIKey 讀取文章生成 Q&A
await build.Services
    .GetRequiredService<Sample_2_0_2_SemanticKernel_Article_QA>()
    .ExecuteAsync();

// 2.1 使用 SemanticKernel + GitHub OpenAI 基本詢問
// await build.Services
//     .GetRequiredService<Sample_2_1_SemanticKernelWithGitHub_ChatCompletion>()
//     .ExecuteAsync();

// 2.2 使用 SemanticKernel + GitHub Model 聊天紀錄
// await build.Services
//     .GetRequiredService<Sample_2_2_SemanticKernelWithGitHub_ChatCompletion_History>()
//     .ExecuteAsync();

// 2.2.1 使用 SemanticKernel + GitHub Model 聊天紀錄 + Reducer 截斷只保留前x次訊息 範例
// await build.Services
//     .GetRequiredService<Sample_2_2_1_1_SemanticKernelWithGitHub_ChatCompletion_Reducer_Truncation>()
//      .ExecuteAsync();

// await build.Services
//     .GetRequiredService<Sample_2_2_1_2_SemanticKernelWithGitHub_ChatCompletion_Reducer_Summarization>()
//     .ExecuteAsync();

// 2.3 使用 SemanticKernel + OpenAI API Key +  Function Calling 範例
// await build.Services
//     .GetRequiredService<Sample_2_3_SemanticKernel_FunctionCalling>()
//     .ExecuteAsync();

// 2.4 使用 SemanticKernel + Gemini API Key +  Function Calling 範例
// await build.Services
//     .GetRequiredService<Sample_2_4_SemanticKernel_FunctionCalling_Gemini>()
//     .ExecuteAsync();

// 3.1 使用 Agent
// await build.Services
//     .GetRequiredService<Sample_3_1_SemanticKernel_Agent>()
//     .ExecuteAsync();

// 3.2 使用 Agent + Plugins
// await build.Services
//     .GetRequiredService<Sample_3_1_SemanticKernel_Agent_Plugins>()
//     .ExecuteAsync();
