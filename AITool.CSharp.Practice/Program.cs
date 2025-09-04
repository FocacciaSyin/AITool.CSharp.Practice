using AITool.CSharp.Practice.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

IConfiguration? _configuration;

var environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Development";
var basePath = AppContext.BaseDirectory;

var configurationBuilder = new ConfigurationBuilder()
    .SetBasePath(basePath)
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
    .AddUserSecrets<Program>()
    .AddEnvironmentVariables();

_configuration = configurationBuilder.Build();

var openAiApiKey = _configuration.GetSection("OpenAI").Get<OpenAISettings>();

// 設定 OpenAI 聊天完成服務
var kernel = Kernel.CreateBuilder()
    .AddOpenAIChatCompletion(
        modelId: openAiApiKey.Model,
        apiKey: openAiApiKey.ApiKey)
    .Build();

Console.WriteLine(openAiApiKey.Model);

//1. 基本詢問的功能
Console.WriteLine(await kernel.InvokePromptAsync("你現在使用的模型是甚麼??"));

//1.1 使用 Copilot Model 

//2. 使用 Function Calling 功能

