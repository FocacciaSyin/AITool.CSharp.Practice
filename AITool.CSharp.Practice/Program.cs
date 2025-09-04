using System.ClientModel;
using System.Text;
using AITool.CSharp.Practice.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using OpenAI;
using OpenAI.Chat;

IConfiguration? _configuration;

var basePath = AppContext.BaseDirectory;

var configurationBuilder = new ConfigurationBuilder()
    .SetBasePath(basePath)
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddUserSecrets<Program>()
    .AddEnvironmentVariables();

_configuration = configurationBuilder.Build();

var openAiApiKey = _configuration.GetSection("OpenAI").Get<OpenAISettings>();
var githubApiKey = _configuration.GetSection("GitHub").Get<GitHubSettings>();

// 1. Kernel Build 基本範例
// var kernel = Kernel.CreateBuilder()
//     .AddOpenAIChatCompletion(
//         modelId: openAiApiKey.Model,
//         apiKey: openAiApiKey.ApiKey)
//     .Build();
// Console.WriteLine(openAiApiKey.Model);

// 2. 使用 Github Model
var openAIOptions = new OpenAIClientOptions()
{
    Endpoint = new Uri(githubApiKey.EndPoint)
};
var client = new ChatClient(githubApiKey.ModelId, new ApiKeyCredential(githubApiKey.ApiKey), openAIOptions);

List<ChatMessage> messages = new List<ChatMessage>()
{
    new SystemChatMessage(""),
    new UserChatMessage("What is the capital of France?"),
};

var requestOptions = new ChatCompletionOptions()
{
    Temperature = 1f,
    TopP = 1f,
};

var response = client.CompleteChat(messages, requestOptions);
Console.WriteLine(response.Value.Content[0].Text);

//1. 基本詢問的功能
// var history = new ChatHistory(systemMessage: "你是一個天氣學家");
// var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

// Console.WriteLine(await kernel.InvokePromptAsync("你現在使用的模型是甚麼??"));

//1.1 使用 Copilot Model 


//2. 使用 Function Calling 功能
