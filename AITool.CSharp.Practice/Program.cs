using AITool.CSharp.Practice.Models.Settings;
using AITool.CSharp.Practice.Samples;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var build = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) => { config.AddUserSecrets<Program>(); })
    .ConfigureServices((context, services) =>
    {
        // Register configuration settings
        services.Configure<OpenAISettings>(context.Configuration.GetSection("OpenAI"));
        services.Configure<GitHubSettings>(context.Configuration.GetSection("GitHub"));
        services.Configure<GeminiSettings>(context.Configuration.GetSection("Gemini"));
        // Register your services
        services.AddSingleton<Sample_1_GitHubOpenAI>();
        services.AddSingleton<Sample_2_0_SemanticKernel_ChatCompletion>();
        services.AddSingleton<Sample_2_1_SemanticKernelWithGitHub_ChatCompletion>();
        services.AddSingleton<Sample_2_2_1_1_SemanticKernelWithGitHub_ChatCompletion_Reducer_Truncation>();
        services.AddSingleton<Sample_2_2_1_2_SemanticKernelWithGitHub_ChatCompletion_Reducer_Summarization>();
        services.AddSingleton<Sample_2_2_SemanticKernelWithGitHub_ChatCompletion_History>();
        services.AddSingleton<Sample_2_3_SemanticKernel_FunctionCalling>();
        services.AddSingleton<Sample_2_4_SemanticKernel_FunctionCalling_Gemini>();
        services.AddSingleton<Sample_3_1_SemanticKernel_Agent>();
        services.AddSingleton<Sample_3_1_SemanticKernel_Agent_Plugins>();
    })
    .Build();

// 1. 使用 OpenAI SDK 基本詢問
// build.Services
//     .GetRequiredService<Sample_1_GitHubOpenAI>()
//     .Execute();

// 2. 使用 SemanticKernel 範例
// 2.0 使用 SemanticKernel + OpenAI APIKey 基本詢問
// await build.Services
//     .GetRequiredService<Sample_2_0_SemanticKernel_ChatCompletion>()
//     .ExecuteAsync();

// 2.1 使用 SemanticKernel + GitHub OpenAI 基本詢問
// await build.Services
//     .GetRequiredService<Sample_2_1_SemanticKernelWithGitHub_ChatCompletion>()
//     .ExecuteAsync();

// 2.2 使用 SemanticKernel + GitHub Model 聊天紀錄
// await build.Services
//     .GetRequiredService<Sample_2_2_SemanticKernelWithGitHub_ChatCompletion_History>()
//     .ExecuteAsync();

// 2.2.1 使用 SemanticKernel + GitHub Model 聊天紀錄 + Reducer 截斷只保留前x次訊息 範例
await build.Services
    .GetRequiredService<Sample_2_2_1_1_SemanticKernelWithGitHub_ChatCompletion_Reducer_Truncation>()
     .ExecuteAsync();

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
