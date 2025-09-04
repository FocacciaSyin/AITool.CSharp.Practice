using AITool.CSharp.Practice.Models;
using AITool.CSharp.Practice.Samples;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var build = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        // Register configuration settings
        services.Configure<OpenAISettings>(context.Configuration.GetSection("OpenAI"));
        services.Configure<GitHubSettings>(context.Configuration.GetSection("GitHub"));

        // Register your services
        services.AddSingleton<Sample_1_GitHubOpenAI>();
        services.AddSingleton<Sample_2_SemanticKernel_ChatCompletion>();
    })
    .Build();


// var sample1 = build.Services.GetRequiredService<Sample_1_GitHubOpenAI>();
// sample1.Execute();


var sample2 = build.Services.GetRequiredService<Sample_2_SemanticKernel_ChatCompletion>();
await sample2.ExecuteAsync();
