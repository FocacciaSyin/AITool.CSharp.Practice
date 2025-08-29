using AITool.CSharp.Practice.Models.Settings;
using Microsoft.Extensions.Configuration;

namespace AITool.CSharp.Practice.Models;

/// <summary>
/// 應用程式設定管理類別
/// </summary>
public static class Configuration
{
    /// <summary>
    /// OpenAI API 金鑰環境變數名稱
    /// </summary>
    public const string OpenAIApiKeyEnvVar = "OPENAI_API_KEY";

    private static IConfiguration? _configuration;
    private static OpenAISettings  _openAiSettings;
    
    /// <summary>
    /// 取得設定實例
    /// </summary>
    public static IConfiguration GetConfiguration()
    {
        if (_configuration == null)
        {
            var environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Development";
            var basePath = AppContext.BaseDirectory;

            var builder = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
                .AddUserSecrets<Program>()
                .AddEnvironmentVariables();

            _configuration = builder.Build();
        }

        return _configuration;
    }

    /// <summary>
    /// 取得 OpenAI API 金鑰
    /// </summary>
    /// <returns>API 金鑰，如果未設定則拋出例外</returns>
    /// <exception cref="InvalidOperationException">當 API 金鑰未設定時拋出</exception>
    public static OpenAISettings GetOpenAIApiKey()
    {
        if (_openAiSettings == null)
        {
            var configuration = GetConfiguration();
            _openAiSettings = new OpenAISettings();
            
            // 使用 Bind 方法將配置綁定到模型
            configuration.GetSection("OpenAI").Bind(_openAiSettings);
        }
        
        return _openAiSettings;
    }
}
