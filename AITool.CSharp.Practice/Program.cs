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

Console.WriteLine(await kernel.InvokePromptAsync("What color is the sky?"));

