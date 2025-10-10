using AITool.CSharp.Practice.Models.Settings;
using AITool.CSharp.Practice.Samples;
using AITool.CSharp.Practice.Samples._2_SemanticKernel;
using AITool.CSharp.Practice.Samples._3_Agent;
using AITool.CSharp.Practice.Samples._4_AutoGen.dotnet;
using CSnakes.Runtime;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

// Load configuration
var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: true)
    .AddUserSecrets<Program>()
    .Build();

var builder = Host.CreateApplicationBuilder(args);
builder.Configuration.AddUserSecrets<Program>();

// 設定 CSnakes Python 環境
var home = Path.Join(Directory.GetCurrentDirectory(), "Python");
builder.Services
    .WithPython()
    .WithHome(home)
    .WithVirtualEnvironment(Path.Combine(home, ".venv"))
    .WithPipInstaller()
    .FromRedistributable();

builder.Services.AddSingleton<Sample_1_3_CSnakes_TokenCounting>();

var app = builder.Build();

// Load settings
var gitHubSettings = configuration.GetSection("GitHub").Get<GitHubSettings>()!;
var openAISettings = configuration.GetSection("OpenAI").Get<OpenAISettings>()!;
var geminiSettings = configuration.GetSection("Gemini").Get<GeminiSettings>()!;

// Define allSamples collection for all examples
var allSamples = new List<(string, Func<Task>)>
{
    ("1.0 [OpenAI.Chat] Use Github Market Place Token", async () => await Sample_1_OpenAISDK_GithubMarketPlace.RunAsync(gitHubSettings)),
    ("1.3 [CSnake] Token Counting", async () => await app.Services.GetRequiredService<Sample_1_3_CSnakes_TokenCounting>().RunAsync()),
    ("1.4 [Microsoft.ML.Tokenizers] Token Counting", async () => await Sample_1_4_TokenCounting.RunAsync()),
    ("2.0 [SemanticKernel] Chat Completion", async () => await Sample_2_0_SemanticKernel_ChatCompletion.RunAsync(openAISettings)),
    ("2.0.1 [SemanticKernel] Chat Completion Response Format", async () => await Sample_2_0_1_SemanticKernel_ChatCompletion_ResponseFormat.RunAsync(openAISettings)),
    ("2.0.2 [SemanticKernel] Article QA", async () => await Sample_2_0_2_SemanticKernel_Article_QA.RunAsync(openAISettings)),
    ("2.1 [SemanticKernel] with GitHub Chat Completion", async () => await Sample_2_1_SemanticKernelWithGitHub_ChatCompletion.RunAsync(gitHubSettings)),
    ("2.2.1.1 [SemanticKernel] with GitHub Reducer Truncation", async () => await Sample_2_2_1_1_SemanticKernelWithGitHub_ChatCompletion_Reducer_Truncation.RunAsync(gitHubSettings)),
    ("2.2.1.2 [SemanticKernel] with GitHub Reducer Summarization", async () => await Sample_2_2_1_2_SemanticKernelWithGitHub_ChatCompletion_Reducer_Summarization.RunAsync(gitHubSettings)),
    ("2.2 [SemanticKernel] with GitHub Chat Completion History", async () => await Sample_2_2_SemanticKernelWithGitHub_ChatCompletion_History.RunAsync(gitHubSettings)),
    ("2.3 [SemanticKernel] Function Calling", async () => await Sample_2_3_SemanticKernel_FunctionCalling.RunAsync(openAISettings)),
    ("2.4 [SemanticKernel] Function Calling Gemini", async () => await Sample_2_4_SemanticKernel_FunctionCalling_Gemini.RunAsync(geminiSettings)),
    ("3.1 [SemanticKernel] Agent", async () => await Sample_3_1_SemanticKernel_Agent.RunAsync(openAISettings)),
    ("3.1 [SemanticKernel] Agent Plugins", async () => await Sample_3_1_SemanticKernel_Agent_Plugins.RunAsync(openAISettings)),
    ("4.0 [AutoGen] Basic Q&A", async () => await Sample_4_0_AutoGen_CSharp.RunAsync(openAISettings)),
};


Console.WriteLine("可以使用的範例:\n\n");
var idx = 1;
var map = new Dictionary<int, (string, Func<Task>)>();
foreach (var sample in allSamples)
{
    map.Add(idx, sample);
    Console.WriteLine("{0}. {1}", idx++, sample.Item1);
}

Console.WriteLine("\n\n輸入你想要測試的案例編號:");

while (true)
{
    var input = Console.ReadLine();
    if (input == "exit")
    {
        break;
    }

    var val = Convert.ToInt32(input);
    if (!map.ContainsKey(val))
    {
        Console.WriteLine("Invalid choice");
    }
    else
    {
        Console.WriteLine("執行： {0}", map[val].Item1);
        await map[val].Item2.Invoke();
    }
}
