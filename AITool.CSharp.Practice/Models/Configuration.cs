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
    
    /// <summary>
    /// 取得設定實例
    /// </summary>
    public static IConfiguration GetConfiguration()
    {
        if (_configuration == null)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Development"}.json", optional: true)
                .AddEnvironmentVariables();
                
            _configuration = builder.Build();
        }
        
        return _configuration;
    }
    
    /// <summary>
    /// 取得 OpenAI API 金鑰（優先順序：環境變數 -> appsettings.json）
    /// </summary>
    /// <returns>API 金鑰，如果未設定則拋出例外</returns>
    /// <exception cref="InvalidOperationException">當 API 金鑰未設定時拋出</exception>
    public static string GetOpenAIApiKey()
    {
        // 1. 先檢查環境變數
        var envApiKey = Environment.GetEnvironmentVariable(OpenAIApiKeyEnvVar);
        if (!string.IsNullOrEmpty(envApiKey))
        {
            return envApiKey;
        }
        
        // 2. 再檢查 appsettings.json
        var config = GetConfiguration();
        var configApiKey = config["OpenAI:ApiKey"];
        if (!string.IsNullOrEmpty(configApiKey) && configApiKey != "your_openai_api_key_here")
        {
            return configApiKey;
        }
        
        throw new InvalidOperationException(
            $"請設定 OpenAI API 金鑰。可以使用以下任一方式：\n" +
            $"1. 環境變數: set {OpenAIApiKeyEnvVar}=your_api_key_here (Windows)\n" +
            $"2. 環境變數: export {OpenAIApiKeyEnvVar}=your_api_key_here (Linux/Mac)\n" +
            $"3. 修改 appsettings.Development.json 中的 OpenAI:ApiKey 設定\n" +
            $"4. 使用 launchSettings.json 中的 environmentVariables"
        );
    }
    
    /// <summary>
    /// 檢查 OpenAI API 金鑰是否已設定
    /// </summary>
    /// <returns>如果已設定則回傳 true，否則回傳 false</returns>
    public static bool IsOpenAIApiKeyConfigured()
    {
        try
        {
            var apiKey = GetOpenAIApiKey();
            return !string.IsNullOrEmpty(apiKey);
        }
        catch
        {
            return false;
        }
    }
    
    /// <summary>
    /// 取得 OpenAI 模型名稱
    /// </summary>
    /// <returns>模型名稱，預設為 gpt-3.5-turbo</returns>
    public static string GetOpenAIModel()
    {
        var config = GetConfiguration();
        return config["OpenAI:Model"] ?? "gpt-3.5-turbo";
    }
}
